using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{

    [CreateAssetMenu(menuName = "AI/ManiacSearchState")]
    public class ManiacSearchState : AIState<AIBaseController>
    {
        [SerializeField] private float searchDuration = 5f;
        [SerializeField] private float moveSpeed = 50f;
        public LayerMask land;
        public float oscillationFrequency = 3f;
        public AudioClip searchManiacAudio;
        private float direction = 1f;

        private BoxCollider2D _bc;
        private Rigidbody2D _rb;

        



        public override void Enter()
        {
            if (_bc == null) _bc = _aiController.GetComponent<BoxCollider2D>();
            if (_rb == null) _rb = _aiController.GetComponent<Rigidbody2D>();
            direction = 1f;

            AudioManager.Instance.PlaySound(searchManiacAudio, _aiController.transform.position);
        }

        public override void Update()
        {
            

            AudioManager.Instance.UpdateSoundPosition(_aiController.transform.position);
            MoveRightToLeftWithSin();
        }

        public override void Exit()
        {

        }

        private void MoveRightToLeftWithSin()
        {
            if (!RaycastHitRight())
            {
                direction *= -1;
                //Debug.Log("The direction shifted to" + direction);
                _aiController.transform.localScale = new Vector3(direction, _aiController.transform.localScale.y, _aiController.transform.localScale.z);
            }

            float speedMultiplier = Mathf.Sin(Time.time * oscillationFrequency);
            float movementSpeed = moveSpeed + moveSpeed * Mathf.Abs(speedMultiplier);

            Vector2 movement = new Vector2(direction * movementSpeed, 0);
            //Debug.Log("Movement speed of goblin" + movement);
            _rb.velocity = movement;
        }

        private bool RaycastHitRight()
        {
            Vector2 localBottomRight = new Vector2(_bc.offset.x + _bc.size.x / 2f, _bc.offset.y - _bc.size.y / 2f);
            
            Vector2 bottomRightCorner = (Vector2)_aiController.transform.TransformPoint(localBottomRight);

            RaycastHit2D hit = Physics2D.Raycast(bottomRightCorner, Vector2.down, .1f, land);


            if (hit.collider != null)
            {
                Debug.DrawRay(new Vector3(bottomRightCorner.x, bottomRightCorner.y, 0), new Vector3(0, -.1f, 0), Color.green);
                return true;
                
            }
            else
            {
                Debug.DrawRay(new Vector3(bottomRightCorner.x, bottomRightCorner.y, 0), new Vector3(0, -.1f, 0), Color.red);
                return false;
            }
        }

        private bool RaycastHitLeft()
        {
            Vector2 localBottomLeft = new Vector2(_bc.offset.x - _bc.size.x / 2f, _bc.offset.y - _bc.size.y / 2f);

            Vector2 bottomLeftCorner = (Vector2)_aiController.transform.TransformPoint(localBottomLeft);

            RaycastHit2D hit = Physics2D.Raycast(bottomLeftCorner, Vector2.down, .1f);

            if (hit.collider != null)
            {
                Debug.DrawRay(new Vector3(bottomLeftCorner.x, bottomLeftCorner.y, 0), new Vector3(0, -.1f, 0), Color.green);
                return true;
            }
            else 
            {

                Debug.DrawRay(new Vector3(bottomLeftCorner.x, bottomLeftCorner.y, 0), new Vector3(0, -.1f, 0), Color.red);
                return false; 
            }

        }
    }
}

