using UnityEngine;
using System.Collections.Generic;

public class GameManager : MonoBehaviour
{
    public Dictionary<int, GameObject> bolas = new Dictionary<int, GameObject>();

    void Start()
    {
        // Encontrar todas las bolas en la escena
        BallIdentifier[] todasLasBolas = FindObjectsOfType<BallIdentifier>();
        foreach (BallIdentifier bola in todasLasBolas)
        {
            bolas[bola.ballID] = bola.gameObject; // Agregar la bola al diccionario
        }
    }

    public GameObject GetBallByID(int id)
    {
        if (bolas.ContainsKey(id))
        {
            return bolas[id];
        }
        return null;
    }
}