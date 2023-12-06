using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThrowedRock : MonoBehaviour
{
    public GameObject enemy;  // Reference to the enemy GameObject with BoxCollider2D
    public LayerMask layerMask;
    private float _timer;
    private bool _collided;
    private Rigidbody2D _rb;

    public Vector2 target;      // The target point in world space
    public float throwSpeed = 10f; // Speed of the throw
    public float gravity = 9.81f;  // Acceleration due to gravity

    void Start()
    {
        Debug.Log("im here man and");
        // Ignore collisions between GameObjects with "Player" and "Enemy" tags
        Physics2D.IgnoreCollision(GetComponent<BoxCollider2D>(), enemy.GetComponent<BoxCollider2D>());
        _timer = 0;


        _rb = GetComponent<Rigidbody2D>();

        // Calculate initial velocity to reach the target
        Vector2 velocity = CalculateInitialVelocity();

        // Apply impulse force to the rock
        _rb.AddForce(velocity, ForceMode2D.Impulse);

    }

    private void Update()
    {
        if(_collided)
            _timer += Time.deltaTime;

        if (_timer > 2)
            Destroy(this.gameObject);
        

        //Debug.Log("Timer is this" + _timer); //
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        _collided = true;
    }

    Vector2 CalculateInitialVelocity()
    {
        // Calculate the direction to the target
        Vector2 direction = target - new Vector2(transform.position.x, transform.position.y);


        // Flip the initial speed if the target is to the left
        float initialSpeedX = (direction.x < 0) ? -throwSpeed : throwSpeed;

        // Calculate the horizontal distance
        float horizontalDistance = direction.x;

        // Calculate the vertical distance
        float verticalDistance = direction.y;

        // Calculate the initial velocity using the projectile motion equations
        float initialSpeedY = (verticalDistance + 0.5f * gravity * Mathf.Pow(horizontalDistance / initialSpeedX, 2)) / (horizontalDistance / initialSpeedX);

        // Create the initial velocity vector
        Vector2 velocity = new Vector2(initialSpeedX, initialSpeedY);

        return velocity;
    }
}
