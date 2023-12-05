using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingPlatform : MonoBehaviour
{
    public Transform startPoint;
    public Transform endPoint;

    public float speed = 2f;
    public bool moveAutomatically = true;

    void Update()
    {
        if (moveAutomatically)
        {
            MovePlatform();
        }
    }

    void MovePlatform()
    {

        Vector2 currentPos = Vector3.Lerp(startPoint.position, endPoint.position, Mathf.PingPong(Time.time * speed, 1f));

        transform.position = currentPos;
        
    }

    public void ToggleAutoMovement()
    {
        moveAutomatically = !moveAutomatically;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.transform.SetParent(transform);
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.collider.tag == "Player")
        {
            collision.collider.transform.SetParent(null);
        }
    }
}
