using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace StateMachine
{
    [CreateAssetMenu(menuName = "States/PersitentPlayerData")]
    public class PersistentPlayerData : ScriptableObject
    {

        public float baseGravityScale = 2f;
        public int maxJumps = 2;
        
        // ehka naita voidaa muokata pelin aikana
        public float dashCooldown = .5f;
        public bool hasSpiritAbility = false;
        public float spiritAbilityCooldown = 20f;
        public float spiritAbilityLength = 8f;


        [HideInInspector]
        public int dir = -1;
        [HideInInspector]
        public int jumpsLeft = 2;
    }

}

