using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuController : MonoBehaviour

{
    public AudioSource insertCoinSource;
    public AudioClip insertCoin;
    public AudioSource insertOptionSource;
    public AudioClip insertOption;

    // Load sound effects
    void Start(){
        insertCoinSource = GetComponent<AudioSource>();
        insertOptionSource = GetComponent<AudioSource>();
    }

    // Start game function
    public void StartGame()
    {
        Debug.Log("Button clicked 'Arcade mode start");
        insertCoinSource.PlayOneShot(insertCoin);
        LoadingManager.CargarEscena("nextStage");
    }

    // Start adventure game function
    public void AdventureMode()
    {
        Debug.Log("Button clicked 'This feature is not available right now");
        insertCoinSource.PlayOneShot(insertCoin);
    }

    // Start gallery function
    public void OpenGallery()
    {
        Debug.Log("Button clicked 'This feature is not available right now");
        insertCoinSource.PlayOneShot(insertOption);
    }

    // Start option menu function
    public void OpenGameOptions()
    {
        Debug.Log("Button clicked 'Options window");
        insertCoinSource.PlayOneShot(insertOption);
        SceneManager.LoadScene("GameOptions");
    }

    // Quit game function
    public void ExitGame()
    {
        Debug.Log("Button clicked 'Quit game...");
        Application.Quit();

        // Note: Application.Quit doesn't works on editor.
        // This additional message helps to verify on the editor.
#if UNITY_EDITOR
        Debug.LogWarning("Note: Application.Quit doesn't works on editor. Use a compilation to test it.");
#endif
    }
}
