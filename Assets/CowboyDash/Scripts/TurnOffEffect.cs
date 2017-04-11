using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    public class TurnOffEffect : MonoBehaviour
    {
        // Constructor
        private TurnOffEffect() { }

        // Animation event
        public void TurnOffStar()
        {
            this.gameObject.SetActive(false);
        }

        // Animation event
        public void EndMachoEffect()
        {
            PlayerController.Instance.machoEffect.SetActive(true);
            PlayerController.Instance.starParticle.Play();
            this.gameObject.SetActive(false);
        }

        // Animation event
        public void OffHeart()
        {
            this.gameObject.SetActive(false);
        }

        // Animation event
        public void OffTutorial()
        {
            this.gameObject.SetActive(false);
        }

        // Animation event
        public void CheckHighScore()
        {
            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.gameResult.Play();
            }

            if (UIManager.Instance.newHighScoreImage.enabled)
            {
                // Sound
                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                {
                    SoundManager.Instance.highScore.Play();
                }
            }
        }
    }
}
