using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DestroyObject : MonoBehaviour
{
    public enum Method
    {
        Button
    };

    public Method method;
    public Button button;

    private void Start()
    {
        if(method == Method.Button)
        {
            button = transform.GetChild(3).GetComponent<Button>();
            button.onClick.AddListener(SelfDestroy);
        }
    }

    public void SelfDestroy()
    {
        Destroy(gameObject);
    }
}
