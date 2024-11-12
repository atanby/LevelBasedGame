using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using DG.Tweening;

public enum SwipeDirection {
    Right,
    Down,
    Left,
    Up
} 

public class GridLayout {
    private int gridWidth;
    private int gridHeight;
    private float cellWidth = 0.4f;
    private float cellHeight = 0.4f;
    private float animationDuration = 0.3f;
    private int[,] gridArray;
    private ItemType[] itemTypes;
    Dictionary<ItemType, Sprite> spriteMap = new Dictionary<ItemType, Sprite>();
    public int moveCount;

    public GridLayout(int gridWidth, int gridHeight, ItemType[] itemTypes, int moveCount) {
        this.gridWidth = gridWidth;
        this.gridHeight = gridHeight;
        this.itemTypes = itemTypes;
        this.moveCount = moveCount;

        gridArray = new int[gridWidth, gridHeight];

        InitSpriteMap();

        for(int i = 0; i < gridHeight; i++) {
            for (int j = 0; j < gridWidth; j++) {
                GameObject grid = new GameObject((i * gridWidth + j).ToString());
                grid.transform.position = new Vector2(-cellWidth * (gridWidth - 1) / 2 + j * cellWidth, cellHeight * (gridHeight - 1) / 2 -i * cellHeight);
                grid.AddComponent<SpriteRenderer>();
                grid.GetComponent<SpriteRenderer>().sprite = spriteMap[itemTypes[i * gridWidth + j]];
            }
        }
    }

    private void InitSpriteMap() {
        Sprite[] all = Resources.LoadAll<Sprite>("Sprites/royal_match_texture");
        Sprite completed = Resources.Load<Sprite>("Sprites/completed");
        Sprite redAsset = all[0];
        Sprite greenAsset = all[2];
        Sprite blueAsset = all[3];
        Sprite yellowAsset = all[1];
        Sprite completedAsset = completed;
        spriteMap.Add(ItemType.Red, redAsset);
        spriteMap.Add(ItemType.Green, greenAsset);
        spriteMap.Add(ItemType.Blue, blueAsset);
        spriteMap.Add(ItemType.Yellow, yellowAsset);
        spriteMap.Add(ItemType.Completed, completedAsset);
    }

    public void SwapElements(int index1, SwipeDirection swipeDirection) {
        int index2 = 0;
        Transform transform1 = GameObject.Find((index1).ToString()).GetComponent<SpriteRenderer>().transform;
        Transform transform2;
        switch(swipeDirection) {
            case SwipeDirection.Right:
            if((index1 + 1) % gridWidth == 0) return;
            index2 = index1 + 1;
            if(itemTypes[index1] == ItemType.Completed || itemTypes[index2] == ItemType.Completed) return;
            transform2 = GameObject.Find((index2).ToString()).GetComponent<SpriteRenderer>().transform;
            transform1.DOMoveX(transform1.position.x + cellHeight, animationDuration);
            transform2.DOMoveX(transform2.position.x - cellHeight, animationDuration);
            moveCount--;
            break;

            case SwipeDirection.Down:
            if(index1 >= gridWidth * (gridHeight - 1)) return;
            index2 = index1 + gridWidth;
            if(itemTypes[index1] == ItemType.Completed || itemTypes[index2] == ItemType.Completed) return;
            transform2 = GameObject.Find((index2).ToString()).GetComponent<SpriteRenderer>().transform;
            transform1.DOMoveY(transform1.position.y - cellHeight, animationDuration);
            transform2.DOMoveY(transform2.position.y + cellHeight, animationDuration);
            moveCount--;
            break;

            case SwipeDirection.Left:
            if(index1 % gridWidth == 0) return;
            index2 = index1 - 1;
            if(itemTypes[index1] == ItemType.Completed || itemTypes[index2] == ItemType.Completed) return;
            transform2 = GameObject.Find((index2).ToString()).GetComponent<SpriteRenderer>().transform;
            transform1.DOMoveX(transform1.position.x - cellHeight, animationDuration);
            transform2.DOMoveX(transform2.position.x + cellHeight, animationDuration);
            moveCount--;
            break;

            case SwipeDirection.Up:
            if(index1 < gridWidth) return;
            index2 = index1 - gridWidth;
            if(itemTypes[index1] == ItemType.Completed || itemTypes[index2] == ItemType.Completed) return;
            transform2 = GameObject.Find((index2).ToString()).GetComponent<SpriteRenderer>().transform;
            transform1.DOMoveY(transform1.position.y + cellHeight, animationDuration);
            transform2.DOMoveY(transform2.position.y - cellHeight, animationDuration);
            moveCount--;
            break;
        }
        ItemType temp = itemTypes[index1];
        itemTypes[index1] = itemTypes[index2];
        itemTypes[index2] = temp;
        GameObject obj1 = GameObject.Find(index1.ToString());
        GameObject obj2 = GameObject.Find(index2.ToString());
        obj1.name = index2.ToString();
        obj2.name = index1.ToString();
    }

