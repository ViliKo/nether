using UnityEngine;
using UnityEngine.UI;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "States/Player/Walk")]
    public class WalkState : PlayerStateWithMovement
    {
        #region Components

        private PlatformerController2D _col;
        private PersistentPlayerData _data;
        private CapsuleCollider2D _cc;
        private SpriteRenderer _sr;
        private Text _transitionReason;
        #endregion


        [Header("Movement State settings")]
        public float xInputTreshold = .15f;
        public float dashInputTreshold = .15f;
        public float rayHeight = .1f;
        public float coyoteTime = .2f;
        public AnimationClip walkAnimation;
        public AnimationClip slideAnimation;
        public float runMaxSpeed = 8f; //Target speed we want the player to reach.
        public float runAcceleration = 8f; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
        public float runDecceleration = 0.5f; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
        public float selectedInputTreshold = .15f;

        // nama kaikki muuttujat resetoidaan
        private float _xInput;
        private bool _jump;
        private float _dash;
        private float _coyoteTimer;
        private bool _spiritState;



        public override void Init(PlayerController parent, CharacterMode characterMode)
        {
            #region Get Components
            base.Init(parent, characterMode);
            if (_col == null) _col = parent.GetComponentInChildren<PlatformerController2D>();
            if (_cc == null) _cc = parent.GetComponentInChildren<CapsuleCollider2D>();
            if (_sr == null) _sr = parent.GetComponentInChildren<SpriteRenderer>();
            if (_data == null) _data = parent.PersistentPlayerData;
            if (_transitionReason == null) _transitionReason = parent.StateTransition;

            #endregion

            Reset();
        }

        public override void CaptureInput()
        {

            if (Mathf.Abs(Input.GetAxis("Horizontal")) >= xInputTreshold)
                _xInput = Mathf.Sign(Input.GetAxis("Horizontal"));
            else
                _xInput = 0;
            _dash = Input.GetAxis("Dash");
            _jump = Input.GetButtonDown("Jump");


            if (Input.GetAxisRaw("Select") > selectedInputTreshold) { 
                if (Input.GetKey(KeyCode.Joystick1Button3))
                    _spiritState = true;
            }
        }

        public override void Update() {
            _anim.AdjustSpriteRotation(_xInput);
            CoyoteTimer();
        }

        public override void FixedUpdate() {
            Move(_xInput * runMaxSpeed, runMaxSpeed, runAcceleration, runDecceleration, walkAnimation.name, slideAnimation.name);
        }




        public override void ChangeState()
        {
       
            if (Mathf.Abs(_rb.velocity.x) <= 0.3 && Mathf.Abs(_xInput) < 1)
            {
                _transitionReason.text = "Kavely -> horisonttaalinen nopeus oli vahemman kuin 0.02 -> Lepo";
                _runner.SetState(typeof(IdleState));
            }
                

            if (_coyoteTimer < coyoteTime && _jump)
            {
                _transitionReason.text = "Kavely -> coyote ajastin oli pienempi kuin maaritetty aika ja hyppya on painettu -> Hyppy";
                _runner.SetState(typeof(JumpState));
            }
                
            
                

            if (!_col.VerticalRaycasts(_cc, rayHeight) && !_jump)
            {
                _transitionReason.text = "Kavely -> maahan osoittava raycast ei osunut ja ei ole painanut hyppya -> Putoaminen";
                _runner.SetState(typeof(FallState));
            }


            if (_dash > dashInputTreshold)
            {
                _transitionReason.text = "Kavely -> dash nappia painettu -> Dash";
                _runner.ActivateAbility(typeof(DashState), _data.dashCooldown);
                
            }

            if (_spiritState)
            {
                _runner.ActivateAbility(typeof(SpiritModeEnterState), 10f);
            }
        }

        public override void Exit() => Reset();

        private void CoyoteTimer()
        {
            if (!_col.VerticalRaycasts(_cc, rayHeight))
                _coyoteTimer += Time.deltaTime;
        } // function


        private void Reset()
        {
            _xInput = 0;
            _jump = false;
            _dash = 0;
            _coyoteTimer = 0;
            _spiritState = false;
        }
    }
}