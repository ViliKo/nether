using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    public bool evtClimbOver = false;
    public bool evtFallingOver = false;


    private void LedgeClimbOver()
    {
        Debug.Log("imhere");
        evtClimbOver = true;
    }


    private void FallingOver()
    {
        Debug.Log("Im here");
        evtFallingOver = true;
    }


}
