using UnityEngine.UI;
using UnityEngine;
using System.Collections;
#if UNITY_IOS || UNITY_ANDROID
using UnityEngine.Advertisements;
#endif

public class AdsScript : MonoBehaviour 
{


	[System.Serializable]
	public struct GameInfo
	{  
		[SerializeField]
		private string _gameID;
		[SerializeField]
		private string _testGameID;
		
		public string GetGameID ()
		{
			return Debug.isDebugBuild && !string.IsNullOrEmpty(_testGameID) ? _testGameID : _gameID;
		}
	}
	public GameInfo iOS;
	public GameInfo android;
	public bool disableTestMode;
	public bool showInfoLogs;
	public bool showDebugLogs;
	public bool showWarningLogs = true;
	public bool showErrorLogs = true;

	public Canvas gem_Icon;
	public Canvas energy_Icon;



	#if UNITY_IOS || UNITY_ANDROID
	protected void Awake() 
	{
		string gameID = null;
		//PlayerPrefs.SetInt ("ADsCounter" ,0);
		#if UNITY_IOS
		gameID = iOS.GetGameID();
		#elif UNITY_ANDROID
		gameID = android.GetGameID();
		#endif
		
		if (!Advertisement.isSupported) 
		{
			Debug.LogWarning("Unity Ads is not supported on the current platform.");
		}
		else if (string.IsNullOrEmpty(gameID))
		{
			Debug.LogError("A valid game ID is required to initialize Unity Ads.");
		}
		else
		{
			Advertisement.allowPrecache = true;
			
			Advertisement.debugLevel = Advertisement.DebugLevel.NONE;	
			if (showInfoLogs) Advertisement.debugLevel    |= Advertisement.DebugLevel.INFO;
			if (showDebugLogs) Advertisement.debugLevel   |= Advertisement.DebugLevel.DEBUG;
			if (showWarningLogs) Advertisement.debugLevel |= Advertisement.DebugLevel.WARNING;
			if (showErrorLogs) Advertisement.debugLevel   |= Advertisement.DebugLevel.ERROR;
			
			bool enableTestMode = Debug.isDebugBuild && !disableTestMode; 
			Debug.Log(string.Format("Initializing Unity Ads for game ID {0} with test mode {1}...",
			                        gameID, enableTestMode ? "enabled" : "disabled"));
			
			Advertisement.Initialize(gameID,enableTestMode);
		}
	}
	void Update()
	{
	//	Debug.Log("ads update.");
		string zone = null;
		/*
		if (Advertisement.isReady ()) {
						Debug.LogWarning (string.Format ("The ad placement zone ($0) is not ready. Unable to show ad.",
			                               zone == null ? "default" : zone));
			if (PlayerPrefs.GetInt ("ADsCounter")==0)
			{
								gem_Icon.enabled = true;
								energy_Icon.enabled = false;
						} 
			else 
			{
								gem_Icon.enabled = false;
								energy_Icon.enabled = true;

						}
						/// code disable here
						/// 
						/// 
						/// 

				} 
		else 
		{
						gem_Icon.enabled = false;
						energy_Icon.enabled = false;
				}
				*/

	}

	public static bool isInitialized { get { return Advertisement.isInitialized; }}
	
	public static bool isReady (string zone = null)
	{
		if (string.IsNullOrEmpty(zone)) zone = null;
		return Advertisement.isReady(zone);
	}

	public static bool ShowAd (string zone = null, bool pauseGameDuringAd = true)
	{
		if (string.IsNullOrEmpty(zone)) zone = null;
		
		if (!Advertisement.isReady(zone))
		{
			Debug.LogWarning(string.Format("The ad placement zone ($0) is not ready. Unable to show ad.",
			                               zone == null ? "default" : zone));
			if (PlayerPrefs.GetInt("energyPrefs")==0)
			{
				PlayerPrefs.SetInt("energyPrefs" , 1);	
			/// code disable here
			/// 
			/// 
			/// 
			}
			return false;
		}
		
		ShowOptions options = new ShowOptions();
		options.pause = pauseGameDuringAd;
		options.resultCallback = HandleShowResult;
		
		Advertisement.Show(zone,options);
		
		return true;
	}

	public static void HandleShowResult (ShowResult result)
	{
		switch (result)
		{
		case ShowResult.Finished:
		{
			Debug.Log("The ad was successfully shown.");

			PlayerPrefs.SetInt("energyPrefs",PlayerPrefs.GetInt("energyPrefs")+3);
			/*
			//i=+20;
			if( PlayerPrefs.GetInt("ADsCounter")==0)
			{
				PlayerPrefs.SetInt("GEMPrefs",PlayerPrefs.GetInt("GEMPrefs")+1);
				PlayerPrefs.SetInt("ADsCounter",PlayerPrefs.GetInt("ADsCounter")+1);

			}
			else if (PlayerPrefs.GetInt("ADsCounter")==9)
			{
				PlayerPrefs.SetInt("energyPrefs",PlayerPrefs.GetInt("energyPrefs")+1000);
				PlayerPrefs.SetInt("ADsCounter",0);			
			}
			else
			{
				PlayerPrefs.SetInt("energyPrefs",PlayerPrefs.GetInt("energyPrefs")+1000);
				PlayerPrefs.SetInt("ADsCounter",PlayerPrefs.GetInt("ADsCounter")+1);
			}
			Debug.Log(PlayerPrefs.GetInt("ADsCounter"));
			*/
		}
			break;
		case ShowResult.Skipped:
			Debug.Log("The ad was skipped before reaching the end.");
			break;
		case ShowResult.Failed:
			Debug.LogError("The ad failed to be shown.");

			break;
		}
	}
	
	public void showTestAds()
	{
		ShowAd ();
	}

	#else
	public enum ShowResult { none }
	const string msgPlatformNotSupported = "Unity Ads is not supported on the current platform.";
	
	public static bool isInitialized { get { return false; }}
	public static bool isReady (string zone = null) { Debug.LogWarning(msgPlatformNotSupported); return false; }
	public static bool ShowAd (string zone = null, bool pauseGameDuringAd = true) { Debug.LogWarning(msgPlatformNotSupported); return false; }
	public static void HandleShowResult (ShowResult result) { Debug.LogWarning(msgPlatformNotSupported); }
	#endif
}

