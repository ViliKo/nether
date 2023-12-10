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
        private Type prevState;
        private CooldownManager _cooldownManager;


        // tama kannattaisi siirtaa muualle loogisesti
        private CharacterMode _currentMode = CharacterMode.Normal;
        private bool _teleportToPrevLocation;
        protected Vector2 _startPosition;
        protected float _startDirection;
        private float _spiritModeTimer = 0f;
        private SpriteRenderer _sr;
        private Material _baseMaterial;

        public static event Action<CharacterMode> ModeChanged;

        protected virtual void Awake()
        {

            SetState(_states[0].GetType());
            _cooldownManager = new CooldownManager();
            CooldownManager.CooldownStarted += OnCooldownStarted;

            _teleportToPrevLocation = false;
            _sr = GetComponent<SpriteRenderer>();
            _baseMaterial = _sr.material;
        }

        private void OnCooldownStarted(Type abilityType, float cooldownTime)
        {
            //Debug.Log("Event triggered form state runner");
            // You can add more information to this event if needed
            // For example, you might want to know which ability is on cooldown
        }



        public void SetMode(CharacterMode mode)
        {
            _currentMode = mode;

            if (_teleportToPrevLocation)
            {
                _teleportToPrevLocation = false;
                transform.position = _startPosition;
                transform.localScale = new Vector2(_startDirection, transform.localScale.y);
                DestroyAllObjectsWithTag("freezeFrame");
                Debug.Log(_startDirection + " This is the start direction");
            }
  

            ModeChanged?.Invoke(mode);
        }

        public void SetState(Type newStateType, params object[] parameters)
        {
            if (!(_currentMode == CharacterMode.Spirit))
                prevState = newStateType; // ota edellinen tila talteen, etta moden jalkeen voi palata siihen

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
                    EnterSpiritState();
                    _spiritModeTimer = (float)parameters[0];
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
            if (!PlayerUI.gameIsPaused)
                _activeState.CaptureInput();
            _activeState.Update();
            _cooldownManager.UpdateCooldowns();
            _activeState.ChangeState();

            if (_currentMode == CharacterMode.Spirit)
            {
                _spiritModeTimer -= Time.deltaTime;
                if (_spiritModeTimer <= 0)
                    ExitSpiritMode();    
            }
            
        }

        public void ExitSpiritMode()
        {

            _teleportToPrevLocation = true;
            SetState(prevState);
            SetMode(CharacterMode.Normal);
            _sr.material = _baseMaterial;
        }

        public void EnterSpiritState()
        {
            SetMode(CharacterMode.Spirit);
            _startDirection = transform.localScale.x;
            _startPosition = transform.position;
        }

        private void FixedUpdate()
        {
            _activeState.FixedUpdate();
        }


        private void OnDisable()
        {
            CooldownManager.CooldownStarted -= OnCooldownStarted;
        }


        private void DestroyAllObjectsWithTag(string tag)
        {
            GameObject[] objectsToDestroy = GameObject.FindGameObjectsWithTag(tag);

            foreach (GameObject obj in objectsToDestroy)
            {
                Destroy(obj);
            }
        }
    }
}