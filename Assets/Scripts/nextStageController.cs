using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class nextStageController : MonoBehaviour
{
    [System.Serializable]
    private class StateData
    {
        public int estado = 1;  // Estado inicial
    }

    public Image imagenEstado;  // Asigna desde el Inspector
    public Sprite[] imagenes;   // Asigna los sprites en el Inspector

    private string filePath;
    private StateData stateData;

    void Start()
    {
        filePath = Path.Combine(Application.persistentDataPath, "estado.json");
        CargarEstado();
        ActualizarImagen();
        GuardarEstado();
    }

    private void CargarEstado()
    {
        if (File.Exists(filePath))
        {
            string json = File.ReadAllText(filePath);
            stateData = JsonUtility.FromJson<StateData>(json);
            stateData.estado++;  // Aumenta el estado en 1
        }
        else
        {
            stateData = new StateData();
        }
    }

    private void GuardarEstado()
    {
        string json = JsonUtility.ToJson(stateData, true);
        File.WriteAllText(filePath, json);
    }

    private void ActualizarImagen()
    {
        if (stateData.estado - 1 < imagenes.Length)
        {
            imagenEstado.sprite = imagenes[stateData.estado - 1];
        }
    }
}
