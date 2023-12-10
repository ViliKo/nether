using System;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.StateMachine
{

    public class CooldownManager
    {
        private Dictionary<Type, float> _cooldowns = new Dictionary<Type, float>();

        public static event Action<Type, float> CooldownStarted;


     


        public void StartCooldown(Type abilityType, float cooldownTime)
        {
            _cooldowns[abilityType] = Time.time + cooldownTime;

            CooldownStarted?.Invoke(abilityType, cooldownTime);
            Debug.Log("this is my ability: " + abilityType);

        }

        public bool IsAbilityOnCooldown(Type abilityType)
        {
            return _cooldowns.ContainsKey(abilityType) && Time.time < _cooldowns[abilityType];
        }

        public void UpdateCooldowns()
        {
            // Optional: Implement logic to remove expired cooldowns
            // For example, you can remove entries where Time.time > _cooldowns[abilityType]

            // This method can be expanded based on your specific needs
        }

    }
}
