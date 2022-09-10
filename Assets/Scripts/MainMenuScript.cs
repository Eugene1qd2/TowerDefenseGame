using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class MainMenuScript : MonoBehaviour
{
    [SerializeField] Sprite speed1;
    [SerializeField] Sprite speed2;

    public bool isSpeed = false;
    public void ExitFromGame()
    {
        Application.Quit();
    }
    public void StartGame()
    {
        SceneManager.LoadScene(1);
        Time.timeScale = 1f;
    }
    public void OpenSettings()
    {
        SceneManager.LoadScene(3);
    }
    public void OpenInformation()
    {
        SceneManager.LoadScene(4);
    }
    public void OpenMainMenu()
    {
        SceneManager.LoadScene(0);
    }
    public void ChangeGameSpeed(GameObject gameObject)
    {
        if(isSpeed)
        {
            Time.timeScale = 1f;
            gameObject.GetComponent<Image>().sprite = speed2;
            isSpeed = false;
        }
        else
        {
            Time.timeScale = 2f;
            gameObject.GetComponent<Image>().sprite = speed1;
            isSpeed = true;
        }
    }
}

