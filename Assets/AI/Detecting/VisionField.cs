using System;
using UnityEngine;

public class VisionField : MonoBehaviour
{

    bool iSeeThePlayer = false;
    [HideInInspector]
    public Vector2 PosOfPlayer;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            Debug.Log("I see player from visionfield");
            iSeeThePlayer = true;
            PosOfPlayer = collision.transform.position;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PosOfPlayer = collision.transform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            iSeeThePlayer = false;
            PosOfPlayer = Vector2.zero;
        }
            
    }

    public bool IseePlayer()
    {
        return iSeeThePlayer;
    }
}
