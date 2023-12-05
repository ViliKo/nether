using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{
    [CreateAssetMenu(menuName = "AI/Search/LazySeesInFronSearch")]
    public class LazySeesInFront : AIState<AIBaseController>
    {
        VisionField _vf;
        AIAnimation _anim;
        public AnimationClip blowAnimation;
        public AnimationClip idleAnimation;

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

