using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour
{
    // Función para iniciar el juego
    public void StartGame()
    {
        Debug.Log("Botón 'Comenzar el Juego' presionado. Cargando escena StartGame...");
        SceneManager.LoadScene("StartGame");
    }

    // Función para abrir la galería
    public void OpenGallery()
    {
        Debug.Log("Botón 'Galería' presionado. Cargando escena Gallery...");
        SceneManager.LoadScene("Gallery");
    }

    // Función para abrir las opciones del juego
    public void OpenGameOptions()
    {
        Debug.Log("Botón 'Opciones del Juego' presionado. Cargando escena GameOptions...");
        SceneManager.LoadScene("GameOptions");
    }

    // Función para salir del juego
    public void ExitGame()
    {
        Debug.Log("Botón 'Salir del Juego' presionado. Cerrando aplicación...");
        Application.Quit();

        // Nota: Application.Quit no funciona en el Editor de Unity.
        // Este mensaje adicional ayuda a verificar en el Editor.
#if UNITY_EDITOR
        Debug.LogWarning("Nota: Application.Quit no funciona en el Editor. Usa una compilación para probarlo.");
#endif
    }
}
