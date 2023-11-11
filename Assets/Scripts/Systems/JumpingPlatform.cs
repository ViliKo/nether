using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class JumpingPlatform : MonoBehaviour
{

    private float deltaX;
    private float deltaY;
    public bool visualize = true;

    public float force = 5000f;



    // Update is called once per frame
    //Debug.DrawLine(new Vector3(transform.position.x, transform.position.y, 0), new Vector3(PlayerController.Player.transform.position.x, PlayerController.Player.transform.position.y, 0), Color.black);



    private void OnCollisionEnter2D(Collision2D collision)
    {

        deltaY = PlayerController.Player.transform.position.y - transform.position.y;
        deltaX = PlayerController.Player.transform.position.x - transform.position.x;



        if (deltaY > 0)
            collision.collider.attachedRigidbody.AddForce(new Vector2(deltaX, deltaY) * force);


    }
}
