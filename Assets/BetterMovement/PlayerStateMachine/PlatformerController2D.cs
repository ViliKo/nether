using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine { 
    public class PlatformerController2D : MonoBehaviour
    {
        private Collider2D hit;
        [HideInInspector]
        public float hitDistance;
        public LayerMask layerMask;


        private bool PerformRaycast(Vector3 origin, Vector2 direction, float length)
        {
            RaycastHit2D hitResult = Physics2D.Raycast(origin, direction, length, layerMask);

            if (hitResult.collider != null)
            {
                
                hit = hitResult.collider;
                hitDistance = hitResult.distance;
                Debug.DrawRay(origin, direction * length, Color.green);
                return true;
            }
            else
            {
                
                Debug.DrawRay(origin, direction * length, Color.red);
                return false;
            }
        }

        public bool VerticalRaycasts(CapsuleCollider2D cc, float extraVerticalHeight)
        {
            Vector3 originPos = new Vector3(cc.bounds.center.x, cc.bounds.center.y, 0);
            float raycastDistance = cc.bounds.extents.y + extraVerticalHeight;

            return PerformRaycast(originPos, Vector2.down, raycastDistance);
        }


        public bool HorizontalRaycastsOriginBottom(float vectorDir, CapsuleCollider2D cc, float length)
        {
            Vector2 wallraycast = new Vector2(vectorDir, 0);
            Vector3 originPosBottom = new Vector3(cc.bounds.center.x, (cc.bounds.center.y - cc.bounds.extents.y), 0);
            return PerformRaycast(originPosBottom, wallraycast, cc.bounds.extents.x + length);
        }


        public bool HorizontalRaycastsOriginBottomUp(float vectorDir, CapsuleCollider2D cc, float length)
        {
            Vector2 wallraycast = new Vector2(vectorDir, 0);
            Vector3 originPosBottomUp = new Vector3(cc.bounds.center.x, (cc.bounds.center.y - cc.bounds.extents.y) + 0.2f, 0);
            return PerformRaycast(originPosBottomUp, wallraycast, cc.bounds.extents.x + length);
        }


        public bool HorizontalRaycastsOriginUp(float vectorDir, CapsuleCollider2D cc, float length)
        {
            Vector2 wallraycast = new Vector2(vectorDir, 0);
            Vector3 originPosUp = new Vector3(cc.bounds.center.x, (cc.bounds.center.y + cc.bounds.extents.y -.1f), 0);
            return PerformRaycast(originPosUp, wallraycast, cc.bounds.extents.x + length);
        }


        public bool HorizontalRaycastsOriginUpLower(float vectorDir, CapsuleCollider2D cc, float length)
        {
            Vector2 wallraycast = new Vector2(vectorDir, 0);
            Vector3 originPosUpLower = new Vector3(cc.bounds.center.x, (cc.bounds.center.y + cc.bounds.extents.y - 0.2f), 0);
            return PerformRaycast(originPosUpLower, wallraycast, cc.bounds.extents.x + length);
        }

    }

}