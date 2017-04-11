using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    /// <summary>
    /// Manage sounds and music in game
    /// </summary>
    public class SoundManager : MonoBehaviour
    {
        public static SoundManager Instance = null;

        public AudioClip[] 
            jumpSounds,
            gunsSounds,
            dynamiteSounds,
            zombieSounds,
            vultureSounds,
            gamePlaySounds,
            gameOverSounds;

        public AudioSource
            buttonIn,
            buttonOut,
            highScore,
            levelStart,
            jump,
            takeDamage,
            gun,
            bearTrap,
            dynamite,
            getMACHO,
            coin,
            hitSkull,
            hitZombie,
            hitVulture,
            hitPinata,
            starExplosion,
            obstacleExplosion,
            gamePlay,
            gameOver,
            gameResult;

        // Constructor
        private SoundManager() { }

        // Behaviour messages
        void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
            }
            else if (Instance != null)
            {
                Destroy(this.gameObject);
            }
        }

        // Behaiour messages
        void Start()
        {
            if (PlayerPrefs.GetInt(Constants.STATE_MUSIC, 1) == 1)
            {
                gamePlay.Play();
            }
        }

        public void PlayJumpSound()
        {
            jump.clip = jumpSounds[Mathf.RoundToInt(Random.Range(0.0f, 3.0f))];

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                jump.Play();
            }
        }

        public void PlayGunSound()
        {
            gun.clip = gunsSounds[PlayerController.Instance.gunType - 1];

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                gun.Play();
            }
        }

        public void PlayZombieSound()
        {
            hitZombie.clip = zombieSounds[Mathf.RoundToInt(Random.Range(0.0f, 2.0f))];

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                hitZombie.Play();
            }
        }

        public void PlayVultureSound()
        {
            hitVulture.clip = vultureSounds[Mathf.RoundToInt(Random.Range(0.0f, 1.0f))];

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                hitVulture.Play();
            }
        }

        public void PlayGameOverSound()
        {
            gameOver.clip = gameOverSounds[Mathf.RoundToInt(Random.Range(0.0f, 5.0f))];

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                gameOver.Play();
            }
        }
    }
}
