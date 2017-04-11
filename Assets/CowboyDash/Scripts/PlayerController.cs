using UnityEngine;
using System.Collections;

namespace Com.KhuongDuy.MachoDash
{
    /// <summary>
    /// Manage player character
    /// </summary>
    /// 
    public class PlayerController : MonoBehaviour
    {
        [SerializeField]
        private Transform[] m_firePoints;

        [SerializeField]
        private Animator m_anim;

        [SerializeField]
        private Rigidbody2D m_rg2D;

        [SerializeField]
        private SpriteRenderer[] m_gunTypes;

        [SerializeField]
        private SpriteRenderer m_renderer;

        [SerializeField]
        private float
            m_jumpForce,
            m_heightLimitForCharacter = 1.0f;

        [SerializeField]
        private float[] m_fireRateGuns;     // Time between the bullets

        private GameObject[] m_bulletsArray;

        private float m_nextFire = 0.0f;

        private int
            m_jumpHash,
            m_jumpAirHash,
            m_isMachoHash,
            m_hurtHash,
            m_dieHash,
            m_jumpCount,
            m_HP;

        private bool
            m_jumping,
            m_jumpButtonIsPressed,
            m_fireButtonIsPressed,
            m_gunBurst,
            m_isInvulnerable;

        public static PlayerController Instance = null;

        public GameObject machoEffect;

        public ParticleSystem starParticle;

        public int gunType = 1;

        public bool IsDie { get; set; }

        public bool IsMacho { get; set; }

        // Constructor
        private PlayerController() { }

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
            gunType = PlayerPrefs.GetInt(Constants.GUNTYPE, 1);
            m_gunTypes[gunType - 1].enabled = true;

            GetBulletArray();
            HideAllBullets();

            m_jumpHash = Animator.StringToHash("Jump");
            m_jumpAirHash = Animator.StringToHash("JumpAir");
            m_isMachoHash = Animator.StringToHash("IsMacho");
            m_hurtHash = Animator.StringToHash("Hurt");
            m_dieHash = Animator.StringToHash("Die");

            m_jumpCount = 0;
            m_HP = 3;

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
            {
                SoundManager.Instance.levelStart.Play();
            }
        }

        private void GetBulletArray()
        {
            m_bulletsArray = new GameObject[m_firePoints[gunType - 1].childCount];

            for (int i = m_bulletsArray.Length - 1; i >= 0; i--)
            {
                m_bulletsArray[i] = m_firePoints[gunType - 1].GetChild(i).gameObject;
            }
        }

        private void HideAllBullets()
        {
            for (int i = m_firePoints.Length - 1; i >= 0; i--)
            {
                for (int j = m_firePoints[i].childCount - 1; j >= 0; j--)
                {
                    m_firePoints[i].GetChild(j).gameObject.SetActive(false);
                }
            }
        }

        // Behaviour messages
        void Update()
        {
            if (!IsDie)
            {
                HandleJump();
                HandleFire();
            }
        }

        private void HandleJump()
        {
            if (m_jumpButtonIsPressed)
            {
                if (m_jumpCount < 2)
                {
                    if (m_jumpCount == 0)
                    {
                        m_jumping = true;
                        m_anim.SetBool(m_jumpHash, true);
                    }
                    else
                    {
                        m_anim.SetBool(m_jumpAirHash, true);
                    }
                    m_rg2D.velocity = new Vector2(m_rg2D.velocity.x, m_jumpForce);
                }
            }

            // 'm_heightLimitForCharacter' is largest height character can jump up if holding the jump button
            // this value depends on the speed of character jump
            if (transform.position.y >= m_heightLimitForCharacter)
            {
                m_jumpButtonIsPressed = false;
            }
        }

        // Part jump is pressed
        public void JumpDown()
        {
            m_jumpButtonIsPressed = true;

            if (!m_jumping || m_jumpCount == 1)
            {
                SoundManager.Instance.PlayJumpSound();
            }
        }

        // Part jump is released
        public void JumpUp()
        {
            m_jumpButtonIsPressed = false;

            if (m_jumping)
            {
                m_jumpCount++;
            }
        }

        private void HandleFire()
        {
            if (m_fireButtonIsPressed)
            {
                if (Time.time >= m_nextFire)
                {
                    for (int i = m_bulletsArray.Length - 1; i >= 0; i--)
                    {
                        if (!m_bulletsArray[i].activeInHierarchy)
                        {
                            m_bulletsArray[i].SetActive(true);

                            m_nextFire = Time.time + m_fireRateGuns[gunType - 1];

                            // Sound
                            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                            {
                                SoundManager.Instance.PlayGunSound();
                            }
                            break;
                        }
                    }
                }
            }
        }

