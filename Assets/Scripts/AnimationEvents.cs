using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{

    public bool evtClimbOver = false;

    //Animator animator;

    //AnimationEvent evtLedgeOver;

    // Start is called before the first frame update
    //void Start()
    //{
    //    Debug.Log("wtf why is this not triggering");

    //    evtLedgeOver = new AnimationEvent();
    //    evtLedgeOver.intParameter = 12123;
    //    evtLedgeOver.time = 0.5f;
    //    evtLedgeOver.functionName = "LedgeClimbOver";

    //    animator = GetComponent<Animator>();

    //    foreach (AnimationClip clip in animator.runtimeAnimatorController.animationClips)
    //    {
    //        if (clip.name == "player-corner-climb-2")
    //        {
    //            clip.AddEvent(evtLedgeOver);

    //            Debug.Log("I added animation event");
    //        }
    //    }

    //}


    private void LedgeClimbOver()
    {
        evtClimbOver = true;
    }


}
