using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace CompositeStateRunner
{

    [CreateAssetMenu(menuName = "AI/Attack/SpawnBullets")]
    public class SpawnBullets : AIState<AIBaseController>
    {
        public GameObject bulletPrefab;
        public int numberOfBullets = 1;
        public float radius = 20f;
        public float bulletSpeed = 5f;
        private AIAnimation _anim;

        public AnimationClip shootAnimation;
        public AnimationClip reloadAnimation;


        private List<GameObject> bulletPool = new List<GameObject>();


        public override void Enter()
        {
            if (_anim == null) _anim = _aiController.AIAnimation;

            InitializeBulletPool();
            SpawnBulletsInCircle();

            _anim.ChangeAnimationState(shootAnimation.name);
 

        }


        public override void Update()
        {
            if (_anim.getCurrentAnimationName(shootAnimation.name))
            {
                

                if(_anim.isAnimationFinished())
                    _anim.ChangeAnimationState(reloadAnimation.name);
            }

            if (_anim.getCurrentAnimationName(reloadAnimation.name) && _anim.isAnimationFinished())
            {
                SpawnBulletsInCircle();
                _anim.ChangeAnimationState(shootAnimation.name);
            }
        }



        void InitializeBulletPool()
        {
            for (int i = 0; i < numberOfBullets; i++)
            {
                GameObject bullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.Euler(0,0,-90f));
                bullet.SetActive(false);
                bulletPool.Add(bullet);
            }
        }

        void SpawnBulletsInCircle()
        {
            // Deactivate existing bullets
            DeactivateBullets();

            for (int i = 0; i < numberOfBullets; i++)
            {
                float angle = i * (360f / numberOfBullets);
                float radians = angle * Mathf.Deg2Rad;

                // Calculate position in polar coordinates
                float x = radius * Mathf.Cos(radians);
                float y = radius * Mathf.Sin(radians);

                Vector3 spawnPosition = new Vector3(x, y, 0f) + _aiController.transform.position;

                // Get a bullet from the pool and set its position
                GameObject bullet = GetPooledBullet();
                bullet.transform.position = spawnPosition;
                bullet.SetActive(true);

                // Apply force to move the bullet away from the origin
                Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
                rb.velocity = new Vector2(x, y).normalized * bulletSpeed;
            }
        }

        GameObject GetPooledBullet()
        {
            foreach (GameObject bullet in bulletPool)
            {
                if (!bullet.activeInHierarchy)
                    return bullet;
            }

            // If all bullets are in use, expand the pool
            GameObject newBullet = Instantiate(bulletPrefab, Vector3.zero, Quaternion.identity);
            newBullet.SetActive(false);
            bulletPool.Add(newBullet);

            return newBullet;
        }

        void DeactivateBullets()
        {
            foreach (GameObject bullet in bulletPool)
            {
                bullet.SetActive(false);
            }
        }

        public override void Exit()
        {
            
        }
    }

}
