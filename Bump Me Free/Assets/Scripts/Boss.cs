using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boss : MonoBehaviour
{
    public int hp;
    public Transform player;
    public GameObject blocker;
    CameraShake shake;
    Color colorNorm;
    Color colorHit;
    CrossSceneManager crossSceneManager;

    void Start()
    {
        shake = FindObjectOfType<CameraShake>();
        ColorUtility.TryParseHtmlString("#FF0066", out colorNorm);
        ColorUtility.TryParseHtmlString("#00FF99", out colorHit);
        crossSceneManager = FindObjectOfType<CrossSceneManager>().GetComponent<CrossSceneManager>();
    }

    void Update()
    {
        if(hp <= 0)
        {
            blocker.SetActive(false);
        }
    }

    void OnCollisionEnter2D(Collision2D other)
    {
        if(other.transform.tag == "Player")
        {
            hp--;
            player.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
            player.gameObject.GetComponent<Player>().count = 2;
            //player.position = new Vector3(0f, -.5f, 0f);
            StartCoroutine(HitEffect());
            if (PlayerPrefs.GetInt("Muted", 0) == 0)
            {
                crossSceneManager.PlayExplosionSound();
            }
            shake.Execute(1f, .25f);
        }
    }

    IEnumerator HitEffect()
    {
        gameObject.GetComponent<SpriteRenderer>().color = colorHit;
        yield return new WaitForSeconds(1f);
        gameObject.GetComponent<SpriteRenderer>().color = colorNorm;
    }
}
