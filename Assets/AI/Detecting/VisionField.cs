using System;
using UnityEngine;

public class VisionField : MonoBehaviour
{

    bool iSeeThePlayer = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            iSeeThePlayer = true;
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
            iSeeThePlayer = false;
    }

    public bool IseePlayer()
    {
        return iSeeThePlayer;
    }
}
