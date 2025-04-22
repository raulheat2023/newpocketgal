using UnityEngine;
using UnityEngine.SceneManagement;

public class LobbyControllerController : MonoBehaviour

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
    public void goToStage()
    {
        LoadingManager.CargarEscena("tutorial");
    }
}