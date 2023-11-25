using UnityEngine;
using UnityEngine.UI;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "States/Player/Idle")]
    public class IdleState : State<PlayerController>
    {

        #region Components

        private Rigidbody2D _rb;
        private PlatformerController2D _col;
        private PersistentPlayerData _data;
        private CapsuleCollider2D _cc;
        private SpriteRenderer _sr;
        private PlayerAnimation _anim;
        private Text _transitionReason;

        #endregion

        [Header("Idle State settings")]
        public float xInputTreshold = .15f;
        public float rayHeight = .1f;
        public AnimationClip idleAnimation;

        // resettaa kaikki nama muuttujat
        private bool _blockinObstacle;
        private float _xInput;
        private bool _jump;
        


        public override void Init(PlayerController parent, CharacterMode characterMode)
        {
            #region Get Components
            base.Init(parent, characterMode);
            if (_col == null) _col = parent.GetComponentInChildren<PlatformerController2D>();
            if (_cc == null) _cc = parent.GetComponentInChildren<CapsuleCollider2D>();
            if (_rb == null) _rb = parent.GetComponentInChildren<Rigidbody2D>();
            if (_sr == null) _sr = parent.GetComponentInChildren<SpriteRenderer>();
            if (_anim == null) _anim = parent.PlayerAnimation;
            if (_data == null) _data = parent.PersistentPlayerData;
            if (_transitionReason == null) _transitionReason = parent.StateTransition;

            #endregion

            Reset();

            _rb.velocity = Vector2.zero;
            _anim.ChangeAnimationState(idleAnimation.name);
        }

        public override void CaptureInput()
        {
            _xInput = Input.GetAxis("Horizontal");
            _jump = Input.GetButtonDown("Jump");
        }



        public override void FixedUpdate() {}

        public override void Update()
        {
            if (Mathf.Abs(_xInput) <= xInputTreshold) return;
            bool touchingWall = CheckForWall();
            if (CheckInputDirectionIfCollided(touchingWall))
                _blockinObstacle = false;
            else if (touchingWall)
                _blockinObstacle = true;
                
        }

        private bool CheckInputDirectionIfCollided(bool touchingWall)
        {
            if (!touchingWall) return false;

            float direction = -_sr.transform.localScale.x; // pitaa kaantaa sprite direction koska alkuperainen spriten suunta on vasemmalle
            if (direction == 1) // jos suunta on oikealle
                return  _xInput + direction < direction;  // katso etta annettu sy�te on eri suuntaan kun suunta

            if (direction == -1) // jos suunta on vasemmalle
                return _xInput + direction > direction; // katso etta annettu sy�te on eri suuntaan kun suunta

            return false;
        }

        public override void ChangeState()
        {

            if (Mathf.Abs(_xInput) > xInputTreshold && _blockinObstacle == false) {
                _transitionReason.text = "Idle -> Inputtia on enemman kuin raja on ja ei ole estetta -> Walk";
                _runner.SetState(typeof(WalkState));
            }

            if (_jump && _col.VerticalRaycasts(_cc, rayHeight))
            {
                _transitionReason.text = "Idle -> Painettu hyppya ja raycast osuu maahan -> Jump";
                _runner.SetState(typeof(JumpState));
            }
                
            if (!_col.VerticalRaycasts(_cc, rayHeight))
            {
                _transitionReason.text = "Idle -> Raycast ei osu maahan -> Putoaminen";
                _runner.SetState(typeof(FallState));
            }
                
        }

        public override void Exit() => Reset();
        

        private void Reset()
        {
            _blockinObstacle = false;
            _xInput = 0;
            _jump = false;
        }

        private bool CheckForWall() => _col.HorizontalRaycastsOriginBottomUp(-_sr.transform.localScale.x, _cc, rayHeight);


        

        

        
    }


}


