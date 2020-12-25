using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class ResoScaler : MonoBehaviour
{
    CanvasScaler canvasScaler;
    float scaler;
    bool is16_9;
    public bool update;

    private void Start()
    {
        canvasScaler = GetComponent<CanvasScaler>();
        canvasScaler.uiScaleMode = CanvasScaler.ScaleMode.ScaleWithScreenSize;

        Adjust();
    }


    private void Update()
    {
        if (!update) return;
        Adjust();
    }

    void Adjust()
    {
        if (Camera.main.aspect > 0.55)
        {
            canvasScaler.referenceResolution = new Vector2(4320, 2700);
            is16_9 = true;
        }
        else
        {
            canvasScaler.referenceResolution = new Vector2(1920, 1080);
            is16_9 = false;
        }

        //canvasScaler.referenceResolution = new Vector2(1920, 1080);

        if (SceneManager.GetActiveScene().name == "01")
        {
            canvasScaler.referenceResolution = new Vector2(4320, 2700);
            if (Screen.width == 1800 && Screen.height == 2560)
            {
                scaler = 0.07f;
            }
            else if (Screen.width == 1200 && Screen.height == 1920)
            {
                scaler = 0.175f;
            }
            else if (Screen.width == 720 && Screen.height == 1280)
            {
                scaler = 0.255f;
            }
            else if (Screen.width == 1080 && Screen.height == 1920)
            {
                scaler = 0.25f;
            }
            else
            {
                scaler = 0.325f;
            }
        }
        else if (SceneManager.GetActiveScene().name == "Menu")
        {
            if (is16_9)
            {
                scaler = 1f;
            }
            else
            {
                scaler = 0.25f;
            }
        }
        else
        {
            if (Screen.width == 1800 && Screen.height == 2560)
            {
                scaler = 0.925f;
            }
            else if (Screen.width == 1200 && Screen.height == 1920)
            {
                scaler = 0.925f;
            }
            else if (Screen.width == 720 && Screen.height == 1280)
            {
                scaler = 0.925f;
            }
            else if (Screen.width == 1080 && Screen.height == 1920)
            {
                scaler = 0.925f;
            }
            else
            {
                scaler = 0.2f;
            }
        }

        canvasScaler.matchWidthOrHeight = scaler;
    }
}
