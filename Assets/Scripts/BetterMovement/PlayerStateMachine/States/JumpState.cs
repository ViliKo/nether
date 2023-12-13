using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "States/Player/Jump")]
    public class JumpState : PlayerStateWithMovement
    {
        #region Components

        private PlatformerController2D _col;
        private PersistentPlayerData _data;
        private CapsuleCollider2D _cc;
        private SpriteRenderer _sr;
        private AudioEntity _audio;

        #endregion

        [Header("Jump State settings")]
        public float xInputTreshold = .15f;
        public float dashInputTreshold = .15f;
        public float _jumpHeight = 40;
        public float _rayHeight = .1f;
        public float timeUntilCheckGround = .3f;
        public AnimationClip jumpAnimation;
        public AudioClip jumpSound;
        public float moveMaxSpeed = 8f; //Target speed we want the player to reach.
        public float moveAcceleration = 8f; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
        public float moveDecceleration = 0.5f; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.
        public float selectedInputTreshold = .15f;

        // Nama kaikki muuttujat resetoidaan
        private float _xInput;
        private bool _jump;
        private bool _jumpHeld;
        private float _dash;
        private float _lastOnGround;
        private bool _isOnGround;
        private bool _enterSpiritState; // tama asetetaan muualla epatodeksi


        public override void Init(PlayerController parent, CharacterMode characterMode)
        {
            #region Get Components
            base.Init(parent, characterMode);
            if (_col == null) _col = parent.GetComponentInChildren<PlatformerController2D>();
            if (_cc == null) _cc = parent.GetComponentInChildren<CapsuleCollider2D>();
            if (_sr == null) _sr = parent.GetComponentInChildren<SpriteRenderer>();
            if (_data == null) _data = parent.PersistentPlayerData;
            if (_audio == null) _audio = parent.audioEntity;
            #endregion

            Reset();
            Jump();
            _audio.PlayState(jumpSound, .4f, true);
        }

        public override void CaptureInput() {
            _xInput = Input.GetAxis("Horizontal");
            _jump = Input.GetButtonDown("Jump");
            _jumpHeld = !Input.GetButtonUp("Jump");
            _dash = Input.GetAxisRaw("Dash");

            if (Input.GetAxisRaw("Select") > selectedInputTreshold)
            {
                if (Input.GetKey(KeyCode.Joystick1Button3))
                    _enterSpiritState = true;
            }
        }

        public override void Update()
        {
            TimeUntilCheckGround();
            StartToIncreaseGravity();
            _anim.AdjustSpriteRotation(_xInput);
        }


        public override void FixedUpdate() {
            Move(_xInput * moveMaxSpeed, moveMaxSpeed, moveAcceleration, moveDecceleration);
        }

        public override void ChangeState()
        {
            if (!_isOnGround && _rb.velocity.y < 0)
                _runner.SetState(typeof(FallState));
            if (_isOnGround)
            {
                _data.jumpsLeft = _data.maxJumps;
                _runner.SetState(typeof(LandState));
            }
            if (_data.jumpsLeft >= 1 && _jump)
                _runner.SetState(typeof(JumpState));
            if (_dash > 0)
                _runner.ActivateAbility(typeof(DashState), _data.dashCooldown);

            if (_enterSpiritState && _data.hasSpiritAbility)
            {
                _runner.ActivateAbility(typeof(SpiritModeEnterState), _data.spiritAbilityCooldown, _data.spiritAbilityLength);
            }

        }

        public override void Exit() => Reset();


        private void TimeUntilCheckGround()
        {
            if (_isOnGround) return;

            _lastOnGround += Time.deltaTime;

            if (_lastOnGround > timeUntilCheckGround)
            {
                if (_col.VerticalRaycasts(_cc, _rayHeight))
                {
                    _isOnGround = true;
                }

            }
        }

        private void StartToIncreaseGravity()
        {
            if (_jumpHeld) return;

            _rb.gravityScale *= 3;
        }


        private void Jump()
        {
            _rb.velocity = new Vector2(_rb.velocity.x, 0);
            _rb.gravityScale = 2;
            _jumpHeld = true;
            _data.jumpsLeft -= 1;
            _lastOnGround = 0;

            float mass = _rb.mass;
            float distance = _jumpHeight;
            float forceMagnitude = (mass * 9.8f * _rb.gravityScale * distance) / 2;

            _rb.AddForce(new Vector2(0, forceMagnitude), ForceMode2D.Impulse);
            _anim.ChangeAnimationState(jumpAnimation.name);
        }

        private void Reset()
        {
            _xInput = 0;
            _jump = false;
            _jumpHeld = false;
            _dash = 0;
            _lastOnGround = 0;
            _isOnGround = false;
            _enterSpiritState = false;

        }


    }
}