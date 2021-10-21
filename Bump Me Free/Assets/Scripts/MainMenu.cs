using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Unity.RemoteConfig;

public class MainMenu : MonoBehaviour
{
    public string levelName;
    public int offset;
    public GameObject[] skins;
    public Button continueButton;
    public Image muted, unmuted;
    public GameObject updatePanel;

    public Color[] glowColors;
    public GameObject[] glowingTexts;
    float t;
    int colorIndex;

    public bool editor;
    public bool actualMenu;

    GameManager game;

    public struct userAttributes { }
    public struct appAttributes { }

    public string currentVersion;

    public GameObject BMFLogo;
    public Text coinText;
    public bool isFriday;

    private void Start()
    {
        StartCoroutine(RunConfig(1f));

        if (actualMenu)
        {
            if (PlayerPrefs.GetInt("Level", 1) == 1)
            {
                continueButton.interactable = false;
                continueButton.transform.GetComponentInChildren<Text>().color = new Color(1f, 1f, 1f, 0.5f);
            }
        }

        game = FindObjectOfType<GameManager>();

        if (BMFLogo)
        {
            Debug.Log(System.DateTime.Now.DayOfWeek);

            if(System.DateTime.Now.DayOfWeek == System.DayOfWeek.Friday)
            {
                isFriday = true;
            }

            if (isFriday)
            {
                BMFLogo.SetActive(true);
                Debug.Log("Bump Me Friday Event");
            }

            coinText.text = PlayerPrefs.GetInt("Coins", 0).ToString();
        }
    }

    private void Update()
    {
        if (actualMenu)
        {
            for (int i = 0; i < skins.Length; i++)
            {
                skins[i].SetActive(false);
            }
            skins[PlayerPrefs.GetInt("Skin", 0)].SetActive(true);

            if (PlayerPrefs.GetInt("Muted", 0) == 1)
            {
                muted.gameObject.SetActive(true);
                unmuted.gameObject.SetActive(false);
            }
            else
            {
                muted.gameObject.SetActive(false);
                unmuted.gameObject.SetActive(true);
            }
        }

        /*foreach (GameObject text in glowingTexts)
        {
            Color lerpColor = Color.Lerp(text.GetComponent<Outline>().effectColor, glowColors[colorIndex], 1 * Time.deltaTime);
            t = Mathf.Lerp(t, 1f, 1 * Time.deltaTime);
            if (t > 0.9f)
            {
                t = 0f;
                colorIndex++;
                colorIndex = (colorIndex >= glowColors.Length) ? 0 : colorIndex;
            }
            text.GetComponent<Outline>().effectColor = lerpColor;
        }*/
    }

    public void Play()
    {
        SceneManager.LoadScene(levelName);
    }

    public void Quit()
    {
    #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
    #else
        Application.Quit();
    #endif
    }

    public void NewRun()
    {
        SceneManager.LoadScene(levelName);
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Lives", 9);
        PlayerPrefs.SetInt("FadeStart", 0);
    }

    public void Continue()
    {
        SceneManager.LoadScene(PlayerPrefs.GetInt("Level") + offset);
        PlayerPrefs.SetInt("FadeStart", 0);
    }

    #region Minigames
    public void Minigames()
    {
        SceneManager.LoadScene("Minigames");
    }

    public void EndlessMode()
    {
        SceneManager.LoadScene("Endless");
    }
    #endregion

    public void Skin(string dir)
    {
        if(dir == "Left")
        {
            if(PlayerPrefs.GetInt("Skin", 0) == 0)
            {
                PlayerPrefs.SetInt("Skin", skins.Length - 1);
            } else
            {
                PlayerPrefs.SetInt("Skin", PlayerPrefs.GetInt("Skin") - 1);
            }
        } else if(dir == "Right")
        {
            if (PlayerPrefs.GetInt("Skin", 0) == skins.Length - 1)
            {
                PlayerPrefs.SetInt("Skin", 0);
            }
            else
            {
                PlayerPrefs.SetInt("Skin", PlayerPrefs.GetInt("Skin") + 1);
            }
        }
    }

    public void ToggleSound()
    {
        if(PlayerPrefs.GetInt("Muted", 0) == 1)
        {
            PlayerPrefs.SetInt("Muted", 0);
        }
        else
        {
            PlayerPrefs.SetInt("Muted", 1);
        }
    }

    IEnumerator RunConfig(float delay)
    {
        yield return new WaitForSeconds(delay);
        ConfigManager.FetchCompleted += CheckForUpdate;
        ConfigManager.FetchConfigs<userAttributes, appAttributes>(new userAttributes(), new appAttributes());
    }

    void CheckForUpdate(ConfigResponse response)
    {
        string latestVersion = ConfigManager.appConfig.GetString("Version");
        currentVersion = Application.version;

        Debug.Log($"{currentVersion}; {latestVersion};");

        if (editor) return;
        if (currentVersion == latestVersion) return;
        updatePanel.SetActive(true);
    }

    public void Credits()
    {
        SceneManager.LoadScene("Credits");
    }

    public void SendToPlayStore()
    {
        Application.OpenURL("market://details?id=" + Application.identifier);
    }
}