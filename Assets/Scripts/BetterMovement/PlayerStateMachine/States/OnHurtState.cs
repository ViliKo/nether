using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "States/Player/OnHurt")]
    public class OnHurtState : State<PlayerController>
    {
        private float hurtDuration = 2.0f;
        private float hurtTimer;
        private int damageTaken;
        private Rigidbody2D _rb;
        //private PlayerAnimation _anim;

        public override void Init(PlayerController parent, CharacterMode characterMode)
        {
            base.Init(parent, characterMode);
            if (_rb == null) _rb = parent.GetComponent<Rigidbody2D>();
            //if (_anim == null) _anim = parent.PlayerAnimation;

            _rb.velocity = Vector2.zero;
            _rb.gravityScale = 0;
        }

        public override void SetParameters(params object[] parameters)
        {
            if (parameters.Length > 0 && parameters[0] is int)
            {
                damageTaken = (int)parameters[0];
            }



            hurtTimer = hurtDuration;
            Debug.Log($"Player took {damageTaken} damage!");
        }



        public override void Update()
        {
            hurtTimer -= Time.deltaTime;


        }

        public override void Exit()
        {
 
        }

        public override void CaptureInput()
        {

        }

        public override void FixedUpdate()
        {

        }

        public override void ChangeState()
        {
            if (hurtTimer <= 0)
            {
                _runner.SetState(typeof(IdleState));
            }
        }


    }
}