using UnityEngine;
using System.Collections;

public class iAdManager : MonoBehaviour {

	#if UNITY_IOS
	private UnityEngine.iOS.ADBannerView banner = null;
	
	public iAdManager(){
		UnityEngine.iOS.ADBannerView.onBannerWasClicked += onAdClicked;
		UnityEngine.iOS.ADBannerView.onBannerWasLoaded += onAdLoaded;

	}
	#endif
	
	public void ShowBanner(){
		#if UNITY_IOS
		if(UnityEngine.iOS.ADBannerView.IsAvailable (UnityEngine.iOS.ADBannerView.Type.Banner)){
			Debug.Log(this.ToString () + ": App running in iOS, preparing banner.");
			banner = new UnityEngine.iOS.ADBannerView(UnityEngine.iOS.ADBannerView.Type.Banner, UnityEngine.iOS.ADBannerView.Layout.BottomCenter);
			banner.visible=true;
		} else {
			Debug.Log(this.ToString () + "." + UnityEngine.iOS.ADBannerView.Type.Banner.ToString () + ": is not available.");
		}
		#else
		Debug.Log(this.ToString () + ": App not running in iOS, banner will not show at all.");
		#endif
		//ADBannerView.onBannerWasLoaded += onAdLoaded;
	}
	
	private void onAdClicked(){
		Debug.Log(this.ToString () + ": Banner was clicked!");
	}
	
	private void onAdLoaded(){
		Debug.Log(this.ToString () + ": Banner is loaded!");
		#if UNITY_IOS
	//	banner.visible = true;
		if(banner.visible && !banner.loaded) banner.visible = false;
		#endif
	}
	public void onCloseBanner()
	{
		banner.visible = false;
		Debug.Log(this.ToString () + ": Banner is closed!");
	}
}
