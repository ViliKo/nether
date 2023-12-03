using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlideFaster : MonoBehaviour
{


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.velocity.y < 0)
        {
            collision.attachedRigidbody.gravityScale = 10;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.attachedRigidbody.velocity.y < 0)
        {
            collision.attachedRigidbody.gravityScale = 2;
        }
    }
}
