using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResolutionManager : MonoBehaviour
{
    public Text resText;

    void Start()
    {
        Debug.Log($"Display.main: {Display.main.systemHeight}:{Display.main.systemWidth}");
        Debug.Log($"Screen: {Screen.height}:{Screen.width}");
        resText.text = $"Display.main: {Display.main.systemHeight}:{Display.main.systemWidth}\nScreen: {Screen.height}:{Screen.width}";
    }
}
