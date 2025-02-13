using UnityEngine;

public class BallIdentifier : MonoBehaviour
{
    public int ballID; // ID único de cada bola

    private void Start()
    {
        Debug.Log($"🎱 Bola {ballID} creada.");
    }
}