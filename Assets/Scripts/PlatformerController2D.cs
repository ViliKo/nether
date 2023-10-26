using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformerController2D : MonoBehaviour
{

    // collision
    public struct Collisions
    {
        public bool top;
        public bool bottom;
        public bool bottomLeft;
        public bool bottomRight;

    }
    public Collisions collisions;



    // Update is called once per frame
    void FixedUpdate()
    {
      

    }

    public void visualizeHorizontalRaysTrue(BoxCollider2D bc, bool isFacingRight, float extraHeight)
    {


        Vector2 wallraycast = Vector2.zero;
        float vectorDir = 1;

        if (isFacingRight)
            vectorDir = 1;
        else
            vectorDir = -1;


        wallraycast = new Vector2(vectorDir, 0);

        Debug.DrawRay(
            new Vector3(bc.bounds.center.x, bc.bounds.center.y + bc.bounds.size.y / 2, 0),
            wallraycast * (bc.bounds.extents.x + extraHeight),
            Color.green);

        Debug.DrawRay(
            new Vector3(bc.bounds.center.x, bc.bounds.center.y - bc.bounds.size.y / 2, 0),
            wallraycast * (bc.bounds.extents.x + extraHeight),
            Color.green);
    }

    public void visualizeHorizontalRaysFalse(BoxCollider2D bc, bool isFacingRight, float extraHeight)
    {



        Vector2 wallraycast = Vector2.zero;
        float vectorDir = 1;

        if (isFacingRight)
            vectorDir = 1;
        else
            vectorDir = -1;


        wallraycast = new Vector2(vectorDir, 0);



        Debug.DrawRay(
            new Vector3(bc.bounds.center.x, bc.bounds.center.y + bc.bounds.size.y / 2, 0),
            wallraycast * (bc.bounds.extents.x + extraHeight),
            Color.red);

        Debug.DrawRay(
            new Vector3(bc.bounds.center.x, bc.bounds.center.y - bc.bounds.size.y / 2, 0),
            wallraycast * (bc.bounds.extents.x + extraHeight),
            Color.red);
    }


    private void visualizeVerticalRaysTrue()
    {

    }

    private void visualizeVerticalRaysFalse(BoxCollider2D bc, bool isFacingRight)
    {

    }


    public void VerticalRaycasts(BoxCollider2D bc, LayerMask collisionLayer, float extraVerticalHeight, float ExtraHorizontalHeight)
    {
        // Cast a ray straight down.
       
        RaycastHit2D hitLeft = Physics2D.Raycast(
            new Vector3(bc.bounds.center.x - bc.bounds.size.x / 2 - ExtraHorizontalHeight, bc.bounds.center.y, 0),
            Vector2.down,
            bc.bounds.extents.y + extraVerticalHeight,
            collisionLayer);

        RaycastHit2D hitRight = Physics2D.Raycast(
            new Vector3(bc.bounds.center.x + bc.bounds.size.x / 2 + ExtraHorizontalHeight, bc.bounds.center.y, 0),
            Vector2.down, bc.bounds.extents.y + extraVerticalHeight,
            collisionLayer);

        if (hitLeft.collider == null)
            collisions.bottomLeft = false;
        else
            collisions.bottomLeft = true;


        if (hitRight.collider == null)
            collisions.bottomRight = false;
        else
            collisions.bottomRight = true;




    }  // function


    public void HorizontalRaycasts(float vectorDir, BoxCollider2D bc, LayerMask collisionLayer, float extraHeight)
    {
        Vector2 wallraycast;

        wallraycast = new Vector2(vectorDir, 0);

        // Cast a ray straight down
        RaycastHit2D hitTop = Physics2D.Raycast(
            new Vector3(bc.bounds.center.x, bc.bounds.center.y + bc.bounds.size.y / 2, 0),
            wallraycast,
            bc.bounds.extents.x + extraHeight,
            collisionLayer);

        RaycastHit2D hitBottom = Physics2D.Raycast(
            new Vector3(bc.bounds.center.x, bc.bounds.center.y - bc.bounds.size.y / 2, 0),
            wallraycast,
            bc.bounds.extents.x + extraHeight,
            collisionLayer);

        if (hitTop.collider == null)
            collisions.top = false;
        else
            collisions.top = true;

        if (hitBottom.collider == null)
            collisions.bottom = false;
        else
            collisions.bottom = true;

    } // function
}
