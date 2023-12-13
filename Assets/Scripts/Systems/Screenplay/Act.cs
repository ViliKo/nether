using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

namespace Screenplay { 

    public abstract class Act<T> : ScriptableObject where T : MonoBehaviour
    {
        protected T _screenplay;
        protected List<GameObject> _actors;

        public virtual void Init(T screenplay, List<GameObject> actors)
        {
            _screenplay = screenplay;
            _actors = actors;

        }

        public abstract void Enter();
        public abstract void Update();
        public abstract void Exit();

        protected GameObject FindActorByName(string actorName)
        {
            return _actors.FirstOrDefault(actor => actor.name == actorName);
        }
    }

}