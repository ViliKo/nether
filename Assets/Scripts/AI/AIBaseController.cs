using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{
    public class AIBaseController : MonoBehaviour
    {
        [SerializeField] AIState<AIBaseController> _searchState;
        [SerializeField] AIState<AIBaseController> _chaseState;
        [SerializeField] AIState<AIBaseController> _attackState;


        private AIState<AIBaseController> _activeState;
        public AIAnimation AIAnimation;
        [HideInInspector]
        public AudioEntity audioEntity;

        private void Awake()
        {
            Debug.Log("how many instances are there");
            audioEntity = AudioManager.Instance.AddAsAnAudioEntity(gameObject);
            AIAnimation = new AIAnimation(GetComponent<Animator>(), transform);
            // Initialize with default statessd 
            SetSearchState();
        }

        private void Update()
        {
            _activeState?.Update();

        }

        private void SetState(ref AIState<AIBaseController> currentState, AIState<AIBaseController> newState)
        {
            if (currentState != null)
            {
                currentState.Exit();
                Destroy(currentState); // Destroy the existing state
            }

            currentState = Instantiate(newState);
            currentState.Init(this);
            currentState.Enter();
        }

        public void SetSearchState()
        {
            SetState(ref _activeState, _searchState);
        }

        public void SetChaseState()
        {
            SetState(ref _activeState, _chaseState);
        }

        public void SetAttackState()
        {
            SetState(ref _activeState, _attackState);
        }

        //public void SetSearchState()
        //{
        //    _activeState?.Exit();
        //    _activeState = Instantiate(_searchState);
        //    _activeState?.Init(this);
        //    _activeState?.Enter();
        //}

        //public void SetChaseState()
        //{
        //    _activeState?.Exit();
        //    _activeState = Instantiate(_chaseState);
        //    _activeState?.Init(this);
        //    _activeState?.Enter();
        //}

        //public void SetAttackState()
        //{
        //    _activeState?.Exit();
        //    _activeState = Instantiate(_attackState);
        //    _activeState?.Init(this);
        //    _activeState?.Enter();
        //}
    }
}
