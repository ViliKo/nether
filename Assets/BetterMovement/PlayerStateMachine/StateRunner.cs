using StateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Utils.StateMachine
{


    public abstract class StateRunner<T> : MonoBehaviour where T : MonoBehaviour
    {
        [SerializeField]
        private List<State<T>> _states;
        private State<T> _activeState;
        private CooldownManager _cooldownManager;
        private CharacterMode _currentMode = CharacterMode.Normal;
        
        protected Vector2 _startPosition;


        private float _spiritModeTimer = 0f;
        private float _spiritModeDuration = 10f; 
        private SpriteRenderer _sr;
        private Material _baseMaterial;

        public event Action<CharacterMode> ModeChanged;

        protected virtual void Awake()
        {
            _cooldownManager = new CooldownManager();
            _sr = GetComponent<SpriteRenderer>();
            _baseMaterial = _sr.material;

            CooldownManager.CooldownStarted += OnCooldownStarted;

            SetState(_states[0].GetType());
        }

        private void OnCooldownStarted(Type abilityType, float cooldownTime)
        {
            Debug.Log("Event triggered form state runner");
            // You can add more information to this event if needed
            // For example, you might want to know which ability is on cooldown
        }

        public void SetMode(CharacterMode mode, Vector2 _startPosition)
        {
            _currentMode = mode;
            transform.position = _startPosition;

            ModeChanged?.Invoke(mode);
        }

        public void SetState(Type newStateType, params object[] parameters)
        {
            if (_activeState != null)
                _activeState.Exit();
            

            _activeState = _states.First(s => s.GetType() == newStateType);
            _activeState.Init(GetComponent<T>(), _currentMode);

            // Laita parametrit jos tila tukee niita
            _activeState.SetParameters(parameters);
        }

        public void ActivateAbility(Type abilityType, float cooldownTime, params object[] parameters)
        {
            if (!_cooldownManager.IsAbilityOnCooldown(abilityType))
            {
                if (abilityType == typeof(SpiritModeEnterState))
                {
                    SetMode(CharacterMode.Spirit, transform.position);
                    _spiritModeTimer = _spiritModeDuration;
                    _startPosition = transform.position;
                }

                SetState(abilityType, parameters);
                _cooldownManager.StartCooldown(abilityType, cooldownTime);
            }
            else
            {
                //Debug.Log($"{abilityType.Name} is on cooldown.");
            }
        }



        private void Update()
        {
            _activeState.CaptureInput();
            _activeState.Update();
            _cooldownManager.UpdateCooldowns();

            if (_currentMode == CharacterMode.Spirit)
            {
                _spiritModeTimer -= Time.deltaTime;
                if (_spiritModeTimer <= 0)
                {
                    SetMode(CharacterMode.Normal, _startPosition);
                    Destroy(GameObject.FindGameObjectWithTag("freezeFrame"));
                    _sr.material = _baseMaterial;

                }
            }


            _activeState.ChangeState();


        }

        private void FixedUpdate()
        {
            _activeState.FixedUpdate();
        }


        private void OnDisable()
        {
            CooldownManager.CooldownStarted -= OnCooldownStarted;
        }

    }
}