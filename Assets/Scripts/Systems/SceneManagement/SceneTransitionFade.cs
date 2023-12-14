using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
[RequireComponent(typeof(Image))]
public class SceneTransitionFade : MonoBehaviour
{
    // Start is called before the first frame update
    public bool fadeOut;

    public bool fadeIn;

     float fadeTime;
    public float FadeOutSlower;
    public bool doFadeOut;
    float alpha;
    public CanvasGroup CanvasGroup;

    void Start()
    {
       fadeTime = 1f;
      
    }

    // Update is called once per frame
    void Update()
    {
               FadeOut();   
               FadeIn(); 
    }


    public void FadeOut(){
   
     
          if(fadeOut == true){
           

            if( CanvasGroup.alpha > 0){

                CanvasGroup.alpha -= fadeTime/2 * Time.deltaTime;

                if(CanvasGroup.alpha <= 0){
                    fadeOut =false;

                }


            }

            
        }

    }


        public void FadeIn(){


          if(fadeIn){
           
            if( CanvasGroup.alpha < 1){

             CanvasGroup.alpha += fadeTime * Time.deltaTime;
                    if(CanvasGroup.alpha == 1){
                        fadeIn =false;
                    }   
            }

        }

    }



}
