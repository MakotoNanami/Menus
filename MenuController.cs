using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenuController : MonoBehaviour
{
    [Header("Main Menu Variables")]
    public GameObject mainMenuUI;
    public GameObject optionsMenuUI;
    public GameObject loadscreen;
    public GameObject[] lights;
    public Animation loadingscreenFade;

    public Transform endMarker;
    public Transform startMarker;
    public GameObject flashLight;
    public int speed;
    public bool optionsOn;

    public bool isMainMenuOpen;
    public bool isOptionsOpen;

    private void Start()
    {
        lights[0].SetActive(true);
        lights[1].SetActive(true);
    }

    void Update()
    {
        OptionsMove();
    }

    //SCENE MANAGER INVOLVED
    public void StartGame()
    {
        SceneManager.LoadScene("Loading");
        lights[0].SetActive(false);
        lights[1].SetActive(false);
        lights[2].SetActive(false);
        lights[3].SetActive(false);
        //mainMenuUI.SetActive(false);
        loadscreen.SetActive(true);
        //loadingscreenFade.Play();
    }

    public void LoadLevel2()
    {
        SceneManager.LoadScene("Level2");
    }

    public void LoadLevel3()
    {
        SceneManager.LoadScene("GameEnd");
    }

    public void QuitGameButton()
    {
        Application.Quit();
    }

    //MOVES CAMERA
    public void OptionsMove()
    {
        if (optionsOn == true)
        {
            transform.position = Vector3.Lerp(transform.position, endMarker.position, speed * Time.deltaTime);
            flashLight.SetActive(true);
        }
        else if (optionsOn == false)
        {
            transform.position = Vector3.Lerp(transform.position, startMarker.position, speed * Time.deltaTime);
            flashLight.SetActive(false);
        }
    }

    //TOGGLES OPTIONS MENU ON
    public void ToggleOptionsMenu()
    {
        if (!optionsOn)
        {
            mainMenuUI.SetActive(false);
            optionsMenuUI.SetActive(true);
            optionsOn = true;
        }
        else
        {
            mainMenuUI.SetActive(true);
            optionsMenuUI.SetActive(false);
            optionsOn = false;
        }
    }

    //FOR TESTING/DEBUGGING
    public void DeletePlayerprefs()
    {
        PlayerPrefs.DeleteKey("graphicsPrefsSaved");
        PlayerPrefs.DeleteKey("graphicsSlider");
        PlayerPrefs.DeleteKey("wantedResolutionX");
        PlayerPrefs.DeleteKey("wantedResolutionY");
        PlayerPrefs.DeleteKey("windowedModeToggle");

        PlayerPrefs.DeleteKey("audioPrefsSaved");
        PlayerPrefs.DeleteKey("mainVolumeF");
        PlayerPrefs.DeleteKey("fxVolumeF");
        PlayerPrefs.DeleteKey("musicVolumeF");
    }
}
