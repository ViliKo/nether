
using UnityEngine;

namespace StateMachine
{

    public class PlayerAnimation
    {
        private string _currentState;
        private readonly Animator _anim;
        private readonly Transform _transform;

        public PlayerAnimation(Animator animator, Transform transform)
        {
            if (animator != null)
            {
                _anim = animator;
                _transform = transform;
            }
            else
            {
                Debug.LogWarning("Animator component is null in PlayerAnimation constructor.");
            }
        }

        public void ChangeAnimationState(string newState)
        {
            if (_anim == null)
            {
                Debug.LogError("Animator is null in ChangeAnimationState");
                return;
            }
            if (_currentState == newState || _anim == null) return;

            _anim.Play(newState);
            _currentState = newState;
        }

        public void AdjustSpriteRotation(float xInput)
        {
            if (xInput != 0)
            {
                _transform.localScale = new Vector3(-Mathf.Sign(xInput), 1, 1);
            }
        }

        public bool isAnimationFinished() => _anim.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1;

        public bool getCurrentAnimationName(string isAnimation) => _anim.GetCurrentAnimatorStateInfo(0).IsName(isAnimation);


    }
}

