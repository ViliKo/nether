using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{


    [CreateAssetMenu(menuName = "AI/Search/GazeStayStillSearch")]
    public class GazeStayStillSearch : AIState<AIBaseController>
    {

        private VisionField _vf;
        private AIAnimation _anim;
        private AudioEntity _audio;
        private BoxCollider2D _bc;
        [SerializeField] private AnimationClip idleAnimation;

        int layerToIgnore;

        public override void Enter()
        {
            if (_vf == null) _vf = _aiController.GetComponentInChildren<VisionField>();
            if (_bc == null) _bc = _aiController.GetComponent<BoxCollider2D>();
            if (_anim == null) _anim = _aiController.AIAnimation;
            if (_audio == null) _audio = _aiController.audioEntity;

            layerToIgnore = ~LayerMask.GetMask("VisionField");

            _anim.ChangeAnimationState(idleAnimation.name);
        }
        public override void Update(){
            if (_vf.IseePlayer())
            {
                float distance = _vf.PosOfPlayer.x - _aiController.transform.position.x;
                float dirOfSprite = distance / Mathf.Abs(distance);

                _aiController.transform.localScale = new Vector3(dirOfSprite, 1, 1);


                Vector2 direction = (_vf.PosOfPlayer - (Vector2)_bc.transform.position);
                RaycastHit2D hit = Physics2D.Raycast(_bc.transform.position, direction, Mathf.Infinity, layerToIgnore);



                if (hit.collider != null && hit.collider.tag == "Player")
                {
                    Debug.DrawRay(new Vector3(_bc.transform.position.x, _bc.transform.position.y, 0), new Vector3(direction.x, direction.y, 0), Color.green);
                    _aiController.SetAttackState();
                }
                else
                {
                    Debug.DrawRay(new Vector3(_bc.transform.position.x, _bc.transform.position.y, 0), new Vector3(direction.x, direction.y, 0), Color.red);
                    Debug.Log("this is the hit: " +  hit.collider.gameObject.name);
                }
            }
        }

        public override void Exit(){}

        
    }
}

