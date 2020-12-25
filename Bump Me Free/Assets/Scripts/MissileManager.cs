using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MissileManager : MonoBehaviour
{
    public Transform missile;
    public int amount;
    public float interval;
    public Vector3 pos;

    void Start()
    {
        for (int i = 0; i < amount; i++)
        {
            Instantiate(missile, pos, Quaternion.identity);
        }
        StartCoroutine(Spawn());
    }

    IEnumerator Spawn()
    {
        yield return new WaitForSeconds(interval);
        for (int i = 0; i < amount; i++)
        {
            Instantiate(missile, pos, Quaternion.identity);
        }
        StartCoroutine(Spawn());
    }
}
