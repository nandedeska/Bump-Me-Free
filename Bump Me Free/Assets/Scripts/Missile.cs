using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Missile : MonoBehaviour
{
    Transform player;
    Vector3 target;
    public float speed;

    void Start()
    {
        player = GameObject.FindWithTag("Player").transform;
        target = player.position;
        //Debug.Log(target);
    }

    void Update()
    {
        var direction = target - transform.position;
        var angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
        transform.position = Vector2.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if(transform.position == target)
        {
            Destroy(gameObject);
        }
    }
}