using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{

    [CreateAssetMenu(menuName = "AI/Search/HobbleConeSearchState")]
    public class HobbleConeSearchState : AIState<AIBaseController>
    {
        private BoxCollider2D _bc;
        private Rigidbody2D _rb;
        private SpriteRenderer _sr;
        private AudioEntity _audio;
        private AIAnimation _anim;
        public float walkSpeed = 10f;
        public AnimationClip walkStart;
        public AnimationClip walkEnd;
        [SerializeField] private AudioClip walkSound;
        public LayerMask land;
        int layerToIgnore;

        VisionField _vf;

        public override void Enter()
        {
            if (_bc == null) _bc = _aiController.GetComponent<BoxCollider2D>();
            if (_rb == null) _rb = _aiController.GetComponent<Rigidbody2D>();
            if (_sr == null) _sr = _aiController.GetComponent<SpriteRenderer>();
            if (_vf == null) _vf = _aiController.GetComponentInChildren<VisionField>();
            if (_audio == null) _audio = _aiController.audioEntity;
            if (_anim == null) _anim = _aiController.AIAnimation;

            layerToIgnore = ~LayerMask.GetMask("VisionField");
            _anim.ChangeAnimationState(walkStart.name);
            _audio.PlayState(walkSound, 2f, false, true);
        }


        public override void Update()
        {
            if(_anim.getCurrentAnimationName(walkStart.name))
            {
                Vector2 localBottomRight = new Vector2(_bc.offset.x + _bc.size.x / 2f + .4f, _bc.offset.y - _bc.size.y / 2f);

                Vector2 bottomRightCorner = (Vector2)_aiController.transform.TransformPoint(localBottomRight);
                RaycastHit2D hit = Physics2D.Raycast(bottomRightCorner, Vector2.down, .1f, land);

                if (hit.collider != null)
                    Debug.DrawRay(new Vector3(bottomRightCorner.x, bottomRightCorner.y, 0), new Vector3(0, -.1f, 0), Color.green);
                else
                {
                    _sr.transform.localScale = new Vector2(_sr.transform.localScale.x * -1, 1);
                    Debug.DrawRay(new Vector3(bottomRightCorner.x, bottomRightCorner.y, 0), new Vector3(0, -.1f, 0), Color.red);
                }

                
                _rb.velocity = new Vector2(_sr.transform.localScale.x * walkSpeed * Time.deltaTime, 0f);

                if (_anim.isAnimationFinished())
                    _anim.ChangeAnimationState(walkEnd.name);
            }
            else if (_anim.getCurrentAnimationName(walkEnd.name))
            {
                _rb.velocity = new Vector2(0f,0f);
                //Debug.Log("It has ended");
                if (_anim.isAnimationFinished())
                    _anim.ChangeAnimationState(walkStart.name);
            }


            if (_vf.IseePlayer())
            {
                Vector2 direction = (_vf.PosOfPlayer - (Vector2)_bc.transform.position);
                RaycastHit2D hit = Physics2D.Raycast(_bc.transform.position, direction, Mathf.Infinity, layerToIgnore);

                Debug.Log("I found thing with this tag" + hit.collider.tag);


                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    Debug.DrawRay(new Vector3(_bc.transform.position.x, _bc.transform.position.y, 0), new Vector3(direction.x, direction.y, 0), Color.green);
                    _aiController.SetAttackState();
                }
                else
                {
                    Debug.DrawRay(new Vector3(_bc.transform.position.x, _bc.transform.position.y, 0), new Vector3(direction.x, direction.y, 0), Color.red);
                }
            }

        }


        public override void Exit()
        {

        }

    }

}
