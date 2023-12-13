using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Screenplay 
{
    public class Screenplay : MonoBehaviour
    {
        // Start is called before the first frame update
        [SerializeField]
        private List<Act<Screenplay>> _actList = new List<Act<Screenplay>>();

        [SerializeField]
        private List<GameObject> _actors = new List<GameObject>(); 

        private Act<Screenplay> _activeAct;

        public ActorAnimation ActorAnimation;

        private BoxCollider2D _bc;

        private int _currentActIndex = 0;

        private void Awake()
        {
            _bc = GetComponent<BoxCollider2D>();

            foreach (GameObject actor in _actors)
            {
                Animator actorAnimator = actor.GetComponent<Animator>();

                if (actorAnimator != null)
                {
                    ActorAnimation actorAnimation = actor.AddComponent<ActorAnimation>();
                    actorAnimation.Initialize(actorAnimator);
                }
                else
                {
                    Debug.LogError("Animator component not found on actor: " + actor.name);
                }
            }

            foreach (Act<Screenplay> act in _actList)
            {
                act.Init(this, _actors);
            }


            
        }

        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.tag == "Player")
                ExecuteNextAction();
        }



        private void Update()
        {
            _activeAct?.Update();
        }

        private void SetAct(ref Act<Screenplay> _activeAct, Act<Screenplay> newAct)
        {
            if (_activeAct != null)
            {
                _activeAct.Exit();
            }

            Debug.Log("Current act is: " + newAct);
            _activeAct = newAct;
            _activeAct.Init(this, _actors);
            _activeAct.Enter();
        }



        public void ExecuteNextAction()
        {
            if (_currentActIndex < _actList.Count)
            {
                Debug.Log("Im here at the execute");
                Act<Screenplay> nextAct = _actList[_currentActIndex];
                SetAct(ref _activeAct, nextAct);
                _currentActIndex++;
            }
            else
            {
                Debug.Log("Screenplay is over");
            }
        }

    }
}