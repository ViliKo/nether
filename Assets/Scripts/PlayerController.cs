using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
[RequireComponent(typeof(TrailRenderer))]
[RequireComponent(typeof(BoxCollider2D))]
[RequireComponent(typeof(PlatformerController2D))]
public class PlayerController : MonoBehaviour
{

    public static PlayerController Player { get; private set; }

    public bool visualiserOn = true;

    private TrailRenderer trail;
    private Rigidbody2D rb;
    private PlatformerController2D col;
    private BoxCollider2D bc;
    public LayerMask collisionLayer;

    public LayerMask JumpingLayerMask;

    #region Variables

    
    [Header("Jumping")]
    private bool jumpInput;
    private int maxJumps = 2;
    private int jumpsLeft;
    public float jumpSpeed = 8f;
    private bool jumpReleaseInput;
    public float normalGravityModifier;
    public float releaseGravityModifier;
    public float baseGravityScale;


    
    [Header("Movement")]
    public float inputTreshold = 0.015f;
    public float runMaxSpeed; //Target speed we want the player to reach.
    public float runAcceleration; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
    [HideInInspector] public float runAccelAmount; //The actual force (multiplied with speedDiff) applied to the player.
    public float runDecceleration; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
    [HideInInspector] public float runDeccelAmount; //Actual force (multiplied with speedDiff) applied to the player .
    [Space(10)]
    [Range(0.01f, 1)] public float accelInAir; //Multipliers applied to acceleration rate when airborne.
    [Range(0.01f, 1)] public float deccelInAir;
    public bool doConserveMomentum;
    private float horizontalMovementInputRaw;
    private float horizontalMovementInput;

    private bool isFacingRight;


    [Header("Dash")]
    public float dashWait = 0.5f;
    public float dashSpeed = 8f;
    public float dashVerticalSpeed = 1f;
    private bool dashInput;
    private bool isDashing;


    [Header("Wall Climb")]
    public float WallclimbSpeed;
    private bool isWallClimbing;
    private bool VerticalMovementInput;
    public float wallJumpAngle = 30f;
    public float baseWallJumpSpeed = 10f;
    public float extraWallJumpSpeed = 10f;
    Vector2 wallJumpSpeedVector;


    

    // time passed, timer start when the game starts
    float timePassed = 0f;


    #endregion

    private void Awake()
    {
        if (Player != null && Player != this)
        {
            Destroy(this);
        }
        else
        {
            Player = this;
        }
    }

    // Kun Peli alkaa niin laske ja aseta muuttujat
    void Start()
    {
        SetVariables();
        Calculations();
    }

    // Tanne kaikki inputtaamiseen liittyvat funktiot
    void Update()
    {
        FollowInputs();

        
    }

    // Tänne kaikki fysiikkaan liittyvat funktiot
    private void FixedUpdate()
    {
        Move();

        Wallclimb();

        Jump();

        InAir();

        ResetLeftJumps();

        WallJump();

        Dash();

        Deaccelerate();

        CheckDir();

        DeaccelerateAfterSpaceLift();

        if (isGrounded())
        {
            rb.gravityScale = baseGravityScale;
            trail.enabled = false;
        }
            

        Calculations();

        //isTouchingJumpingPlatform();

    }



    #region Move

