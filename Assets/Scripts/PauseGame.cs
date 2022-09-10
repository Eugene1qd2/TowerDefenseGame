using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseGame : MonoBehaviour
{
    public bool isPaused = false;
    float timeScale = 1f;
    [SerializeField] GameObject pauseMenu;


    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (isPaused)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }
    }
    public void Pause()
    {
        isPaused = true;
        timeScale = Time.timeScale;
        Time.timeScale = 0f;
        pauseMenu.active = true;
    }
    public void Resume()
    {
        isPaused = false;
        Time.timeScale = timeScale;
        pauseMenu.active = false;
    }
    public void Restart()
    {
        SceneManager.LoadScene(1);
    }
}
