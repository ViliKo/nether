using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "States/Player/WallSlide")]
    public class WallSlideState : State<PlayerController>
    {
        #region Components

        private Rigidbody2D _rb;
        private PlatformerController2D _col;
        private PersistentPlayerData _data;
        private CapsuleCollider2D _cc;
        private SpriteRenderer _sr;
        private PlayerAnimation _anim;

        #endregion

        [Header("Wall Slide Settings")]
        public float slideSpeed = 2f;
        public float rayHeight = .1f;
        public AnimationClip wallSlideAnimation;
        public bool visualizer = true;
        public float selectedInputTreshold;
        public float inputTreshold = .15f;
        public float dashInputTreshold = .15f;

        [SerializeField]
        private float _rayHeight = .1f;
        private float _yInput;
        private bool _jump;
        private bool _spiritState;
        private float _dash;
        private bool _dashedFromWall;





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

            #endregion

            Reset();

            

            if (visualizer)
                _sr.color = Color.magenta;


        }

        public override void CaptureInput()
        {
            _yInput = Input.GetAxis("Vertical");
            _jump = Input.GetButtonDown("Jump");
            _dash = Input.GetAxis("Dash");

            if (Input.GetAxisRaw("Select") > selectedInputTreshold)
            {
                if (Input.GetKey(KeyCode.Joystick1Button3))
                    _spiritState = true;
            }
        }




        public override void Update() {
            _anim.ChangeAnimationState(wallSlideAnimation.name);
        }

        public override void FixedUpdate()
        {
            _rb.velocity = new Vector2(0, -slideSpeed);

        }

        private bool isClimbable()
        {
            _col.HorizontalRaycastsOriginUp(-_sr.transform.localScale.x, _cc, rayHeight);
            if (_col.hit.tag == "nonClimbable")
                return false;
            else
                return true;   
        }

        public override void ChangeState()
        {
            if (_yInput > inputTreshold && isClimbable())
                _runner.SetState(typeof(WallClimbState));
            else if (_col.VerticalRaycasts(_cc, _rayHeight)) 
                _runner.SetState(typeof(IdleState));
            else if (_jump)
                _runner.SetState(typeof(JumpState));
            else if (!_col.HorizontalRaycastsOriginUp(-_sr.transform.localScale.x, _cc, rayHeight) && !_col.HorizontalRaycastsOriginUpLower(-_sr.transform.localScale.x, _cc, rayHeight))
            {
                _data.jumpsLeft -= 1;
                _runner.SetState(typeof(FallState));
            }
            else if (_spiritState && _data.hasSpiritAbility)
            {
                _runner.ActivateAbility(typeof(SpiritModeEnterState), _data.spiritAbilityCooldown, _data.spiritAbilityLength);
            }
            else if (_dash > dashInputTreshold)
            {
                _dashedFromWall = true;
                _runner.ActivateAbility(typeof(DashState), _data.dashCooldown);
                _dashedFromWall = false;
            }
        }

        public override void Exit() {
            if (_dashedFromWall)
            {
                Debug.Log("Im here mother focker");
                _rb.transform.localScale = new Vector2(-_rb.transform.localScale.x, _rb.transform.localScale.y);
            }
                

            Reset();
        }

        private void Reset()
        {
            _spiritState = false;
            _dashedFromWall = false;
            _yInput = 0;
            _jump = false;
            _dash = 0;
        }

  
    }
}


