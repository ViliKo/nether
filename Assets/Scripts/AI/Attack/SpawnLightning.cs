using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace CompositeStateRunner
{
    [CreateAssetMenu(menuName = "AI/Attack/SpawnLightning")]
    public class SpawnLightning : AIState<AIBaseController>
    {

        private VisionField _vf;
        private AIAnimation _anim;
        private AudioEntity _audio;
        private BoxCollider2D _bc;
        [SerializeField] private AnimationClip attackAnimation;
        [SerializeField] private AnimationClip attackSpawnAnimation;

        public GameObject lightningPrefab;

        public override void Enter()
        {
            if (_vf == null) _vf = _aiController.GetComponentInChildren<VisionField>();
            if (_bc == null) _bc = _aiController.GetComponent<BoxCollider2D>();
            if (_anim == null) _anim = _aiController.AIAnimation;
            if (_audio == null) _audio = _aiController.audioEntity;


            _anim.ChangeAnimationState(attackAnimation.name);
        }


        public override void Update()
        {
            if (_anim.getCurrentAnimationName(attackAnimation.name) && _anim.isAnimationFinished())
            {
                _anim.ChangeAnimationState(attackSpawnAnimation.name);

                Instantiate(lightningPrefab);

            }

            if (_anim.getCurrentAnimationName(attackSpawnAnimation.name) && _anim.isAnimationFinished())
                _aiController.SetSearchState();
        }

        public override void Exit()
        {
            
        }

    }
}

