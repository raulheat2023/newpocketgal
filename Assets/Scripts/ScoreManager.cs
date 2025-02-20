using UnityEngine;
using TMPro;
using System.IO;
using System.Collections.Generic;


public class ScoreManager : MonoBehaviour
{
    public TextMeshProUGUI textoPuntaje;
    private int puntaje = 0;
    private List<string> aciertos = new List<string>();
    private List<string> aciertosnumeros = new List<string>();
    private string rutaArchivo;

    [System.Serializable]
    private class ScoreData
    {
        public int puntaje;
        public List<string> aciertos;
        public List<string> aciertosnumeros;
    }

    void Start()
    {
        rutaArchivo = Application.persistentDataPath + "/puntaje.json";
        CargarPuntaje();
        puntaje = 0;
        aciertos.Clear();
        aciertosnumeros.Clear();
        ActualizarUI();
    }

    public void RegistrarBolaIngresadaJson(string bolaNombre)
    {        
        if (bolaNombre.ToLower() == "whiteball" || bolaNombre == "turnofail")
        {
            aciertos.Add("error");
        }
        if (bolaNombre.ToLower()  == "ball1"){
            aciertosnumeros.Add("1");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball2"){
            aciertosnumeros.Add("2");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball3"){
            aciertosnumeros.Add("3");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball4"){
            aciertosnumeros.Add("4");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball5"){
            aciertosnumeros.Add("5");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball6"){
            aciertosnumeros.Add("6");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball7"){
            aciertosnumeros.Add("7");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball8"){
            aciertosnumeros.Add("8");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball9"){
            aciertosnumeros.Add("9");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball10"){
            aciertosnumeros.Add("10");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball11"){
            aciertosnumeros.Add("11");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball12"){
            aciertosnumeros.Add("12");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball13"){
            aciertosnumeros.Add("13");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball14"){
            aciertosnumeros.Add("14");
            aciertos.Add("acierto");
        }
        if (bolaNombre.ToLower()  == "ball15"){
            aciertosnumeros.Add("15");
            aciertos.Add("acierto");
        }
        GuardarPuntaje();
    }



    public void SumarPuntos(int cantidad)
    {
        puntaje += cantidad;
        GuardarPuntaje();
        ActualizarUI();
    }

    private void ActualizarUI()
    {
        textoPuntaje.text = "" + puntaje;
    }

    private void CargarPuntaje()
    {
        if (File.Exists(rutaArchivo))
        {
            string json = File.ReadAllText(rutaArchivo);
            ScoreData data = JsonUtility.FromJson<ScoreData>(json);
            puntaje = data.puntaje;
            aciertos = data.aciertos;
            aciertosnumeros = data.aciertosnumeros;
        }
    }

    private void GuardarPuntaje()
    {
        ScoreData data = new ScoreData { puntaje = puntaje, aciertos = aciertos, aciertosnumeros = aciertosnumeros };
        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(rutaArchivo, json);
    }

    

    public int ObtenerPuntaje()
    {
        return puntaje;
    }
}
