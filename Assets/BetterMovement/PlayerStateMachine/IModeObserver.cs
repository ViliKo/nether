using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Utils.StateMachine
{
    public interface IModeObserver 
    {
        void ModeChanged(CharacterMode characterMode);
    }
}


