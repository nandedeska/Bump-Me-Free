using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SkinFollow : MonoBehaviour
{
    Transform player;

    private void Start()
    {
        player = FindObjectOfType<Player>().transform;
    }

    private void Update()
    {
        transform.position = player.position;
        transform.rotation = player.rotation;
    }
}
