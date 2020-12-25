using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Goal : MonoBehaviour
{
    GameManager game;
    Player player;
    public GameObject restartPopup;
    public bool lastLevel;
    public bool loadRestartPopup;

    void Start()
    {
        game = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        player = FindObjectOfType<Player>().GetComponent<Player>();
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.transform.tag == "Player")
        {
            if (lastLevel) { return; }
            if (loadRestartPopup)
            {
                restartPopup.SetActive(true);
            }
            else
            {
                game.Finish();
                PlayerPrefs.SetInt("Level", PlayerPrefs.GetInt("Level", 1) + 1);
                //Debug.Log(PlayerPrefs.GetInt("Level"));
            }
        }
    }
}