        // Part fire is pressed
        public void FireDown()
        {
            m_fireButtonIsPressed = true;
        }

        // Part fire is released
        public void FireUp()
        {
            m_fireButtonIsPressed = false;
        }

        // Behaviour messages
        void OnCollisionEnter2D(Collision2D other)
        {
            if (other.gameObject.tag == "Ground")
            {
                if (m_jumping)
                {
                    m_jumping = false;
                    m_anim.SetBool(m_jumpHash, false);
                    m_anim.SetBool(m_jumpAirHash, false);

                    m_jumpCount = 0;
                }
            }
        }

        // Behaviour messages
        void OnTriggerEnter2D(Collider2D other)
        {
            HandleLetters(other);
            Hurt(other);
        }

        private void HandleLetters(Collider2D other)
        {
            if (other.tag == "M" || other.tag == "A" || other.tag == "C" || other.tag == "H" || other.tag == "O")
            {
                // Sound
                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1 && !IsDie)
                {
                    SoundManager.Instance.getMACHO.Play();
                }

                // Disable letter and make effect
                GameController.Instance.letterEffect.transform.position = other.gameObject.transform.position;
                GameController.Instance.letterEffect.SetActive(true);
                other.gameObject.SetActive(false);

                // Set the last letter
                GameController.Instance.SetLetterReached(other.tag);

                // Set HUD
                UIManager.Instance.SetHUDLetter(other.tag);

                // Transform into MACHO
                if (other.tag == "O")
                {
                    IsMacho = true;
                    m_anim.SetBool(m_isMachoHash, true);

                    UIManager.Instance.machoLettersAnim.enabled = true;

                    GameController.Instance.machoEffect.transform.position = transform.position;
                    GameController.Instance.machoEffect.SetActive(true);

                    StartCoroutine(MachoDuration());

                    // Sound
                    if (PlayerPrefs.GetInt(Constants.STATE_MUSIC, 1) == 1)
                    {
                        SoundManager.Instance.gamePlay.clip = SoundManager.Instance.gamePlaySounds[1];
                        SoundManager.Instance.gamePlay.Play();
                    }
                }
            }
        }

        private IEnumerator MachoDuration()
        {
            yield return new WaitForSeconds(10.0f);
            IsMacho = false;
            m_anim.SetBool(m_isMachoHash, false);
            machoEffect.SetActive(false);
            starParticle.Stop();

            // Reset letters and HUD of letters
            UIManager.Instance.machoLettersAnim.enabled = false;
            UIManager.Instance.ResetHUDLetter();
            GameController.Instance.SetLetterReached("");

            // Sound
            if (PlayerPrefs.GetInt(Constants.STATE_MUSIC, 1) == 1)
            {
                SoundManager.Instance.gamePlay.clip = SoundManager.Instance.gamePlaySounds[0];
                SoundManager.Instance.gamePlay.Play();
            }
        }

        private void Hurt(Collider2D other)
        {
            if (!IsDie)
            {
                if (other.tag == "Enemy" || other.tag == "Bird" || other.tag == "Star" || other.tag == "Bomb" || other.tag == "Trap")
                {
                    // Sound
                    if (other.tag == "Trap")
                    {
                        if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                        {
                            SoundManager.Instance.bearTrap.Play();
                        }
                    }

                    if (!IsMacho && !m_isInvulnerable)
                    {
                        if (m_HP > 0)
                        {
                            m_HP--;
                            UIManager.Instance.UpdateHeart();

                            if (m_HP != 0)
                            {
                                m_anim.SetTrigger(m_hurtHash);
                                m_isInvulnerable = true;
                                StartCoroutine(ImmortalDuration(1.5f));
                            }
                            else
                            {
                                IsDie = true;
                                m_anim.SetTrigger(m_dieHash);

                                // Sound
                                if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                                {
                                    SoundManager.Instance.PlayGameOverSound();
                                }

                                GameController.Instance.GameOver();
                            }

                            // Sound
                            if (PlayerPrefs.GetInt(Constants.STATE_SOUND, 1) == 1)
                            {
                                SoundManager.Instance.takeDamage.Play();
                            }
                        }
                    }
                }
            }
        }

        private IEnumerator ImmortalDuration(float duration)
        {
            float time = 0.0f;
            while (true)
            {
                yield return new WaitForSeconds(0.05f);
                m_renderer.enabled = false;
                m_gunTypes[gunType - 1].enabled = false;
                yield return new WaitForSeconds(0.05f);
                m_renderer.enabled = true;
                m_gunTypes[gunType - 1].enabled = true;

                time += 0.15f;
                if (time >= duration)
                {
                    m_isInvulnerable = false;
                    break;
                }
            }
        }
    }
}
