using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "States/Player/Fall")]
    public class FallState : PlayerStateWithMovement
    {
        #region Components

        private PlatformerController2D _col;
        private PersistentPlayerData _data;
        private CapsuleCollider2D _cc;
        private SpriteRenderer _sr;

        #endregion


        [Header("Fall State settings")]
        public float xInputTreshold = .15f;
        public bool assistedOverCorner = false;
        public float rayHeight = .1f;
        public float jumpBufferTime = .4f;
        public float moveMaxSpeed = 8f; //Target speed we want the player to reach. asd
        public float moveAcceleration = 8f; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
        public float moveDecceleration = 0.5f; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
        public float selectedInputTreshold = .15f;

        private float _xInput;
        private bool _jump;
        private float _dash;
        private float _jumpBufferTimer;
        private bool _pressedJump;
        private bool _enterSpiritState;



        public override void Init(PlayerController parent, CharacterMode characterMode)
        {
            #region Get Components
            base.Init(parent, characterMode);
            if (_col == null) _col = parent.GetComponentInChildren<PlatformerController2D>();
            if (_cc == null) _cc = parent.GetComponentInChildren<CapsuleCollider2D>();
            if (_sr == null) _sr = parent.GetComponentInChildren<SpriteRenderer>();
            if (_data == null) _data = parent.PersistentPlayerData;

            #endregion

            Reset();
            _rb.gravityScale = 5;

            _anim.ChangeAnimationState("player-air-peak");

 
        }
  
        public override void CaptureInput()
        {
            _xInput = Input.GetAxis("Horizontal");
            _dash = Input.GetAxisRaw("Dash");
            _jump = Input.GetButtonDown("Jump");

            if (Input.GetAxisRaw("Select") > selectedInputTreshold)
            {
                if (Input.GetKey(KeyCode.Joystick1Button3))
                    _enterSpiritState = true;
            }
        }


        public override void Update()
        {
            if (_rb.velocity.y < -3f)
                _anim.ChangeAnimationState("player-air-falling");

            if (_jump)
            {
                JumpBufferTimer();
                _pressedJump = true;
            }
                

            _anim.AdjustSpriteRotation(_xInput);
        }

        public override void FixedUpdate()
        {
            Move(_xInput * moveMaxSpeed, moveMaxSpeed, moveAcceleration, moveDecceleration);
            AssistOverCorner();
        }

        public override void ChangeState()
        {


            if (_data.jumpsLeft >= 1 && _jump)
                _runner.SetState(typeof(JumpState));
            else if (_pressedJump && (_jumpBufferTimer < jumpBufferTime && _col.VerticalRaycasts(_cc, rayHeight)))
            {
                Debug.Log("I should have doulble jump");
                _data.jumpsLeft = _data.maxJumps;
                _runner.SetState(typeof(JumpState));
            }
            else if(_col.VerticalRaycasts(_cc, rayHeight))
            {
                _data.jumpsLeft = _data.maxJumps;
                _runner.SetState(typeof(LandState));
            }

            if (_dash > 0)
                _runner.ActivateAbility(typeof(DashState), _data.dashCooldown);

            if (_col.HorizontalRaycastsOriginUp(-_sr.transform.localScale.x, _cc, rayHeight) && 
                _col.HorizontalRaycastsOriginUpLower(-_sr.transform.localScale.x, _cc, rayHeight))
            {
                _data.jumpsLeft = _data.maxJumps;
                _runner.SetState(typeof(WallSlideState));
            }


            if (_enterSpiritState)
            {
                _runner.ActivateAbility(typeof(SpiritModeEnterState), 10f);
            }

        }

        public override void Exit() => Reset();


        private void JumpBufferTimer()
        {
            _jumpBufferTimer += Time.deltaTime; // aloita ajan bufferointi
        }

        private void AssistOverCorner()
        {
            // TODO: assis over corner function
        }

        private void Reset()
        {
            _xInput = 0;
            _jump = false;
            _dash = 0;
            _jumpBufferTimer = 0;
            _pressedJump = false;
            _enterSpiritState = false;

            _rb.gravityScale = _data.baseGravityScale;
        }
    }
}


