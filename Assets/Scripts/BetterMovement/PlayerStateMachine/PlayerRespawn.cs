using UnityEngine;
using StateMachine;
using UnityEngine.SceneManagement;

namespace Utils.StateMachine { 

    public class PlayerRespawn : MonoBehaviour, IModeObserver
    {
        public PlayerController playerController;
        public CheckpointManager checkpointManager;

        private CharacterMode characterMode;
        [SerializeField] private float timeUntilRespawn = 4f;
        private float timer;
        private bool startTimer = false;

        private void Start()
        {
            

            PlayerController.ModeChanged += ModeChanged;
        }

        private void Update()
        {
            if (startTimer)
            {
                timer += Time.deltaTime;
                playerController.SetState(typeof(OnHurtState));
            }
                

            if (timer > timeUntilRespawn)
            {
                timer = 0;
                startTimer = false;

                
                

                if (characterMode == CharacterMode.Spirit)
                {
                    playerController.SetState(typeof(FallState));
                    playerController.ExitSpiritMode();
                }
                    
                
                else
                {
                    //SceneManager.LoadScene("MainMenu");
                    Debug.Log("I m here respawning");
                    EnemyPoolingManager.Instance.ResetAllEnemiesToInitialState();
                    transform.position = checkpointManager.GetActiveCheckpointPosition(); //talla tehdaan oikeasti
                }
                    

            }
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                startTimer = true;

                
            }
            
        }

        private void OnDisable()
        {
            PlayerController.ModeChanged -= ModeChanged;
        }

        public void ModeChanged(CharacterMode characterMode)
        {
            Debug.Log("triggered form Player respawn current mode is: " + characterMode);
            this.characterMode = characterMode;
        }
    }

}