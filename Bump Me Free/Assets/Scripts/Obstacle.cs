using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Obstacle : MonoBehaviour
{
    [Header("General Settings")]
    public float speed;
    public float rotateSpeed;
    public float delay;

    [Header("Move Settings")]
    public bool moveRight, moveLeft, moveUp, moveDown, loopMove;
    public float timer;
    private float runTime;

    [Header("Loop Settings")]
    public float dist;
    public bool loopRight, loopLeft, loopUp, loopDown;
    private Vector3 startPos;

    [Header("Spin Settings")]
    public string dir;
    public bool spin;

    void Start()
    {
        startPos = transform.position;
        runTime = timer;
    }

    void Update()
    {
        Move();
        Loop();
        Spin();
    }

    void Move()
    {
        if (moveRight && !loopMove)
        {
            transform.position = transform.position + new Vector3(0.25f * speed * Time.deltaTime, 0f, 0f);
        }
        
        if (moveLeft && !loopMove)
        {
            transform.position = transform.position + new Vector3(-0.25f * speed * Time.deltaTime, 0f, 0f);
        }
        
        if (moveRight && loopMove)
        {
            if (runTime <= 0)
            {
                transform.position = startPos;
                runTime = timer;
            }
            else
            {
                runTime -= 1 * Time.deltaTime;
                transform.position = transform.position + new Vector3(0.25f * speed * Time.deltaTime, 0f, 0f);
            }
        } 
        
        if (moveLeft && loopMove)
        {
            if(runTime <= 0)
            {
                transform.position = startPos;
                runTime = timer;
            }
            else
            {
                runTime -= 1 * Time.deltaTime;
                transform.position = transform.position + new Vector3(-0.25f * speed * Time.deltaTime, 0f, 0f);
            }
        }

        if (moveUp && !loopMove)
        {
            transform.position = transform.position + new Vector3(0f, 0.25f * speed * Time.deltaTime, 0f);
        }
        
        if (moveDown && !loopMove)
        {
            transform.position = transform.position + new Vector3(0f, -0.25f * speed * Time.deltaTime, 0f);
        }
        
        if (moveUp && loopMove)
        {
            if (runTime <= 0)
            {
                transform.position = startPos;
                runTime = timer;
            }
            else
            {
                runTime -= 1 * Time.deltaTime;
                transform.position = transform.position + new Vector3(0f, 0.25f * speed * Time.deltaTime, 0f);
            }
        }
        
        if (moveDown && loopMove)
        {
            if (runTime <= 0)
            {
                transform.position = startPos;
                runTime = timer;
            }
            else
            {
                runTime -= 1 * Time.deltaTime;
                transform.position = transform.position + new Vector3(0f, -0.25f * speed * Time.deltaTime, 0f);
            }
        }
    }

    void Loop()
    {
        if(loopUp && loopRight)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(v.x += dist * Mathf.Sin((Time.time - delay) * speed), v.y += dist * Mathf.Sin((Time.time - delay) * speed), 0f);
        }

        if(loopUp && loopLeft)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(v.x -= dist * Mathf.Sin((Time.time - delay) * speed), v.y += dist * Mathf.Sin((Time.time - delay) * speed), 0f);
        }

        if (loopDown && loopRight)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(v.x += dist * Mathf.Sin((Time.time - delay) * speed), v.y -= dist * Mathf.Sin((Time.time - delay) * speed), 0f);
        }

        if (loopDown && loopLeft)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(v.x -= dist * Mathf.Sin((Time.time - delay) * speed), v.y -= dist * Mathf.Sin((Time.time - delay) * speed), 0f);
        }

        if (loopRight && !loopDown && !loopUp)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(v.x += dist * Mathf.Sin((Time.time - delay) * speed), transform.position.y, 0f);
        }
        
        if (loopLeft && !loopUp && !loopDown)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(v.x -= dist * Mathf.Sin((Time.time - delay) * speed), transform.position.y, 0f);
        }
        
        if (loopUp && !loopRight && !loopLeft)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(transform.position.x, v.y += dist * Mathf.Sin((Time.time - delay) * speed), 0f);
        } 
        
        if (loopDown && !loopRight && !loopLeft)
        {
            Vector3 v = startPos;
            transform.position = new Vector3(transform.position.x, v.y -= dist * Mathf.Sin((Time.time - delay) * speed), 0f);
        }
    }

    void Spin()
    {
        if (spin)
        {
            if (dir == "Left")
            {
                transform.Rotate(0f, 0f, 1f * rotateSpeed * Time.deltaTime);
            } else if (dir == "Right")
            {
                transform.Rotate(0f, 0f, -1f * rotateSpeed * Time.deltaTime);
            }
        }
    }
}
