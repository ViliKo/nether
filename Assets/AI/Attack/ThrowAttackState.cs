using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{

    [CreateAssetMenu(menuName = "AI/Attack/ThrowAttackState")]
    public class ThrowAttackState : AIState<AIBaseController>
    {

        public GameObject rockPrefab;
        private AIAnimation _anim;
        private Rigidbody2D _rb;
        public AnimationClip throwAnimation;
        public AnimationClip throwEndAnimation;
        private VisionField _vf;
        private BoxCollider2D _bc;
        private SpriteRenderer _sr;

        public override void Enter()
        {
            if (_anim == null) _anim = _aiController.AIAnimation;
            if (_rb == null) _rb = _aiController.GetComponent<Rigidbody2D>();
            if (_vf == null) _vf = _aiController.GetComponentInChildren<VisionField>();
            if (_bc == null) _bc = _aiController.GetComponent<BoxCollider2D>();
            if (_sr == null) _sr = _aiController.GetComponent<SpriteRenderer>();

            _rb.velocity = Vector2.zero;

            _anim.ChangeAnimationState(throwAnimation.name);
        }

        public override void Update()
        {
            if(_anim.getCurrentAnimationName(throwAnimation.name) && _anim.isAnimationFinished())
            {

                // Instantiate the rockPrefab
                GameObject rock = Instantiate(rockPrefab);

                // Set the position and rotation in local space
                rock.GetComponent<ThrowedRock>().target = _vf.PosOfPlayer;
                rock.transform.parent = _aiController.transform; // Make it a child of the AI object
                rock.transform.localPosition = new Vector3(0.2200241f, -0.5887735f, 0f); // Set local position
                rock.transform.localRotation = Quaternion.Euler(0f, 0f, -32.592f); // Set local rotation

                _anim.ChangeAnimationState(throwEndAnimation.name);
            } 

            if (_anim.getCurrentAnimationName(throwEndAnimation.name) && _anim.isAnimationFinished())
            {
                _bc.enabled = false;
            }
        }

        public override void Exit()
        {

        }

    }
}

