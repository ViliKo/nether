using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Screenplay
{
    [CreateAssetMenu(menuName = "Screenplay/Act/WaitAct")]
    public class WaitAct : Act<Screenplay>
    {
        [SerializeField]
        private float timeDuration = 5f;
        private float timer;

        public override void Enter()
        {
            timer = timeDuration;
            Debug.Log("Im waiting");

        }

        public override void Update()
        {
            timer -= Time.deltaTime;
            Debug.Log(timer);

            if (timer < 0)
            {
                _screenplay.ExecuteNextAction();
            }
                
        }

        public override void Exit()
        {

        }


    }
}

