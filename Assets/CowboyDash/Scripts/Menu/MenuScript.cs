using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace Com.KhuongDuy.MachoDash
{
	/// <summary>
	/// Script for scene 'Menu'
	/// </summary>
	/// 
	public class MenuScript : MonoBehaviour
	{
		public RectTransform contentScrollView;

		public GameObject
			gunSection,
			emptyBtnGun_2,
			playBtnGun_2,
			emptyBtnGun_3,
			playBtnGun_3,
			setting,
			market,
			coinsSection,
			livesSection;

		public Text
			totalCoin,
			marketTotalCoins,
			buyBtnTextGun_2,
			buyBtnTextGun_3;

		public Image
			soundBtn,
			musicBtn,
			coinsTab,
			livesTab;

		public Sprite
			soundAndMusicOn,
			soundAndMusicOff,
			tabSelected,
			tabUnselected;

		public AudioSource 
			menuAS,
			buttonInAS,
			buttonOutAS,
			buttonPlayAS;

		private Vector2 originalContentScrollViewPos;

		// Constructor
		private MenuScript ()
		{
		}

		// Behaviour messages
		void Start ()
		{
			SetUpSetting ();
			SetUpGunSection ();
			UpdateTotalCoinText ();

			originalContentScrollViewPos = contentScrollView.anchoredPosition;

			if (PlayerPrefs.GetInt ("Begin") == 0) {

				PlayerPrefs.SetFloat (Constants.COIN, 0);
				PlayerPrefs.SetInt ("Begin", 1);
			}
		}

		private void SetUpSetting ()
		{
			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 0) {
				soundBtn.sprite = soundAndMusicOff;
			} else {
				soundBtn.sprite = soundAndMusicOn;
			}

			// Music
			if (PlayerPrefs.GetInt (Constants.STATE_MUSIC, 1) == 0) {
				musicBtn.sprite = soundAndMusicOff;
			} else {
				musicBtn.sprite = soundAndMusicOn;

				// Sound
				menuAS.Play ();
			}


		}

		private void SetUpGunSection ()
		{
			// Gun 2
			if (PlayerPrefs.GetInt (Constants.STATE_GUN_2, 0) == 1) {
				emptyBtnGun_2.SetActive (false);
				playBtnGun_2.SetActive (true);

				buyBtnTextGun_2.text = "Available";
			}

			// Gun 3
			if (PlayerPrefs.GetInt (Constants.STATE_GUN_3, 0) == 1) {
				emptyBtnGun_3.SetActive (false);
				playBtnGun_3.SetActive (true);

				buyBtnTextGun_3.text = "Available";
			}
		}

		private void UpdateTotalCoinText ()
		{
			totalCoin.text = PlayerPrefs.GetFloat (Constants.COIN, 0.0f) + "";
			marketTotalCoins.text = PlayerPrefs.GetFloat (Constants.COIN, 0.0f) + "";
		}

		// PLay button of home menu
		public void PlayBtn_Onclick ()
		{
			contentScrollView.anchoredPosition = originalContentScrollViewPos;
			gunSection.SetActive (true);

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonPlayAS.Play ();
			}
		}

		// Back button of gun section
		public void BackBtn_Onclick ()
		{
			gunSection.SetActive (false);

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonOutAS.Play ();
			}
		}

		// Back button of market section
		public void BackBtn1_Onclick ()
		{
			market.SetActive (false);

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonOutAS.Play ();
			}
		}

		// Play game with guns
		public void PlayWithGunBtn_Onclick (int gunType)
		{
			PlayerPrefs.SetInt (Constants.GUNTYPE, gunType);
			SceneManager.LoadScene ("Play");

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		// Purchase button of gun 1
		public void BuyBtnGun1_Onclick ()
		{
			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		// Purchase button of gun 2
		public void BuyBtnGun2_Onclick (float price)
		{
			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			if (totalCoin >= price) {
				if (PlayerPrefs.GetInt (Constants.STATE_GUN_2, 0) == 0) {
					emptyBtnGun_2.SetActive (false);
					playBtnGun_2.SetActive (true);

					buyBtnTextGun_2.text = "Available";
					PlayerPrefs.SetInt (Constants.STATE_GUN_2, 1);

					totalCoin -= price;
					PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
					UpdateTotalCoinText ();
				}
			}

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		// Purchase button of gun 3
		public void BuyBtnGun3_Onclick (float price)
		{
			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			if (totalCoin >= price) {
				if (PlayerPrefs.GetInt (Constants.STATE_GUN_3, 0) == 0) {
					emptyBtnGun_3.SetActive (false);
					playBtnGun_3.SetActive (true);

					buyBtnTextGun_3.text = "Available";
					PlayerPrefs.SetInt (Constants.STATE_GUN_3, 1);

					totalCoin -= price;
					PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
					UpdateTotalCoinText ();
				}
			}

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		public void SettingBtn_Onclick ()
		{
			setting.SetActive (true);

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		// Back button of setting section
		public void BackBtn2_Onclick ()
		{
			setting.SetActive (false);

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonOutAS.Play ();
			}
		}

		// On - Off sound or music
		public void OnOffBtn_Onclick (int number)
		{
			if (number == 1) {
				if (soundBtn.sprite == soundAndMusicOn) {
					soundBtn.sprite = soundAndMusicOff;
					PlayerPrefs.SetInt (Constants.STATE_SOUND, 0);
				} else {
					soundBtn.sprite = soundAndMusicOn;
					PlayerPrefs.SetInt (Constants.STATE_SOUND, 1);
				}

				// Sound
				if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
					buttonInAS.Play ();
				}
			} else if (number == 2) {
				if (musicBtn.sprite == soundAndMusicOn) {
					musicBtn.sprite = soundAndMusicOff;
					PlayerPrefs.SetInt (Constants.STATE_MUSIC, 0);

					// Sound
					menuAS.Stop ();
				} else {
					musicBtn.sprite = soundAndMusicOn;
					PlayerPrefs.SetInt (Constants.STATE_MUSIC, 1);

					// Sound
					menuAS.Play ();
				}

				// Sound
				if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
					buttonInAS.Play ();
				}
			}
		}

		public void ShowFacebookLink ()
		{
			Application.OpenURL ("https://www.facebook.com/Adios-United-States-1086049941493480/?ref=bookmarks");
		}

		public void ShowMoreGame()
		{
			AdsControl.Instance.showAds ();
		}

		public void OpenMarket()
		{
			market.SetActive (true);
			CoinsSection_OnClick ();
			UpdateTotalCoinText ();
			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		public void CoinsSection_OnClick()
		{
			coinsSection.SetActive (true);
			livesSection.SetActive (false);
			coinsTab.sprite = tabSelected;
			livesTab.sprite = tabUnselected;

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		public void ThousandCoinsButton_OnClick() {

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			totalCoin += 1000;
			PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
			UpdateTotalCoinText ();
		}

		public void FiveThousandCoinsButton_OnClick() {

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			totalCoin += 5000;
			PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
			UpdateTotalCoinText ();
		}

		public void TenThousandCoinsButton_OnClick() {

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			totalCoin += 10000;
			PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
			UpdateTotalCoinText ();
		}

		public void LivesSection_OnClick()
		{
			livesSection.SetActive (true);
			coinsSection.SetActive (false);
			livesTab.sprite = tabSelected;
			coinsTab.sprite = tabUnselected;

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}
		}

		public void TenLifePacksButton_OnClick() {

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}

			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			totalCoin -= 1000;
			PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
			UpdateTotalCoinText ();
		}

		public void TwentyLifePacksButton_OnClick() {

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}

			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			totalCoin -= 5000;
			PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
			UpdateTotalCoinText ();
		}

		public void FiftyLifePacksButton_OnClick() {

			// Sound
			if (PlayerPrefs.GetInt (Constants.STATE_SOUND, 1) == 1) {
				buttonInAS.Play ();
			}

			float totalCoin = PlayerPrefs.GetFloat (Constants.COIN, 0.0f);
			totalCoin -= 10000;
			PlayerPrefs.SetFloat (Constants.COIN, totalCoin);
			UpdateTotalCoinText ();
		}

		public void PurchaseFailed() {
			Debug.Log ("Purchase failed");
		}
	}


}
