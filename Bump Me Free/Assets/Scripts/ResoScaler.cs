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
        if (SceneManager.GetActiveScene().name == "01")
        {
            canvasScaler.referenceResolution = new Vector2(4320, 2700);
            scaler = 0.15f;
        }
        else if (SceneManager.GetActiveScene().name == "Menu")
        {
            canvasScaler.referenceResolution = new Vector2(1600f, 900f);
            scaler = 0.25f;
        }
        else
        {
            canvasScaler.referenceResolution = new Vector2(4320, 2700);
            scaler = 0.9f;
        }

        canvasScaler.matchWidthOrHeight = scaler;
    }
}
