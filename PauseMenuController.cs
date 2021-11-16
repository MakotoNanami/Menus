using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class PauseMenuController : MonoBehaviour
{
    public static bool GameIsPaused = false;

    public GameObject pauseMenuUI;
    public GameObject pauseOptionsUI;

    public GameObject[] puzzleUI;

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (GameIsPaused)
            {
                ResumeGame();
            }
            else
            {
                PauseGame();
            }
        }
        /*if (Input.GetKeyDown(KeyCode.T))
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }*/
    }

    //PAUSE MENU FUNCTIONS
    public void ResumeGame()
    {
        pauseMenuUI.SetActive(false);
        pauseOptionsUI.SetActive(false);
        Time.timeScale = 1f;
        GameIsPaused = false;
        PlayerStats.ps.canMove = true;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    public void PauseGame()
    {
        for (int i = 0; i < puzzleUI.Length; i++)
        {
            puzzleUI[i].SetActive(false);
        }
        pauseMenuUI.SetActive(true);
        Time.timeScale = 0f;
        GameIsPaused = true;
        PlayerStats.ps.canMove = false;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        StartCoroutine(DelayMouseUnlock());
    }

    public void ReturnToMainMenu()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        Time.timeScale = 1f;
        PlayerStats.ps.canMove = true;
        GameIsPaused = false;
        SceneManager.LoadScene("MainMenu");
    }

    public void ToggleOptionsOn()
    {
        pauseMenuUI.SetActive(false);
        pauseOptionsUI.SetActive(true);
    }

    public void ToggleOptionsOff()
    {
        pauseMenuUI.SetActive(true);
        pauseOptionsUI.SetActive(false);
    }

    IEnumerator DelayMouseUnlock()
    {
        yield return new WaitForSecondsRealtime(0.2f);
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

}
