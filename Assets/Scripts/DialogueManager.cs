using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.Collections;
using System.IO;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI dialogueText;
    public Button nextButton;
    public Animator Isumi_01;
    private List<string> lines;
    private int currentLine = 0;

    [Header("Configuración de Diálogo")]
    public float velocidadEscritura = 0.05f;

    private Coroutine typingCoroutine;

    public void StartDialogueLine(string line)
    {
        if (typingCoroutine != null) StopCoroutine(typingCoroutine);
        typingCoroutine = StartCoroutine(TypeLine(line));
    }

    void Start()
    {
        CargarDialogo();
        MostrarLineaActual();
        nextButton.onClick.AddListener(MostrarSiguienteLinea);
    }

    void CargarDialogo()
    {
        string jsonPath = Path.Combine(Application.streamingAssetsPath, "dialogue.json");
        string jsonText = File.ReadAllText(jsonPath);

        DialogueData data = JsonUtility.FromJson<DialogueData>(jsonText);
        Stage stage = data.stage01[0]; // Primer stage

        nameText.text = stage.girlName;
        lines = stage.stagetext[0].stagephase00text;       
    }

    void MostrarLineaActual()
    {
        StopAllCoroutines();

        if (currentLine < lines.Count)
        {
            nextButton.interactable = true;
            // Detectar si es la última línea
            if (currentLine == lines.Count - 1)
            {
                nextButton.interactable = false;
            }
            StartDialogueLine(lines[currentLine]); // ahora controlado por StartDialogueLine
        }
        else {
            
        }
    }

    // función que muestra la siguiente línea de dialogo
    void MostrarSiguienteLinea()
    {
        currentLine++;
        MostrarLineaActual();
    }

    //coroutine con animación de texto
    IEnumerator TypeLine(string line) 
    {
        dialogueText.text = "";
        Isumi_01.SetBool("isTalking", true);
        Debug.Log("Animación iniciada");

        foreach (char letter in line.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(velocidadEscritura);  
        }

        Isumi_01.SetBool("isTalking", false);
        Debug.Log("Animación finalizada");
    }
}
