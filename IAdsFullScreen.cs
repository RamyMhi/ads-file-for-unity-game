using UnityEngine;
using System.Collections;

public class IAdsFullScreen : MonoBehaviour {
	private UnityEngine.iOS.ADInterstitialAd fullscreenAd = null;

	// Use this for initialization
	void Start () {

	
	}
	public IAdsFullScreen()
#if UNITY_IOS
	{
		fullscreenAd = new UnityEngine.iOS.ADInterstitialAd();
		UnityEngine.iOS.ADInterstitialAd.onInterstitialWasLoaded += OnFullscreenLoaded;
	}
#endif

	 public void OnFullscreenLoaded()
	{
		fullscreenAd.Show();
	}

	// Update is called once per frame
	void Update () {
	
	}
}
