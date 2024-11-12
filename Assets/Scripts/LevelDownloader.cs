using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using System.IO;

public class LevelDownloader : MonoBehaviour
{
    public static IEnumerator DownloadLevel(int levelNumber) {

        string levelName = "";

        if(levelNumber >= 11 && levelNumber <= 15) {
            levelName = "RM_A" + levelNumber;
        }
        else if(levelNumber >= 16 && levelNumber <= 25) {
            levelName = "RM_B" + (levelNumber - 15);
        }
        string assetUrl = "https://row-match.s3.amazonaws.com/levels/" + levelName;
        string assetName = "RM_A" + levelNumber + ".txt";
        string savePath = "Assets/Resources/Levels";
        string assetPath = Path.Combine(savePath, assetName);

        UnityWebRequest www = UnityWebRequest.Get(assetUrl);
        yield return www.SendWebRequest();

        if (www.result != UnityWebRequest.Result.Success)
        {
            Debug.LogError("Failed to download asset: " + www.error);
            yield break;
        }

        byte[] assetBytes = www.downloadHandler.data;

        File.WriteAllBytes(assetPath, assetBytes);

        Debug.Log("Asset downloaded and saved to " + assetPath);
    }
}