    private void Move()
    {
        if (Mathf.Abs(horizontalMovementInputRaw) < inputTreshold || isDashing == true  || isWallClimbing == true)
        {
            horizontalMovementInput = 0;
            return;
        }

        //Calculate the direction we want to move in and our desired velocity
        float targetSpeed = horizontalMovementInput * runMaxSpeed;

        #region Calculate AccelRate
        float accelRate = 0;

        //Gets an acceleration value based on if we are accelerating (includes turning) 
        //or trying to decelerate (stop). As well as applying a multiplier if we're air borne.
        if (isGrounded()==true)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount : runDeccelAmount;
        else if (isGrounded() == false)
            accelRate = (Mathf.Abs(targetSpeed) > 0.01f) ? runAccelAmount * accelInAir : runDeccelAmount * deccelInAir;
        #endregion

        //Not used since no jump implemented here, but may be useful if you plan to implement your own
        /* 
		#region Add Bonus Jump Apex Acceleration
		//Increase are acceleration and maxSpeed when at the apex of their jump, makes the jump feel a bit more bouncy, responsive and natural
		if ((IsJumping || IsWallJumping || _isJumpFalling) && Mathf.Abs(RB.velocity.y) < Data.jumpHangTimeThreshold)
		{
			accelRate *= Data.jumpHangAccelerationMult;
			targetSpeed *= Data.jumpHangMaxSpeedMult;
		}
		#endregion
		*/

        #region Conserve Momentum
        //We won't slow the player down if they are moving in their desired direction but at a greater speed than their maxSpeed
        if (doConserveMomentum && Mathf.Abs(rb.velocity.x) > Mathf.Abs(targetSpeed) && Mathf.Sign(rb.velocity.x) == Mathf.Sign(targetSpeed) && Mathf.Abs(targetSpeed) > 0.01f && isGrounded() == false)
        {

            // && LastOnGroundTime < 0 test this in the last part of if statement

            //Prevent any deceleration from happening, or in other words conserve are current momentum
            //You could experiment with allowing for the player to slightly increae their speed whilst in this "state"
            accelRate = 0;
        }
        #endregion

        //Calculate difference between current velocity and desired velocity
        float speedDif = targetSpeed - rb.velocity.x;
        //Calculate force along x-axis to apply to thr player

        float movement = speedDif * accelRate;

        //Convert this to a vector and apply to rigidbody
        rb.AddForce(movement * Vector2.right, ForceMode2D.Force);

        /*
		 * For those interested here is what AddForce() will do
		 * RB.velocity = new Vector2(RB.velocity.x + (Time.fixedDeltaTime  * speedDif * accelRate) / RB.mass, RB.velocity.y);
		 * Time.fixedDeltaTime is by default in Unity 0.02 seconds equal to 50 FixedUpdate() calls per second
		*/

        // applies force to a rigidbody
        //rb.AddForce(movement * Vector2.right);



    } // function

    

    private void Dash()
    {
        if (dashInput == false) return;
        if (isDashing) Debug.Log("I'm dashing " + isDashing);


        StartCoroutine(DashWait(dashWait));


        IEnumerator DashWait(float wait)
        {
            isDashing = true;

            rb.AddForce(new Vector2(horizontalMovementInput * dashSpeed, dashVerticalSpeed), ForceMode2D.Impulse);

            yield return new WaitForSeconds(wait/2);

            isDashing = false;
            dashInput = false;
            rb.velocity = new Vector2(0, rb.velocity.y);

            yield return new WaitForSeconds(wait/2);

        } // IEnumerator 
    } // function

    private void Jump()
    {
        if (TouchingWall() == true) return; // if touches wall cant do normal jump
        if (jumpInput == false) return;
        if (isGrounded() == false && jumpsLeft <= 1) return; // Jos oot ilmassa ja ei oo hyppyja jaljella palaa tai et paina hyppynappia

        rb.velocity = new Vector2(rb.velocity.x, 0f);


        jumpsLeft--;
        rb.AddForce(Vector2.up * jumpSpeed, ForceMode2D.Impulse);

        jumpInput = false;

    }  // function

    private void Deaccelerate()
    {
        if (isGrounded() == true) { rb.gravityScale = baseGravityScale; return; }
        if (TouchingWall() == true) { rb.gravityScale = baseGravityScale; return; }


        if (rb.velocity.y < 0)
            rb.gravityScale = baseGravityScale * normalGravityModifier;
    }  // function


