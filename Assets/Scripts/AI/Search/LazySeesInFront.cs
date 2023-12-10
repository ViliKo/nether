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
        private AudioEntity _audio;
        [SerializeField] private AnimationClip blowAnimation;
        [SerializeField] private AudioClip iSeePlayerAudio;
        [SerializeField] private AnimationClip idleAnimation;

        public override void Enter()
        {
            if (_vf == null) _vf = _aiController.GetComponentInChildren<VisionField>();
            if (_anim == null) _anim = _aiController.AIAnimation;
            if (_audio == null) _audio = _aiController.audioEntity;

            _anim.ChangeAnimationState(idleAnimation.name);

        }

        public override void Update()
        {
            if (_vf.IseePlayer())
            {
                _anim.ChangeAnimationState(blowAnimation.name);
                _audio.PlayState(iSeePlayerAudio, 1.3f);
      
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

