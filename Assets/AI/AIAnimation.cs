using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{
    public class AIAnimation
    {

        private string _currentState;
        private readonly Animator _anim;
        private readonly Transform _transform;

        public AIAnimation(Animator animator, Transform transform)
        {
            _anim = animator;
            _transform = transform;
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


