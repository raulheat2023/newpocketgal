using UnityEngine;

public class LogoSceneController : MonoBehaviour
{
    public FadeController fadeController; // Referencia al script de FadeController

    private void Start()
    {
        // Espera 3 segundos antes de iniciar la transición
        Invoke("TransitionToMainMenu", 6f);
    }

    private void TransitionToMainMenu()
    {
        // Llama a la función de FadeController para realizar el fade out y cambiar de escena
        fadeController.LoadNextScene("MainMenu");
    }
}
