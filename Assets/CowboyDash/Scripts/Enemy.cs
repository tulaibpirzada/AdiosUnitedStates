using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    /// <summary>
    /// Manage Enemies
    /// </summary>
    /// 
    public class Enemy : Move
    {
        private Animator anim;

        private int dieHash;

        private bool die;

        [HideInInspector]
        public GameObject
            enemyHpBar,
            obstacleHpBar,
            hpChump;

        [HideInInspector]
        public bool
            isEnemy,
            isObstacle,
            weakEnemy,
            strongEnemy,
            weakObstacle,
            strongObstacle;

        // Behaviour messages
        void Awake()
        {
            if (this.tag != "Bomb")
            {
                anim = GetComponent<Animator>();
            }
            else
            {
                anim = GetComponentInChildren<Animator>();
            }
        }

        // Behaviour messages
        void Start()
        {
            dieHash = Animator.StringToHash("Die");
        }

        // Behaviour messages
        void OnEnable()
        {
            die = false;

            if (isEnemy)
            {
                if (strongEnemy)
                {
                    enemyHpBar.SetActive(false);
                }
            }

            if (isObstacle)
            {
                obstacleHpBar.SetActive(false);
                if (strongObstacle)
                {
                    hpChump.SetActive(true);
                }
            }

            if (this.tag == "Bomb")
            {
                transform.localPosition = new Vector3(6.5f, 0.0f, 0.0f);

                // Sound
                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1 && !PlayerController.Instance.IsDie)
                {
                    SoundManager.Instance.dynamite.clip = SoundManager.Instance.dynamiteSounds[0];
                    SoundManager.Instance.dynamite.Play();
                }
            }
            else
            {
                if (this.tag == "Bird")
                {
                    transform.localPosition = new Vector3(6.5f, Random.Range(0.25f, 2.0f), 0.0f);
                }
                else if (this.tag == "Star")
                {
                    transform.localPosition = new Vector3(6.5f, Random.Range(0.35f, 1.7f), 0.0f);
                }
                else if (this.tag == "Trap")
                {
                    transform.localPosition = new Vector3(6.5f, -1.65f, 0.0f);
                }
                else
                {
                    transform.localPosition = new Vector3(6.5f, -1.38f, 0.0f);
                }
            }
        }

        // Behaviour messages
        protected override void Update()
        {
            base.Update();

            if (die)
            {
                if (isEnemy && strongEnemy)
                    enemyHpBar.SetActive(false);

                if (isObstacle)
                    obstacleHpBar.SetActive(false);
            }
        }

        // Behaviour messages
        void OnTriggerEnter2D(Collider2D other)
        {
            if (other.tag == "Bullet")
            {
                // if this is a enemy
                CheckEnemy(ref other);

                // if this is a obstacle
                CheckObstacle(ref other);
            }

            if (other.tag == "Player")
            {
                die = true;
                anim.SetTrigger(dieHash);

                if (isObstacle)
                {
                    // Sound
                    if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1 && !PlayerController.Instance.IsDie)
                    {
                        SoundManager.Instance.obstacleExplosion.Play();
                    }
                }

                if (!PlayerController.Instance.IsDie)
                {
                    if (isEnemy)
                    {
                        if (this.tag != "Bomb")
                        {
                            if (this.name == "Skeleton")
                            {
                                // Sound
                                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                                {
                                    SoundManager.Instance.hitSkull.Play();
                                }
                            }
                            else if (this.name == "Zombie")
                            {
                                SoundManager.Instance.PlayZombieSound();
                            }
                            else if (this.name == "Bird")
                            {
                                SoundManager.Instance.PlayVultureSound();
                            }
                            else if (this.name == "Monster")
                            {
                                // Sound
                                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                                {
                                    SoundManager.Instance.hitPinata.Play();
                                }
                            }
                            else if (this.name == "Star")
                            {
                                // Sound
                                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                                {
                                    SoundManager.Instance.starExplosion.Play();
                                }
                            }

                            // Add coin
                            if (PlayerController.Instance.IsMacho)
                            {
                                GameController.Instance.UpdateCoin(2.0f);
                            }
                            else
                            {
                                GameController.Instance.UpdateCoin(1.0f);
                            }

                            // Sound
                            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                            {
                                SoundManager.Instance.coin.Play();
                            }

                            // Add enemy defeated
                            GameController.Instance.UpdateEnemyDefeated(1);
                        }
                        else
                        {
                            // Sound
                            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1 && !PlayerController.Instance.IsDie)
                            {
                                SoundManager.Instance.dynamite.clip = SoundManager.Instance.dynamiteSounds[1];
                                SoundManager.Instance.dynamite.Play();
                            }
                        }
                    }
                }
            }
        }

        private void CheckEnemy(ref Collider2D other)
        {
            if (isEnemy)
            {
                if (weakEnemy)
                {
                    if (this.tag == "Bomb")
                    {
                        transform.localPosition = other.gameObject.transform.position;
                    }

                    anim.SetTrigger(dieHash);
                    other.gameObject.SetActive(false);

                    if (this.tag != "Bomb")
                    {
                        if (this.name == "Skeleton")
                        {
                            // Sound
                            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                            {
                                SoundManager.Instance.hitSkull.Play();
                            }
                        }
                        else if (this.name == "Zombie")
                        {
                            SoundManager.Instance.PlayZombieSound();
                        }
                        else if (this.name == "Bird")
                        {
                            SoundManager.Instance.PlayVultureSound();
                        }
                        else if (this.name == "Monster")
                        {
                            // Sound
                            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                            {
                                SoundManager.Instance.hitPinata.Play();
                            }
                        }

                        // Add coin
                        if (PlayerController.Instance.IsMacho)
                        {
                            GameController.Instance.UpdateCoin(2.0f);
                        }
                        else
                        {
                            GameController.Instance.UpdateCoin(1.0f);
                        }

                        // Sound
                        if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                        {
                            SoundManager.Instance.coin.Play();
                        }

                        // Add enemy defeated
                        GameController.Instance.UpdateEnemyDefeated(1);
                    }
                    else
                    {
                        // Sound
                        if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                        {
                            SoundManager.Instance.dynamite.clip = SoundManager.Instance.dynamiteSounds[1];
                            SoundManager.Instance.dynamite.loop = false;
                            SoundManager.Instance.dynamite.Play();
                        }
                    }
                }
                else
                {
                    if (strongEnemy)
                    {
                        if (this.name == "Zombie")
                        {
                            SoundManager.Instance.PlayZombieSound();
                        }
                        else if (this.name == "Bird")
                        {
                            SoundManager.Instance.PlayVultureSound();
                        }

                        if (!enemyHpBar.activeInHierarchy)
                        {
                            enemyHpBar.SetActive(true);
                        }
                        else
                        {
                            die = true;
                            anim.SetTrigger(dieHash);

                            if (this.name == "Star")
                            {
                                // Sound
                                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                                {
                                    SoundManager.Instance.starExplosion.Play();
                                }
                            }

                            // Add coin
                            if (PlayerController.Instance.IsMacho)
                            {
                                GameController.Instance.UpdateCoin(4.0f);
                            }
                            else
                            {
                                GameController.Instance.UpdateCoin(2.0f);
                            }

                            // Sound
                            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                            {
                                SoundManager.Instance.coin.Play();
                            }

                            // Add enemy defeated
                            GameController.Instance.UpdateEnemyDefeated(1);
                        }
                        other.gameObject.SetActive(false);
                    }
                }
            }
        }

        private void CheckObstacle(ref Collider2D other)
        {
            if (isObstacle)
            {
                if (weakObstacle)
                {
                    if (!obstacleHpBar.activeInHierarchy)
                    {
                        obstacleHpBar.SetActive(true);
                    }
                    else
                    {
                        die = true;
                        anim.SetTrigger(dieHash);

                        // Sound
                        if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                        {
                            SoundManager.Instance.obstacleExplosion.Play();
                        }
                    }
                    other.gameObject.SetActive(false);
                }
                else
                {
                    if (strongObstacle)
                    {
                        if (!obstacleHpBar.activeInHierarchy)
                        {
                            obstacleHpBar.SetActive(true);
                        }
                        else
                        {
                            if (hpChump.activeInHierarchy)
                            {
                                hpChump.SetActive(false);
                            }
                            else
                            {
                                die = true;
                                anim.SetTrigger(dieHash);

                                // Sound
                                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                                {
                                    SoundManager.Instance.obstacleExplosion.Play();
                                }
                            }
                        }
                        other.gameObject.SetActive(false);
                    }
                }
            }
        }
    }
}
