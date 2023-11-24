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

        #endregion


        private float _xInput;
        private bool _jump;
        private bool _jumpHeld;
        private float _dash;
        public float xInputTreshold = .15f;
        public float dashInputTreshold = .15f;

        [SerializeField]
        private bool visualizer = true;

        [SerializeField]
        private float _jumpHeight = 40;
        


        [SerializeField]
        private float _rayHeight = .1f;

        public float timeUntilCheckGround = .3f;
        private float lastOnGround;

        private bool isOnGround = false;




        public AnimationClip jumpAnimation;
        public float moveMaxSpeed = 8f; //Target speed we want the player to reach.
        public float moveAcceleration = 8f; //Time (approx.) time we want it to take for the player to accelerate from 0 to the runMaxSpeed.
        public float moveDecceleration = 0.5f; //Time (approx.) we want it to take for the player to accelerate from runMaxSpeed to 0.


        private float selectedInputTreshold = .15f;
        private bool _spiritState;


        public override void Init(PlayerController parent, CharacterMode characterMode)
        {
            #region Get Components
            base.Init(parent, characterMode);
            if (_col == null) _col = parent.GetComponentInChildren<PlatformerController2D>();
            if (_cc == null) _cc = parent.GetComponentInChildren<CapsuleCollider2D>();
            if (_sr == null) _sr = parent.GetComponentInChildren<SpriteRenderer>();
            if (_data == null) _data = parent.PersistentPlayerData;

            #endregion

            _col.Reset();

            _spiritState = false;

            Jump();

            if (visualizer)
            {
                _sr.color = Color.black;
                Debug.Log("<color=black>Started a jump state </color>");

            }
        }

        public override void CaptureInput() {
            FollowInputs();
        }

        public override void Update()
        {
            TimeUntilCheckGround();
            StartToIncreaseGravity();
            
            _anim.AdjustSpriteRotation(_xInput);

            if (visualizer)
                Debug.Log("Jump is held: " + _jumpHeld);

        }


        public override void FixedUpdate() {

            _col.VerticalRaycasts(_cc, _rayHeight);
            Move(_xInput * moveMaxSpeed, moveMaxSpeed, moveAcceleration, moveDecceleration);
        }

        public override void ChangeState()
        {
            if (!isOnGround && _rb.velocity.y < 0)
                _runner.SetState(typeof(FallState));
            if (isOnGround && _rb.velocity.x < 1f)
                _runner.SetState(typeof(IdleState));
            if (isOnGround && _rb.velocity.x > 1f)
                _runner.SetState(typeof(WalkState));
            if (_data.jumpsLeft >= 1 && _jump)
                _runner.SetState(typeof(JumpState));
            if (_dash > 0)
                _runner.ActivateAbility(typeof(DashState), _data.dashCooldown);

            if (_spiritState)
            {
                _runner.ActivateAbility(typeof(SpiritModeEnterState), 10f);
            }

        }

        public override void Exit() {
            isOnGround = false;
        }


        private void TimeUntilCheckGround()
        {
            if (isOnGround) return;

            lastOnGround += Time.deltaTime;

            if (lastOnGround > timeUntilCheckGround)
            {
                if (_col.collisions.VerticalBottom)
                {
                    isOnGround = true;
                }

            }
        }
        private void FollowInputs()
        {
            if (Input.GetAxisRaw("Select") > selectedInputTreshold && Input.GetKey(KeyCode.Joystick1Button3))
            {
                _spiritState = true;
            }

            if (Mathf.Abs(Input.GetAxis("Horizontal")) > xInputTreshold)
            {
                _xInput = Input.GetAxis("Horizontal");
            }
            else
            {
                _xInput = 0;
            }

            _jump = Input.GetButtonDown("Jump");
            _jumpHeld = !Input.GetButtonUp("Jump");
            _dash = Input.GetAxisRaw("Dash");
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
            lastOnGround = 0;

            float mass = _rb.mass;
            float distance = _jumpHeight;
            float forceMagnitude = (mass * 9.8f * _rb.gravityScale * distance) / 2;

            _rb.AddForce(new Vector2(_rb.velocity.x, forceMagnitude), ForceMode2D.Impulse);
            _anim.ChangeAnimationState(jumpAnimation.name);
        }




    }
}