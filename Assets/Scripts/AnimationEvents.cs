using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    public bool evtClimbOver = false;



    private void LedgeClimbOver()
    {
        evtClimbOver = true;
    }


}
