using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestScript : MonoBehaviour
{
    private bool test = true;

    // Start is called before the first frame update
    void Start()
    {

        
    }

    // Update is called once per frame
    void Update()
    {
        if (test)
            PlayerController.Player.TestHello();

        test = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {


        collision.collider.attachedRigidbody.AddForce(Vector2.up * 50000f);
    }

}
