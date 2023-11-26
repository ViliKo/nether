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

        private AIState<AIBaseController> _activeSearchState;
        private AIState<AIBaseController> _activeChaseState;
        private AIState<AIBaseController> _activeAttackState;

        private void Start()
        {
            // Initialize with default states
            SetSearchState();
        }

        private void Update()
        {
            _activeSearchState?.Update();
            _activeChaseState?.Update();
            _activeAttackState?.Update();
        }

        public void SetSearchState()
        {
            _activeSearchState?.Exit();
            _activeSearchState = _searchState;
            _activeSearchState?.Init(this);
            _activeSearchState?.Enter();
        }

        public void SetChaseState()
        {
            _activeChaseState?.Exit();
            _activeChaseState = _chaseState;
            _activeChaseState?.Init(this);
            _activeChaseState?.Enter();
        }

        public void SetAttackState()
        {
            _activeAttackState?.Exit();
            _activeAttackState = _attackState;
            _activeAttackState?.Init(this);
            _activeAttackState?.Enter();
        }
    }
}
