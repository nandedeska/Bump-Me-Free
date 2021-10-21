using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class EndlessManager : MonoBehaviour
{
    public Transform[] spawns;
    public Transform platform;
    public float delay;
    public float speed;
    public Text scoreText;
    [HideInInspector] public float score;
    public GameObject endlessLosePanel;
    public bool dead;

    private void Start()
    {
        StartCoroutine(Spawn(delay));
    }

    private void Update()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Platform");

        if (!dead)
        {
            score += Time.deltaTime;
            scoreText.text = Mathf.FloorToInt(score).ToString("F0");

            foreach (GameObject wall in walls)
            {
                wall.transform.position -= new Vector3(0f, Mathf.Clamp(score / 25, 1f, 10f), 0f) * Time.deltaTime;
            }
        }
    }

    IEnumerator Spawn(float delay)
    {
        int index = Random.Range(0, spawns.Length);
        Instantiate(platform, spawns[index].position, Quaternion.identity, null);
        yield return new WaitForSeconds(delay);
        StartCoroutine(Spawn(delay));
    }
}
