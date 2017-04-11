using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Com.KhuongDuy.MachoDash
{
    /// <summary>
    /// Manage game UI
    /// </summary>
    /// 
    public class UIManager : MonoBehaviour
    {
        [SerializeField]
        private GameObject[] m_hearts;

        [SerializeField]
        private Sprite m_emptyLetter;

        [SerializeField]
        private Sprite[] m_letters;

        [SerializeField]
        private Image[] m_hudLetters;

        [SerializeField]
        private Text m_coinText;

        [SerializeField]
        private Text 
            m_scoreText,
            m_bestDistance,
            m_highScore,
            m_totalCoin,
            m_currentDistance,
            m_reachedCoin,
            m_enemyDefeated;

        public static UIManager Instance = null;

        public Animator machoLettersAnim;

        public GameObject 
            gameOverMenu,
            pauseMenu,
            tutorialMenu;

        public Image 
            tutorialImage,
            newHighScoreImage;

        public Sprite[] tutorialSprites;

        // Constructor
        private UIManager() { }

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

        public void SetHUDLetter(string letter)
        {
            if (letter == "M")
            {
                m_hudLetters[0].sprite = m_letters[0];
            }
            else if (letter == "A")
            {
                m_hudLetters[1].sprite = m_letters[1];
            }
            else if (letter == "C")
            {
                m_hudLetters[2].sprite = m_letters[2];
            }
            else if (letter == "H")
            {
                m_hudLetters[3].sprite = m_letters[3];
            }
            else if (letter == "O")
            {
                m_hudLetters[4].sprite = m_letters[4];
            }
        }

        public void ResetHUDLetter()
        {
            for (int i = m_hudLetters.Length - 1; i >= 0; i--)
            {
                m_hudLetters[i].sprite = m_emptyLetter;
            }
        }

        public void UpdateDistance(float distance)
        {
            m_scoreText.text = distance + "";
        }

        public void UpdateBestDistance(float distance)
        {
            m_bestDistance.text = distance + "";
        }

        public void UpdateCoin(float coin)
        {
            m_coinText.text = coin + "";
        }

        public void UpdateHeart()
        {
            for (int i = 0; i < m_hearts.Length; i++)
            {
                if (m_hearts[i].activeInHierarchy)
                {
                    m_hearts[i].GetComponent<Animator>().SetTrigger("Break");
                    break;
                }
            }
        }

        public void UpdateGameOver(float highScore, float totalCoin, float currentDistance, float currentCoin, float enemyDefeated)
        {
            m_highScore.text = highScore + "";
            m_totalCoin.text = totalCoin + "";
            m_currentDistance.text = "Distance: " + currentDistance;
            m_reachedCoin.text = "Coin collected: " + currentCoin;
            m_enemyDefeated.text = "Enemy defeated: " + enemyDefeated;
        }

        /* Button Events */
        public void PauseBtn_Onclick()
        {
            Time.timeScale = 0.0f;
            pauseMenu.SetActive(true);

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonIn.Play();
            }
        }

        public void ResumeBtn_Onclick()
        {
            pauseMenu.SetActive(false);
            Time.timeScale = 1.0f;

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonIn.Play();
            }
        }

        public void RestartBtn_Onclick()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Play");

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonIn.Play();
            }
        }

        public void TutorialBtn_Onclick()
        {
            tutorialMenu.SetActive(true);

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonIn.Play();
            }
        }

        public void ExitBtn_Onclick()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Menu");

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonOut.Play();
            }
        }

        public void CheckTutorial()
        {
            if (tutorialImage.sprite == tutorialSprites[0])
            {
                tutorialImage.sprite = tutorialSprites[1];
            }
            else if (tutorialImage.sprite == tutorialSprites[1])
            {
                tutorialImage.sprite = tutorialSprites[2];
            }
            else if (tutorialImage.sprite == tutorialSprites[2])
            {
                tutorialImage.sprite = tutorialSprites[3];
            }
            else if (tutorialImage.sprite == tutorialSprites[3])
            {
                tutorialMenu.SetActive(false);
                tutorialImage.sprite = tutorialSprites[0];
            }

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonIn.Play();
            }
        }

        public void PlayAgainBtn_Onclick()
        {
            SceneManager.LoadScene("Play");

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonIn.Play();
            }
        }

        // Back to menu
        public void BackBtn_Onclick()
        {
            Time.timeScale = 1.0f;
            SceneManager.LoadScene("Menu");

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.buttonIn.Play();
            }
        }
    }
}
