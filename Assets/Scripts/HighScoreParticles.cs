using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighScoreParticles : MonoBehaviour
{
    void Start() {
        var particles = GetComponent<ParticleSystem>().main;
        particles.stopAction = ParticleSystemStopAction.Callback;
    }
    
    void OnParticleSystemStopped()
    {
        Debug.Log("Stop");
        GameObject.Find("HighScorePanel").SetActive(false);
    }
}