    private void DeaccelerateAfterSpaceLift()
    {
        if (TouchingWall() == true) { rb.gravityScale = baseGravityScale; return; }
        if (isGrounded() == true || jumpReleaseInput == false)
        { rb.gravityScale = baseGravityScale; jumpReleaseInput = false; return; }

        rb.gravityScale = baseGravityScale * releaseGravityModifier;




    }  // function

    private void ResetLeftJumps()
    {
        if (TouchingWall() == true) return; // jos kosketat seinää älä resettaa hyppyä
        if (isGrounded() == false) return;  // jos sä oot maassa niin resettaa hyppy määrät normaaliksi

        jumpsLeft = maxJumps;
    }  // function




    private void InAir()
    {
        if (isGrounded() == true) return;
        trail.enabled = true;

    }


    // wallclimb
    private void Wallclimb()
    {
        if (isGrounded() == true) return;
        if (TouchingWall() == false) return;

        if (visualiserOn)
            visualizeWallJump();

        // implimentaatio
        isWallClimbing = true;

        if (VerticalMovementInput)
            rb.velocity = Vector2.up * WallclimbSpeed;
        else
            rb.velocity = Vector2.up;



    } // function



    // walljump
    private void WallJump()
    {
        if (TouchingWall() == false) return; // jos osut seinään ja jump on true  (false || false)  (true && true)
        if (jumpInput == false) return;

        isWallClimbing = false;

        float dir = isFacingRight ? 1 : -1;

        float wallJumpSpeedXvector;
        float wallJumpSpeedYvector;

        wallJumpSpeedYvector = extraWallJumpSpeed + baseWallJumpSpeed;
        wallJumpSpeedXvector = wallJumpSpeedYvector * Mathf.Tan(Mathf.Deg2Rad * wallJumpAngle) * -dir;


        wallJumpSpeedVector = new Vector2(wallJumpSpeedXvector, wallJumpSpeedYvector+rb.velocity.y);

        rb.AddForce(wallJumpSpeedVector, ForceMode2D.Impulse);


        jumpInput = false;

    }  // function

    private void CheckDir()
    {


        if (horizontalMovementInput > 0)
            isFacingRight = true;

        if (horizontalMovementInput < 0)
            isFacingRight = false;
    }


    #endregion


    private void TimedVariableUpdates()
    {
        timePassed += Time.deltaTime;
        if (timePassed > 2f)
        {
            timePassed = 0f;

            //Debug.Log("Horizontal raw movement input is: " + horizontalMovementInputRaw);
            //Debug.Log("Horizontal movement input is: " + horizontalMovementInput);
            //Debug.Log("Acceleration is: " + acceleration);
            //Debug.Log("Current speed is: " + currentSpeed);
            //Debug.Log("Current runtime is: " + runTime);
            //Debug.Log("Current jumps left: " + jumpsLeft);

        } // if
    } // function


    private void Calculations()
    {
        runAccelAmount = (50 * runAcceleration) / runMaxSpeed;
        runDeccelAmount = (50 * runDecceleration) / runMaxSpeed;

        #region Variable Ranges
        runAcceleration = Mathf.Clamp(runAcceleration, 0.01f, runMaxSpeed);
        runDecceleration = Mathf.Clamp(runDecceleration, 0.01f, runMaxSpeed);
        #endregion
    }

    private void SetVariables()
    {
        trail = GetComponent<TrailRenderer>();
        rb = GetComponent<Rigidbody2D>();
        bc = GetComponent<BoxCollider2D>();
        col = GetComponent<PlatformerController2D>();
        isDashing = false;
        isWallClimbing = false;
        jumpInput = false;
        dashInput = false;
        jumpsLeft = maxJumps;
        horizontalMovementInput = 0;

        JumpingLayerMask = LayerMask.GetMask("JumpingPlatform");
       
    }

