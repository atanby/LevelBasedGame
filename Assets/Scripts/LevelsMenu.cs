using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LevelsMenu : MonoBehaviour
{
    public GameObject levelPrefab;
    public GameObject content;
    public static float place = 0.0f;
    public static int currentLevel;
    public static bool openedUIBefore = false;
    public static int remoteLevelCount = 15;

    void Start() {
        if(!PlayerPrefs.HasKey("gameOpenedBefore")) {
            Debug.Log("GAME OPENED FIRST TIME!");
            //Game opened first time it has 10 levels in assets
            PlayerPrefs.SetInt("levelCount", 10);
            //Current max level
            PlayerPrefs.SetInt("currentMaxLevel", 1);
            for(int i = 1; i < 11; i++) {
                //-1 is for no high score info
                PlayerPrefs.SetInt("levelHighScore" + i, -1);
            }
            PlayerPrefs.SetInt("gameOpenedBefore", 1);
            PlayerPrefs.Save();
        }
        PlayerPrefs.SetInt("currentMaxLevel", 5);
        PlayerPrefs.Save();

        if(!openedUIBefore) {
            for(int i = 11; i < 11 + remoteLevelCount; i++) {
                StartCoroutine(LevelDownloader.DownloadLevel(i));
            }
        }

        //Check if there is high score and play animation
        if(GameManager.score != 0 && GameManager.score > PlayerPrefs.GetInt("levelHighScore" + currentLevel)) {

            //Check if the maxlevel is finished first time
            if(PlayerPrefs.GetInt("currentMaxLevel") == currentLevel && PlayerPrefs.GetInt("levelHighScore" + currentLevel) == -1) {
                //Increase maxlevel
                PlayerPrefs.SetInt("currentMaxLevel", PlayerPrefs.GetInt("currentMaxLevel") + 1);
            }
            foreach (Transform child in transform) {
                if(child.name == "HighScorePanel") {
                    child.gameObject.SetActive(true);
                }
            }
            PlayerPrefs.SetInt("levelHighScore" + currentLevel, GameManager.score);
            PlayerPrefs.Save();
            GameObject.Find("HighScoreParticles").GetComponent<ParticleSystem>().Play();
        }

        int currentMaxLevel = PlayerPrefs.GetInt("currentMaxLevel");

        if(openedUIBefore) {
            //Then simulate button click and show only levels
            LevelsButtonClicked();
        }

        for(int i = 1; i < 1 + PlayerPrefs.GetInt("levelCount"); i++) {
            GameObject levelLabel = Instantiate(levelPrefab,content.transform);
            levelLabel.name = "level" + i;

            //Play Button
            Button playButton = levelLabel.transform.GetChild(0).gameObject.GetComponent<Button>();

            //Level Text
            TextMeshProUGUI levelText = levelLabel.transform.GetChild(1).gameObject.GetComponent<TextMeshProUGUI>();
            levelText.text= "level" + i + " - " + LevelLoader.getMoveCount(i) + " Moves";
            
            //HighScore Text
            TextMeshProUGUI highScoreText = levelLabel.transform.GetChild(2).gameObject.GetComponent<TextMeshProUGUI>();
            if(currentMaxLevel >= i) {
                int index = i;
                //Level is unlocked
                playButton.onClick.AddListener(() => {
                    currentLevel = index;
                    SceneManager.LoadScene("GameScene");
                });
                int highScore = PlayerPrefs.GetInt("levelHighScore" + i);
                if(highScore == -1) {
                    //There is no High Score
                    highScoreText.text = "No score";
                }
                else {
                    //There is a high score so display it
                    highScoreText.text = "Highest Score: " + highScore;
                }
            }

            else {
                //Level is locked
                highScoreText.text = "Locked Level";
                playButton.GetComponentInChildren<TextMeshProUGUI>().text = "Locked";
                playButton.interactable = false;
            }
            
            levelLabel.SetActive(true);
        }

        openedUIBefore = true;
        
    }

    public void LevelsButtonClicked() {
        Debug.Log("levels button clicked");
        foreach (Transform child in transform) {
            if(child.name == "LevelsPopUp") {
                child.gameObject.SetActive(true);
            }
            else if(child.name == "LevelsButton") {
                child.gameObject.SetActive(false);
            }
        }
    }

    public void CancelButtonClicked() {
        Debug.Log("cancel button clicked");
        foreach (Transform child in transform)
            {
                if(child.name == "LevelsPopUp") {
                    child.gameObject.SetActive(false);
                }
                else if(child.name == "LevelsButton") {
                    child.gameObject.SetActive(true);
                }
            }
    }
}
