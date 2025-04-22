using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoSceneLoader : MonoBehaviour
{
    public string nextScene = "Escena2";
    public float waitTime = 5f;

    private void Start()
    {
        FadeController fade = FindObjectOfType<FadeController>();
        if (fade != null)
        {
            fade.LoadSceneAfterDelay(nextScene, waitTime);
        }
        else
        {
            Debug.LogError("No se encontró el FadeController en la escena.");
        }
    }
}
