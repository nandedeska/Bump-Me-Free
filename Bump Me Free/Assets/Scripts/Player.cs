using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    Vector2 touchPos;
    Vector2 mousePos;
    public Rigidbody2D rb;
    public float force = 500f;
    [HideInInspector] public Vector3 startPos;

    Transform canvas;

    Text jumpText;
    public int jumpAmt;
    public GameObject touchButton;

    GameManager game;
    CameraShake shake;
    Boss boss;
    BossManager bossManager;

    [HideInInspector] public int count;
    [HideInInspector] public bool movement = true;

    bool jumpCheck;

    void Start()
    {
        startPos = transform.position;
        canvas = FindObjectOfType<Canvas>().transform;
        game = FindObjectOfType<GameManager>().GetComponent<GameManager>();
        shake = FindObjectOfType<CameraShake>();

        if(SceneManager.GetActiveScene().name == "Boss01")
        {
            boss = FindObjectOfType<Boss>().GetComponent<Boss>();
            bossManager = FindObjectOfType<BossManager>().GetComponent<BossManager>();
        }
        //touchButton = game.touchButton;
        //jumpText = game.jumpText;

        if(PlayerPrefs.GetInt("Lives", 9) < 0)
        {
            gameObject.SetActive(false);
        }
        {
            gameObject.SetActive(true);
        }

        jumpText = canvas.transform.GetChild(2).GetComponent<Text>();
        jumpText.text = jumpAmt.ToString();
    }

    void Update()
    {
        if (movement)
        {
            // Mobile touch
            if (Input.touchCount > 0)
            {
                Touch touch = Input.GetTouch(0);
                touchPos = Camera.main.ScreenToWorldPoint(touch.position);

                Vector2 lookDir = touchPos - rb.position;
                float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - 90f;
                rb.rotation = angle;
            }

            mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            
            Vector2 lookDir2 = mousePos - rb.position;
            float angle2 = Mathf.Atan2(lookDir2.y, lookDir2.x) * Mathf.Rad2Deg - 90f;
            rb.rotation = angle2;

            if (jumpAmt <= 0 && !jumpCheck)
            {
                game.JumpDie(1.5f);
                jumpCheck = true;
            }            
        }

        if (PlayerPrefs.GetInt("Lives", 9) < 0)
        {
            gameObject.SetActive(false);
        }
        {
            gameObject.SetActive(true);
        }

        //skin.transform.position = transform.position;
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Wall")
        {
            WallReload();
        }

        if(other.transform.tag == "Platform")
        {
            WallReload();
            jumpAmt = (int)System.Math.Pow(2, 31) - 1;
        }

        if(other.transform.tag == "Obstacle" || other.transform.tag == "Missile")
        {
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            shake.Execute(1f, .05f);
            if(SceneManager.GetActiveScene().name == "Boss01")
            {
                if(boss.hp == 1)
                {
                    transform.position = startPos + new Vector3(0f, 1f, 0f);
                    bossManager.LavaReset();
                }
                count = 2;
                WallReload();
                DestroyMissiles();
            }
            if(SceneManager.GetActiveScene().name == "Endless")
            {
                game.Die(1f, "Endless");
            }
            else
            {
                game.Die(1f);
            }
        }
    }

    void WallReload()
    {
        count++;
        //Debug.Log(count);
        if (count >= 2)
        {
            count = 0;
            rb.velocity = Vector2.zero;
            rb.angularVelocity = 0f;
            touchButton.SetActive(true);
        }
        //else
        //{
            if (PlayerPrefs.GetInt("Muted", 0) == 0)
            {
                game.crossSceneManager.PlayBounceSound();
            }
        //}
    }

    void DestroyMissiles()
    {
        GameObject[] missiles = GameObject.FindGameObjectsWithTag("Missile");
        for (int i = 0; i < missiles.Length; i++)
        {
            Destroy(missiles[i]);
        }
    }

    public void MoveForward()
    {
        if(jumpAmt <= 0) { return; }
        rb.AddForce(transform.up * force);
        touchButton.SetActive(false);
        jumpAmt--;
        jumpText.text = jumpAmt.ToString();
        if (PlayerPrefs.GetInt("Muted", 0) == 0)
        {
            game.crossSceneManager.PlayJumpSound();
        }
        //Debug.Log("moving");  
    }
}
