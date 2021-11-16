using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;

public class GraphicSettings : MonoBehaviour
{
    public enum saveFormat { playerprefs, iniFile };
    public saveFormat saveAs;

    [Tooltip("Check for IOS and Windows Store Apps.")]
    public bool usePersistentDatapath;

    public Slider qualityLevelSlider;
    public Text qualityText;
    public GameObject resolutionsPanel, resButtonPrefab;
    public Text currentResolutionText;
    public Toggle windowedModeToggle;

    private GameObject resolutionsPanelParent;
    private Resolution[] resolutions;

    private float setTextQualDelay;
    private int wantedResX, wantedResY;

    private bool fullScreenMode;

    private string saveFileDataPath;

    private Coroutine setTextQualityCoR;

    private class MenuVariables
    {
        public int Qualitylevel;
        public int ResolutionX, ResolutionY;
        public bool WindowedMode;

        public string Warning;
    }

    MenuVariables saveVars;

    //SET THESE TO THE VALUES YOU WANT TO RESET TO.
    MenuVariables DefaultSettings = new MenuVariables
    {
        Qualitylevel = 1,
        ResolutionX = 0,//not used use Screen.width instead
        ResolutionY = 0,//not used use Screen.height instead
        WindowedMode = false,

        Warning = "Edit this file at your own risk!"
    };

    // Start is called before the first frame update
    void Start()
    {
        if (UnityEngine.EventSystems.EventSystem.current == null)
        {
            Debug.LogWarning("There is no Event System in the scene !! UI Elements can not detect input.");
        }

        resolutionsPanelParent = resolutionsPanel.transform.parent.parent.gameObject;

        if (!usePersistentDatapath)
        {
            saveFileDataPath = Application.dataPath + "/QualitySettings.ini"; //puts the file in the games/applications folder.
        }
        else
        {
            saveFileDataPath = Application.persistentDataPath + "/QualitySettings.ini";
        }

        SetValues();
    }

    public void SetQuality()
    {
        int graphicSetting = Mathf.RoundToInt(qualityLevelSlider.value);
        QualitySettings.SetQualityLevel(graphicSetting, true);
        qualityText.text = QualitySettings.names[graphicSetting];

        SetWindowedMode();
    }

    public void SetWindowedMode()
    {
        if (windowedModeToggle.isOn)
        {
            fullScreenMode = false;
        }
        else
        {
            fullScreenMode = true;
        }
        Screen.SetResolution(wantedResX, wantedResY, fullScreenMode);
    }

