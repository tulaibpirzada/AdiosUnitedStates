using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    /// <summary>
    /// Manage game logic
    /// </summary>
    /// 
    public class GameController : BackGroundMove
    {
        [SerializeField]
        private float m_distanceFactor;

        private float            
            m_distanceMove,
            m_coin,
            m_bestDistance;

        private int m_enemyDefeated;

        private string m_letterReached;

        private bool m_gameOver;

        public static GameController Instance = null;

        public GameObject[] letters;

        public GameObject
            letterEffect,
            machoEffect;

        public GameObject[]
            enemies,
            obstacles;

        // Constructor
        private GameController() { }

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

        // Behaviour messages
        void Start()
        {
			Application.targetFrameRate = 60;
            m_coin = 0.0f;
            UIManager.Instance.UpdateCoin(m_coin);

            m_bestDistance = PlayerPrefs.GetFloat(Constants.DISTANCE, 0.0f);
            UIManager.Instance.UpdateBestDistance(m_bestDistance);

            m_enemyDefeated = 0;

            m_distanceMove = 0.0f;
            m_letterReached = "";

            // Spawn letters
            StartCoroutine(SpawnLetter());

            SpawnEnemies();
        }

        private IEnumerator SpawnLetter()
        {
            yield return new WaitForSeconds(5.0f);
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(5.0f, 7.0f));
                if (Random.value <= 0.4f)
                {
                    if (m_letterReached == "")
                    {
                        letters[0].SetActive(true);
                    }
                    else if (m_letterReached == "M")
                    {
                        letters[1].SetActive(true);
                    }
                    else if (m_letterReached == "A")
                    {
                        letters[2].SetActive(true);
                    }
                    else if (m_letterReached == "C")
                    {
                        letters[3].SetActive(true);
                    }
                    else if (m_letterReached == "H")
                    {
                        letters[4].SetActive(true);
                    }
                }
            }
        }

        private void SpawnEnemies()
        {
            // Spawn normal male zombie
            StartCoroutine(SpawnEnemy(7.5f, 8.5f, 0, 2, false));

            // Spawn normal female zombie
            StartCoroutine(SpawnEnemy(8.5f, 9.5f, 3, 5, false));

            // Spawn normal strong male zombie
            StartCoroutine(SpawnEnemy(16.5f, 20.0f, 6, 8, false));

            // Spawn normal strong female zombie
            StartCoroutine(SpawnEnemy(18.5f, 22.0f, 9, 11, false));

            // Spawn monster
            StartCoroutine(SpawnEnemy(9.5f, 12.5f, 21, 23, true));

            // Spawn normal bird
            StartCoroutine(SpawnEnemy(9.0f, 15.0f, 12, 14, false));

            // Spawn strong bird 1
            StartCoroutine(SpawnEnemy(16.0f, 21.0f, 15, 17, false));

            // Spawn strong bird 2
            StartCoroutine(SpawnEnemy(8.0f, 10.5f, 18, 20, true));

            // Spawn star
            StartCoroutine(SpawnEnemy(3.5f, 5.5f, 24, 26, false));

            // Spawn bomb
            StartCoroutine(SpawnBomb());

            // Spawn obstacles
            StartCoroutine(SpawnObstacles(3.0f, 6.0f));
        }

        private IEnumerator SpawnEnemy(float timeLimit_1, float timeLimit_2, int indexLimitOfEnemies_1, int indexLimitOfEnemies_2, bool isSpecial)
        {
            yield return new WaitForSeconds(3.0f);
            while (true)
            {
                float time1 = timeLimit_1, time2 = timeLimit_2;
                bool spawn = true;

                if (m_distanceMove >= 1000.0f)
                {
                    if (!isSpecial)
                    {
                        time1 = timeLimit_1 - 2.0f;
                        time2 = timeLimit_2 - 2.0f;
                    }
                }
                else
                {
                    if (isSpecial)
                    {
                        spawn = false;
                    }
                }

                if (spawn)
                {
                    yield return new WaitForSeconds(Random.Range(time1, time2));
                    for (int i = indexLimitOfEnemies_2; i >= indexLimitOfEnemies_1; i--)
                    {
                        if (!enemies[i].activeInHierarchy)
                        {
                            enemies[i].SetActive(true);
                            break;
                        }
                    }
                }
                else
                {
                    yield return new WaitForSeconds(3.0f);
                }
            }
        }

        private IEnumerator SpawnBomb()
        {
            yield return new WaitForSeconds(10.0f);
            while (true)
            {
                yield return new WaitForSeconds(18.0f);
                if (Random.value >= 0.5f)
                {
                    enemies[27].SetActive(true);
                }
            }
        }

        private IEnumerator SpawnObstacles(float timeLimit_1, float timeLimit_2)
        {
            yield return new WaitForSeconds(10.0f);
            while (true)
            {
                float time1 = timeLimit_1, time2 = timeLimit_2;

                if (m_distanceMove >= 1000.0f)
                {
                    time1 = timeLimit_1 - 2.0f;
                    time2 = timeLimit_2 - 2.0f;
                }

                yield return new WaitForSeconds(Random.Range(time1, time2));
                while (true)
                {
                    int random = 0;

                    if (m_distanceMove < 1000.0f)
                    {
                        random = (int)Mathf.Round(Random.Range(0.0f, 4.0f));
                    }
                    else
                    {
                        random = (int)Mathf.Round(Random.Range(0.0f, 8.0f));
                    }

                    if (!obstacles[random].activeInHierarchy)
                    {
                        obstacles[random].SetActive(true);
                        break;
                    }
                }
            }
        }

        // Behaviour messages
        void Update()
        {
            if (!m_gameOver)
            {
                UpdateDistance();
            }
        }

        private void UpdateDistance()
        {
            m_distanceMove += Time.deltaTime * m_distanceFactor;
            float round = Mathf.Round(m_distanceMove);
            UIManager.Instance.UpdateDistance(round);

            if (m_distanceMove > m_bestDistance)
            {
                UIManager.Instance.UpdateBestDistance(round);
            }
        }

        public void SetLetterReached(string letter)
        {
            m_letterReached = letter;
        }

        public void UpdateCoin(float bonusCoin)
        {
            m_coin += bonusCoin;
            UIManager.Instance.UpdateCoin(m_coin);
        }

        public void UpdateEnemyDefeated(int amount)
        {
            m_enemyDefeated += amount;
        }

        public void GameOver()
        {
            m_gameOver = true;
			AdsControl.Instance.showAds ();
            if (m_distanceMove > m_bestDistance)
            {
                PlayerPrefs.SetFloat(Constants.DISTANCE, Mathf.Round(m_distanceMove));
                UIManager.Instance.newHighScoreImage.enabled = true;
            }

            float totalCoin = PlayerPrefs.GetFloat(Constants.COIN, 0.0f);
            totalCoin += m_coin;
            PlayerPrefs.SetFloat(Constants.COIN, totalCoin);

            UIManager.Instance.UpdateGameOver(PlayerPrefs.GetFloat(Constants.DISTANCE, 0.0f), totalCoin,
                Mathf.Round(m_distanceMove), m_coin, m_enemyDefeated);

            StartCoroutine(WaitToShowGameOver());
        }

        private IEnumerator WaitToShowGameOver()
        {
            yield return new WaitForSeconds(1.0f);
            UIManager.Instance.gameOverMenu.SetActive(true);
        }
    }
}
