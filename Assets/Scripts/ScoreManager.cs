using UnityEngine;
using TMPro;
using System.IO;


public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI textoPuntaje;
    private int puntaje = 0;
    private string rutaArchivo;

    void Start()
    {
        rutaArchivo = Application.persistentDataPath + "/puntaje.json";
        CargarPuntaje();
        ActualizarUI();
    }

    public void SumarPuntos(int cantidad)
    {
        puntaje += cantidad;
        GuardarPuntaje();
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        textoPuntaje.text = "Puntaje: " + puntaje;
    }

    private void GuardarPuntaje()
    {
        string json = JsonUtility.ToJson(new PuntajeData(puntaje));
        File.WriteAllText(rutaArchivo, json);
    }

    private void CargarPuntaje()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            PuntajeData data = JsonUtility.FromJson<PuntajeData>(json);
            puntaje = data.puntaje;
        }
    }

    public int ObtenerPuntaje()
    {
        return puntaje;
    }

    [System.Serializable]
    private class PuntajeData
    {
        public int puntaje;
        public PuntajeData(int p) { puntaje = p; }
    }
}