    private void SetValues()//set all settings according to the menu buttons.
    {
        //this reads how many Quality levels the game has and sices the slider accordingly.
        qualityLevelSlider.maxValue = QualitySettings.names.Length - 1;

        resolutions = Screen.resolutions;//checking the available resolution options.

        int prefResX = 0;
        int prefRezY = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].width != prefResX && resolutions[i].height != prefRezY)//prevent creating duplicate resolution buttons.
            {
                GameObject button = Instantiate(resButtonPrefab);//the button prefab.
                button.GetComponentInChildren<Text>().text = resolutions[i].width + "x" + resolutions[i].height;
                int index = i;
                button.GetComponent<Button>().onClick.AddListener(() => { SetResolution(index); });//adding a "On click" SetResolution() function to the button.
                button.transform.SetParent(resolutionsPanel.transform, false);

                prefResX = resolutions[i].width;
                prefRezY = resolutions[i].height;
            }
        }

        LoadMenuVariables(); // if any settings were saved before, this is where they are loaded and Sliders and toggles are set to the saved position.

        //reading Sliders and toggles and setting everything accordingly.
        int graphicSetting = Mathf.RoundToInt(qualityLevelSlider.value);
        QualitySettings.SetQualityLevel(graphicSetting, true);
        qualityText.text = QualitySettings.names[graphicSetting];
        SetWindowedMode();
    }

    public void SetResolution(int index) //the on click function on the res buttons.
    {
        wantedResX = resolutions[index].width;
        wantedResY = resolutions[index].height;
        Screen.SetResolution(wantedResX, wantedResY, fullScreenMode);
        currentResolutionText.text = wantedResX + "x" + wantedResY;
    }

    public void ShowResolutionOptions()//opens the dropdown menu with available res options.
    {
        resolutionsPanelParent.SetActive(!resolutionsPanelParent.activeSelf);
    }

    public void SaveMenuVariables()
    {
        if (saveAs == saveFormat.playerprefs)
        {
            PlayerPrefs.SetInt("graphicsPrefsSaved", 0);

            PlayerPrefs.SetInt("graphicsSlider", Mathf.RoundToInt(qualityLevelSlider.value));

            PlayerPrefs.SetInt("wantedResolutionX", wantedResX);
            PlayerPrefs.SetInt("wantedResolutionY", wantedResY);

            int toggle = 0;
            if (windowedModeToggle.isOn)
            {
                toggle = 1;
            }
            else
            {
                toggle = 0;
                PlayerPrefs.SetInt("windowedModeToggle", toggle);
            }
        }
        else if (saveAs == saveFormat.iniFile)
        {
            saveVars = new MenuVariables
            {
                Qualitylevel = Mathf.RoundToInt(qualityLevelSlider.value),
                ResolutionX = wantedResX,
                ResolutionY = wantedResY,
                WindowedMode = windowedModeToggle.isOn,

                Warning = "Edit this file at your own risk!"
            };

            string saveVasrJS = JsonUtility.ToJson(saveVars, true);
            File.WriteAllText(saveFileDataPath, saveVasrJS);

            saveVasrJS = null;
            saveVars = null;
        }
    }

    private void LoadMenuVariables()
    {
        if (saveAs == saveFormat.playerprefs)
        {
            if (PlayerPrefs.HasKey("graphicsPrefsSaved"))//to check if there are any.
            {
                qualityLevelSlider.value = PlayerPrefs.GetInt("graphicsSlider");

                wantedResX = PlayerPrefs.GetInt("wantedResolutionX");
                wantedResY = PlayerPrefs.GetInt("wantedResolutionY");
                currentResolutionText.text = wantedResX + "x" + wantedResY;

                int toggle = PlayerPrefs.GetInt("windowedModeToggle");
                if (toggle == 1)
                {
                    windowedModeToggle.isOn = true;
                }
                else
                {
                    windowedModeToggle.isOn = false;
                }
            }
            else //no player prefs are saved.
            {
                //if nothing was saved use the full screen resolutions
                wantedResX = Screen.width;
                wantedResY = Screen.height;
                currentResolutionText.text = Screen.width + "x" + Screen.height;//sets the text of the Screen Resolution button to the res we start with.
            }
        }
        else if (saveAs == saveFormat.iniFile)
        {
            if (File.Exists(saveFileDataPath))//to check if the file exists.
            {
                string loadedVasrJS = File.ReadAllText(saveFileDataPath);
                saveVars = JsonUtility.FromJson<MenuVariables>(loadedVasrJS);

                qualityLevelSlider.value = saveVars.Qualitylevel;

                wantedResX = saveVars.ResolutionX;
                wantedResY = saveVars.ResolutionY;
                currentResolutionText.text = wantedResX + "x" + wantedResY;

                windowedModeToggle.isOn = saveVars.WindowedMode;

                loadedVasrJS = null;
                saveVars = null;
            }
            else //no settings were saved.
            {
                //if nothing was saved use the full screen resolutions
                wantedResX = Screen.width;
                wantedResY = Screen.height;
                currentResolutionText.text = Screen.width + "x" + Screen.height;
            }
        }
    }

    public void ResetToDefault()
    {
        //Setting Sliders and toggles 
        qualityLevelSlider.value = DefaultSettings.Qualitylevel;

        wantedResX = Screen.width;
        wantedResY = Screen.height;
        currentResolutionText.text = wantedResX + "x" + wantedResY;

        windowedModeToggle.isOn = DefaultSettings.WindowedMode;

        //Reading Sliders and toggles and setting everything accordingly.
        int graphicSetting = Mathf.RoundToInt(qualityLevelSlider.value);
        QualitySettings.SetQualityLevel(graphicSetting, true);
        qualityText.text = QualitySettings.names[graphicSetting];
        SetWindowedMode();
    }

}
