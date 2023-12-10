using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{
    [CreateAssetMenu(menuName = "AI/Attack/SpawnWind")]
    public class SpawnWind : AIState<AIBaseController>
    {
        private AudioEntity _audio;
        public GameObject windPrefab; // Reference to your wind prefab
        public float windDuration = 2f; // Duration of the wind effect
        private BoxCollider2D _bc;
        private SpriteRenderer _sr;
        private AIAnimation _anim;
        public AnimationClip effectEndAnimation;
        [SerializeField] private AudioClip spawnWindAudio;

        Quaternion flippedRotation = Quaternion.Euler(0f, 0f, 180f);

        Quaternion normalRotation;


        public override void Enter()
        {
            if (_bc == null) _bc = _aiController.GetComponent<BoxCollider2D>();
            if (_sr == null) _sr = _aiController.GetComponent<SpriteRenderer>();
            if (_anim == null) _anim = _aiController.AIAnimation;
            if (_audio == null) _audio = _aiController.audioEntity;

            normalRotation = _aiController.transform.rotation;

            _audio.PlayState(spawnWindAudio,1f);

            if (_sr.transform.localScale.x == -1)
            {
                normalRotation = flippedRotation;
            }
           

            _anim.ChangeAnimationState(effectEndAnimation.name);

            GameObject windInstance = Instantiate(windPrefab, 
                new Vector3(
                     _bc.transform.position.x + (_sr.transform.localScale.x*windPrefab.GetComponent<BoxCollider2D>().size.x/2),
                    _bc.transform.position.y + _bc.offset.y,
                    0), 
                normalRotation);

            Debug.Log("imhere");
            // Destroy the windPrefab after windDuration seconds
            Destroy(windInstance, windDuration);


        }

        public override void Exit()
        {

        }

        public override void Update()
        {
            if (_anim.getCurrentAnimationName(effectEndAnimation.name) && _anim.isAnimationFinished())
                _aiController.SetSearchState();
        }


    }

}

