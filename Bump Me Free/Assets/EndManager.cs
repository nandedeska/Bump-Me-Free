using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class EndManager : MonoBehaviour
{
    public void MainMenu()
    {
        PlayerPrefs.SetInt("Level", 1);
        PlayerPrefs.SetInt("Lives", 9);
        PlayerPrefs.SetInt("FadeStart", 0);
        SceneManager.LoadScene("Menu");
    }
}
