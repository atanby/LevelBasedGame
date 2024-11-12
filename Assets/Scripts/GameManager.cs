using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public TextMeshProUGUI moveCountText;
    public TextMeshProUGUI scoreText;
    public static GridLayout gridLayout;
    public static int score;
    
    void Start()
    {
        score = 0;
        int levelIndex = 8;
        gridLayout = LevelLoader.LoadLevelFromTextAsset(LevelsMenu.currentLevel);
    }

    // Update is called once per frame
    void Update()
    {
        if(gridLayout.moveCount <= 0) {
            //There are no moves left
            //Call level is over
            SceneManager.LoadScene("MainScene");
        }
        moveCountText.text = "MoveCount: " + gridLayout.moveCount;
        scoreText.text = "Score: " + score;
    }
}
