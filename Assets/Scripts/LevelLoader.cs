using UnityEngine;
using UnityEditor;
using System.IO;
using System.Linq;
using System;
using System.Collections.Generic;
public class LevelLoader
{
   public static GridLayout LoadLevelFromTextAsset(int levelNumber) {
    
        string levelName = "Levels/RM_A" + levelNumber;
        TextAsset file = Resources.Load<TextAsset>(levelName);

        string text = file.text;

        StringReader stringReader = new StringReader(text);
        
        //LEVEL_NUMBER
        string line = stringReader.ReadLine();
        int level = Int32.Parse(line.Substring(text.IndexOf(":") + 1));

        //GRID_WIDTH
        line = stringReader.ReadLine();
        int gridWidth = Int32.Parse(line.Substring(line.IndexOf(":") + 1));
        
        //GRID_HEIGHT
        line = stringReader.ReadLine();
        int gridHeight = Int32.Parse(line.Substring(line.IndexOf(":") + 1));

        //MOVE_COUNT
        line = stringReader.ReadLine();
        int moveCount = Int32.Parse(line.Substring(line.IndexOf(":") + 1));

        //GRID
        line = stringReader.ReadLine();
        char[] itemTypeSymbols = (line.Substring(line.IndexOf(":") + 1)).Split(',').Select(s => s.Trim().ToCharArray()[0]).ToArray();

        ItemType[] itemTypes = new ItemType[itemTypeSymbols.GetLength(0)];

        for(int i = 0; i < itemTypeSymbols.Length; i++) {
            switch(itemTypeSymbols[i]) {
                case 'r':
                itemTypes[i] = ItemType.Red;
                break;
                case 'g':
                itemTypes[i] = ItemType.Green;
                break;
                case 'b':
                itemTypes[i] = ItemType.Blue;
                break;
                case 'y':
                itemTypes[i] = ItemType.Yellow;
                break;
                default:
                itemTypes[i] = ItemType.Completed;
                break;
            }
        }

        return new GridLayout(gridWidth, gridHeight, itemTypes, moveCount);
   }

   public static int getMoveCount(int levelNumber) {

        string levelName = "Levels/RM_A" + levelNumber;
        TextAsset file = Resources.Load<TextAsset>(levelName);

        string text = file.text;

        StringReader stringReader = new StringReader(text);

        //LEVEL_NUMBER
        string line = stringReader.ReadLine();

        //GRID_WIDTH
        line = stringReader.ReadLine();
        
        //GRID_HEIGHT
        line = stringReader.ReadLine();

        //MOVE_COUNT
        line = stringReader.ReadLine();
        return Int32.Parse(line.Substring(line.IndexOf(":") + 1));
        
   }


}