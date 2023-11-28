using System;

namespace Utils.StateMachine
{
    public interface ICooldownObserver
    {
        void OnCooldownStarted(Type abilityType, float cooldownTime);
    }

}