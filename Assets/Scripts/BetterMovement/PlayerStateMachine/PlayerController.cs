using UnityEngine;
using UnityEngine.UI;
using Utils.StateMachine;

namespace StateMachine
{
    public class PlayerController : StateRunner<PlayerController>
    {

        public PlayerAnimation PlayerAnimation;
        public AudioSource PlayerSound;
        public PersistentPlayerData PersistentPlayerData;
        public Text StateTransition;
        [HideInInspector]
        public AudioManager audioPlayer;
        public AudioEntity audioEntity;



        protected override void Awake()
        {
            PlayerAnimation = new PlayerAnimation(GetComponent<Animator>(), transform);

            audioEntity = AudioManager.Instance.AddAsAnAudioEntity(gameObject);
            base.Awake();
        }


    }
}