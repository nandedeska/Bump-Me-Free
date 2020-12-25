using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour
{
    public string bossLvl;
    public GameObject[] waves;
    Boss boss;
    public GameObject lava;
    Vector3 lavaPos;

    void Start()
    {
        boss = FindObjectOfType<Boss>().GetComponent<Boss>();
        lavaPos = lava.transform.position;
    }

    void Update()
    {
        Boss();
    }

    void Boss()
    {
        switch (bossLvl)
        {
            case "Level10":
                switch (boss.hp)
                {
                    case 3:
                        waves[0].SetActive(true);
                        break;

                    case 2:
                        waves[0].SetActive(false);
                        waves[1].SetActive(true);
                        break;

                    case 1:
                        waves[1].SetActive(false);
                        waves[2].SetActive(true);
                        break;

                    case 0:
                        waves[2].SetActive(false);
                        StartCoroutine(Death());
                        break;
                }
                break;

            case "Level20":
                // Not yet added
                break;

            case "Level30":
                // Not yet added
                break;
        }
    }

    public void LavaReset()
    {
        lava.transform.position = lavaPos;
    }

    IEnumerator Death()
    {
        yield return new WaitForSeconds(1f);
        boss.gameObject.SetActive(false);
    }
}
