using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialLevel : MonoBehaviour
{
    public GameObject text;
    public GameObject text2;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.transform.tag != "Player") { return; }
        text.SetActive(false);
        text2.SetActive(false);
    }
}
