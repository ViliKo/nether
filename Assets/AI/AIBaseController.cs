using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{
    public class AIBaseController : MonoBehaviour
    {
        [SerializeField] private List<AIState<AIBaseController>> _searchStates;
        [SerializeField] private List<AIState<AIBaseController>> _chaseStates;
        [SerializeField] private List<AIState<AIBaseController>> _attackStates;

        private AIState<AIBaseController> _activeSearchState;
        private AIState<AIBaseController> _activeChaseState;
        private AIState<AIBaseController> _activeAttackState;

        private void Start()
        {
            // Initialize with default states
            SetSearchState(_searchStates[0]);
            SetChaseState(_chaseStates[0]);
            SetAttackState(_attackStates[0]);
        }

        private void Update()
        {
            _activeSearchState?.Update();
            _activeChaseState?.Update();
            _activeAttackState?.Update();
        }

        public void SetSearchState(AIState<AIBaseController> state)
        {
            _activeSearchState?.Exit();
            _activeSearchState = state;
            _activeSearchState?.Init(this);
            _activeSearchState?.Enter();
        }

        public void SetChaseState(AIState<AIBaseController> state)
        {
            _activeChaseState?.Exit();
            _activeChaseState = state;
            _activeChaseState?.Init(this);
            _activeChaseState?.Enter();
        }

        public void SetAttackState(AIState<AIBaseController> state)
        {
            _activeAttackState?.Exit();
            _activeAttackState = state;
            _activeAttackState?.Init(this);
            _activeAttackState?.Enter();
        }
    }
}
