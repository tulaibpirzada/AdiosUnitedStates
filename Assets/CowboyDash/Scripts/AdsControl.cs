using UnityEngine;
using System.Collections.Generic;
using System.Collections;
using System;
using UnityEngine.SocialPlatforms;
using GoogleMobileAds.Api;
namespace Com.KhuongDuy.MachoDash
{

	public class AdsControl : MonoBehaviour
	{
		
		
		protected AdsControl ()
		{
		}

		private static AdsControl _instance;
		InterstitialAd interstitial;
		public string AdmobID_Android, AdmobID_IOS;

		public static AdsControl Instance { get { return _instance; } }

		void Awake ()
		{
			
			if (FindObjectsOfType (typeof(AdsControl)).Length > 1) {
				Destroy (gameObject);
				return;
			}
			
			_instance = this;
			MakeNewInterstial ();

			
			DontDestroyOnLoad (gameObject); 


		}

		void Start ()
		{
			
		}


		public void HandleInterstialAdClosed (object sender, EventArgs args)
		{

			if (interstitial != null)
				interstitial.Destroy ();
			MakeNewInterstial ();
			
		}

		void MakeNewInterstial ()
		{
			
	#if UNITY_ANDROID
			interstitial = new InterstitialAd (AdmobID_Android);
	#endif
	#if UNITY_IPHONE
			interstitial = new InterstitialAd (AdmobID_IOS);
	#endif
			Debug.Log ("AdMob ID: " + AdmobID_Android);
			interstitial.OnAdClosed += HandleInterstialAdClosed;
			interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
			AdRequest request = new AdRequest.Builder ()
				.AddTestDevice(AdRequest.TestDeviceSimulator)
				.AddTestDevice("357513062042313")
				.Build ();
			interstitial.LoadAd (request);
			Debug.Log ("Ad Loaded");
		}


		public void showAds ()
		{
			int noOfGameOvers = PlayerPrefs.GetInt(Constants.STATE_ADS, 0);
			noOfGameOvers += 1;

			if (noOfGameOvers == 4) {
				noOfGameOvers = 0;
				Debug.Log ("Show Ad");
				if (interstitial.IsLoaded ()) {
					interstitial.Show ();
					Debug.Log ("Ad Shown");
				} else {
					Debug.Log ("Ad Not Loaded");
				}
			}
//			Debug.Log ("No of game overs: " + noOfGameOvers);
			PlayerPrefs.SetInt(Constants.STATE_ADS, noOfGameOvers);

		
		}

		public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
		{
			print("Interstitial Failed to load: " + args.Message);
			// Handle the ad failed to load event.
		}

		public bool GetRewardAvailable ()
		{
			bool avaiable = false;
	//
			return avaiable;
		}



		public void HideBannerAds ()
		{
		}

		public void ShowBannerAds ()
		{
		}
	}
}

