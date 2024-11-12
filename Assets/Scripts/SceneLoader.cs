using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneLoader : MonoBehaviour
{
    public static void LoadGameScene() {
        SceneManager.LoadScene("GameScene");
    }
    public static void LoadMainScene() {
        SceneManager.LoadScene("MainScene");
    }
}
