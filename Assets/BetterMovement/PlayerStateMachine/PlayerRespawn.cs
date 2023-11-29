using UnityEngine;
using StateMachine;

namespace Utils.StateMachine { 

    public class PlayerRespawn : MonoBehaviour, IModeObserver
    {
        public PlayerController playerController;
        public CheckpointManager checkpointManager;

        private CharacterMode characterMode;


        private void Start()
        {

            PlayerController.ModeChanged += ModeChanged;
        }



        private void OnTriggerEnter2D(Collider2D collision)
        {
            if (collision.CompareTag("Enemy"))
            {
                if (characterMode == CharacterMode.Spirit)
                {
                
                    Debug.Log("I have collided");
                    playerController.ExitSpiritMode();


                    //playerController.SetState(typeof(OnHurtState), damageAmount);
                } else
                {
                    transform.position = checkpointManager.GetActiveCheckpointPosition();
                }
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