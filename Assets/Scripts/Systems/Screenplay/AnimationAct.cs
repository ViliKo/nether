using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Screenplay
{
    [CreateAssetMenu(menuName = "Screenplay/Act/AnimationAct")]
    public class AnimationAct : Act<Screenplay>
    {
        [SerializeField]
        private string actor;
        private ActorAnimation _anim;
        [SerializeField]
        private AnimationClip _actorAnimation;
      
        public override void Enter()
        {

            GameObject specificActor = FindActorByName(actor);
            Debug.Log("I found this actor" + specificActor);
            if (_anim == null) _anim = specificActor.GetComponent<ActorAnimation>();

            _anim.ChangeAnimationState(_actorAnimation.name);

        }

        public override void Update()
        {
            if (_anim.getCurrentAnimationName(_actorAnimation.name) && _anim.isAnimationFinished())
                _screenplay.ExecuteNextAction();
        }

        public override void Exit()
        {
   
        }

 
    }
}


