using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{
    [CreateAssetMenu(menuName = "AI/Search/LazySeesInFronSearch")]
    public class LazySeesInFront : AIState<AIBaseController>
    {
        private VisionField _vf;
        private AIAnimation _anim;
        [SerializeField] private AnimationClip blowAnimation;
        [SerializeField] private AnimationClip idleAnimation;

        public override void Enter()
        {
            if (_vf == null) _vf = _aiController.GetComponentInChildren<VisionField>();
            if (_anim == null) _anim = _aiController.AIAnimation;

            _anim.ChangeAnimationState(idleAnimation.name);

        }

        public override void Update()
        {
            if (_vf.IseePlayer())
            {
                _anim.ChangeAnimationState(blowAnimation.name);
                Debug.Log("isee playeraeraer");
      
            } else
            {
               // Debug.Log("I dont see player");
            }

            if (_anim.getCurrentAnimationName(blowAnimation.name)  && _anim.isAnimationFinished())
            {
                _aiController.SetAttackState();
            }
        }

        public override void Exit()
        {
  
        }


    }

}

