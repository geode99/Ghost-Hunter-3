using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenuMethods : MonoBehaviour
{
    public void StartButton()
    {
        Debug.Log("Started the game!");
        SceneManager.LoadScene("platformahscene");
    }

    public void QuitButton()
    {
        Debug.Log("Quit the game!");
        Application.Quit();
    }

    public void CreditsButton()
    {
        Debug.Log("Opened Credits!");
        SceneManager.LoadScene("Credits");
    }
}