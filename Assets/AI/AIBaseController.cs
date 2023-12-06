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


        private void Awake()
        {
            Debug.Log("how many instances are there");
            AIAnimation = new AIAnimation(GetComponent<Animator>(), transform);
            // Initialize with default states
            SetSearchState();
        }

        private void Update()
        {
            _activeState?.Update();

        }

        public void SetSearchState()
        {
            _activeState?.Exit();
            _activeState = Instantiate(_searchState);
            _activeState?.Init(this);
            _activeState?.Enter();
        }

        public void SetChaseState()
        {
            _activeState?.Exit();
            _activeState = Instantiate(_chaseState);
            _activeState?.Init(this);
            _activeState?.Enter();
        }

        public void SetAttackState()
        {
            _activeState?.Exit();
            _activeState = Instantiate(_attackState);
            _activeState?.Init(this);
            _activeState?.Enter();
        }
    }
}