    private void FollowInputs()
    {
        horizontalMovementInputRaw = Input.GetAxis("Horizontal");

        if (horizontalMovementInputRaw != 0)
            horizontalMovementInput = horizontalMovementInputRaw;

        if (Input.GetKeyDown(KeyCode.W) || Input.GetKeyDown(KeyCode.UpArrow))
            VerticalMovementInput = true;

        if (Input.GetKeyUp(KeyCode.W) || Input.GetKeyUp(KeyCode.UpArrow))
            VerticalMovementInput = false;


        if (Input.GetKeyDown(KeyCode.X) == true)
            dashInput = true;


        if (Input.GetKeyDown(KeyCode.Space) == true)
            jumpInput = true;


        if (Input.GetKeyUp(KeyCode.Space))
            jumpReleaseInput = true;


    }



    private void OnDrawGizmos()
    {

    }

    private void visualizeWallJump()
    {

        float dir = isFacingRight ? 1 : -1;

        float wallJumpSpeedXvector;
        float wallJumpSpeedYvector;


        wallJumpSpeedYvector = (extraWallJumpSpeed + baseWallJumpSpeed);
        wallJumpSpeedXvector = wallJumpSpeedYvector * Mathf.Tan((Mathf.Deg2Rad * wallJumpAngle)) * -dir;


        wallJumpSpeedVector = new Vector2(wallJumpSpeedXvector, wallJumpSpeedYvector);


        Debug.DrawRay(
                new Vector3(bc.bounds.center.x, bc.bounds.center.y, 0),
                Vector3.Normalize(new Vector3(wallJumpSpeedVector.x, wallJumpSpeedVector.y + rb.velocity.y, 0)) * 4,
                Color.yellow);
    }


    public void TestHello()
    {
        Debug.Log("Hellou man");
    }

    private bool isGrounded()
    {
        float extraVerticalHeight = .4f;
        float extraHorizontalHeight = .2f;


        col.VerticalRaycasts(bc, collisionLayer, extraVerticalHeight, extraHorizontalHeight);

        // guard clause
        if (col.collisions.bottomLeft == false && col.collisions.bottomRight == false)
        {
            Debug.DrawRay(new Vector3(bc.bounds.center.x - bc.bounds.size.x / 2 - extraHorizontalHeight, bc.bounds.center.y, 0), Vector2.down * (bc.bounds.extents.y + extraVerticalHeight), Color.red);
            Debug.DrawRay(new Vector3(bc.bounds.center.x + bc.bounds.size.x / 2 + extraHorizontalHeight, bc.bounds.center.y, 0), Vector2.down * (bc.bounds.extents.y + extraVerticalHeight), Color.red);
            return false;
        }
        else
        {
            Debug.DrawRay(new Vector3(bc.bounds.center.x - bc.bounds.size.x / 2 - extraHorizontalHeight, bc.bounds.center.y, 0), Vector2.down * (bc.bounds.extents.y + extraVerticalHeight), Color.green);
            Debug.DrawRay(new Vector3(bc.bounds.center.x + bc.bounds.size.x / 2 + extraHorizontalHeight, bc.bounds.center.y, 0), Vector2.down * (bc.bounds.extents.y + extraVerticalHeight), Color.green);
            isWallClimbing = false;

            Debug.Log("Why isnt it working");
            return true;
        } // if

    } // function



    private bool TouchingWall()
    {
        if (isGrounded() == true) return false;
        // you need to do this wall raycast better man

        float extraHeight = .4f;

        float vectorDir;


        if (isFacingRight)
            vectorDir = 1;
        else
            vectorDir = -1;


        col.HorizontalRaycasts(vectorDir, bc, collisionLayer, extraHeight);



        //Debug.Log(vectorDir);






        if (col.collisions.top == false && col.collisions.bottom == false)
        {
            if (visualiserOn)
                col.visualizeHorizontalRaysFalse(bc, isFacingRight, extraHeight);

            isWallClimbing = false;
            return false;
        }
        else
        {
            if (visualiserOn)
                col.visualizeHorizontalRaysTrue(bc, isFacingRight, extraHeight);
            return true;
        } // if
    } // function


} // class