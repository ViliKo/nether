using UnityEngine;


namespace CompositeStateRunner
{
    public abstract class AIState<T> : ScriptableObject where T : MonoBehaviour
    {
        protected T _aiController;

        public virtual void Init(T aiController) => _aiController = aiController;

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();
    }
}

