using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Screenplay
{
    public class ActorAnimation : MonoBehaviour
    {
        private string _currentState;
        private  Animator _anim;

        public void Initialize(Animator animator)
        {
            _anim = animator;
        }




        public void ChangeAnimationState(string newState)
        {
            if (_currentState == newState) return;

            _anim.Play(newState);
            _currentState = newState;
        }


        public bool isAnimationFinished() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

        public bool getCurrentAnimationName(string isAnimation) => _anim.GetCurrentAnimatorStateInfo(0).IsName(isAnimation);
    }
    

}