    //Changes sprites to completed if the any of the rows is completed
    //Returns the score that is from the previous move
    public int checkIfRowsCompleted() {
        int addedScore = 0;
        for(int i = 0; i < gridHeight; i++) {
            ItemType previousItemType = itemTypes[i * gridWidth];
            for(int j = 1; j < gridWidth; j++) {
                if(previousItemType != itemTypes[i * gridWidth + j]) {
                    break;
                }
                if(j == gridWidth - 1 && itemTypes[i * gridWidth] != ItemType.Completed) {
                    //All of the row items are the same and not type of completed
                    switch(itemTypes[i * gridWidth]) {
                        case ItemType.Red:
                        addedScore += 100 * gridWidth;
                        rowCompleted(i);
                        break;
                        case ItemType.Green:
                        addedScore += 150 * gridWidth;
                        rowCompleted(i);
                        break;
                        case ItemType.Blue:
                        addedScore += 200 * gridWidth;
                        rowCompleted(i);
                        break;
                        case ItemType.Yellow:
                        addedScore += 250 * gridWidth;
                        rowCompleted(i);
                        break;
                        default:
                        break;
                    }
                }
                previousItemType = itemTypes[i * gridWidth + j];
            }
        }

        return addedScore;
    }

    public bool checkIfThereAreMoveLeft() {
        //Item type counts for each area that is between completed areas
        //0: RED, 1: GREEN, 2: BLUE 3: YELLOW
        int[] itemTypeCounts = {0, 0, 0, 0};

        for(int i = 0; i < gridHeight; i++) {
            for(int j = 0; j < gridWidth; j++) {
                switch(itemTypes[i * gridWidth + j]) {
                    case ItemType.Red:
                    itemTypeCounts[0]++;
                    break;
                    case ItemType.Green:
                    itemTypeCounts[1]++;
                    break;
                    case ItemType.Blue:
                    itemTypeCounts[2]++;
                    break;
                    case ItemType.Yellow:
                    itemTypeCounts[3]++;
                    break;
                    case ItemType.Completed:
                    //Reset itemtypes counts
                    for(int k = 0; k < itemTypeCounts.Length; k++) {
                        itemTypeCounts[k] = 0;
                    }
                    break;
                    default:
                    break;
                }

                //Check if there are any item types as many as row width
                for(int k = 0; k < itemTypeCounts.Length; k++) {
                    if(itemTypeCounts[k] >= gridWidth) {
                        //There are possible moves to complete a row
                        return true;
                    }
                }
            }
        }
        return false;
    }

    public int getClickedElementIndex(Vector2 touchPosition) {
        if(-cellWidth * gridWidth / 2 > touchPosition.x || cellWidth * gridWidth / 2 < touchPosition.x ||
        -cellHeight * gridHeight / 2 > touchPosition.y || cellHeight * gridHeight / 2 < touchPosition.y) {
            //Clicked out of bounds
            return -1;
        }
        else {
            return (int)(gridWidth * Math.Floor(Math.Abs(touchPosition.y - gridHeight * cellHeight / 2) / cellHeight) + Math.Floor((touchPosition.x + gridWidth * cellWidth/ 2) / cellWidth));
        }
    }

    private void rowCompleted(int rowNo) {
        for(int i = 0; i < gridWidth; i++) {
            itemTypes[rowNo * gridWidth + i] = ItemType.Completed;
            GameObject.Find((rowNo * gridWidth + i).ToString()).GetComponent<SpriteRenderer>().sprite = spriteMap[ItemType.Completed];
        }
    }

}
