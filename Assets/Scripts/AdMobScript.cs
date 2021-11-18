using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using GoogleMobileAds.Api;
using UnityEngine.UI;

public class AdMobScript : MonoBehaviour
{
    public static AdMobScript instance;
    public Text _notInternetText;
    bool shouldShowText;

    //android
    readonly string appId = "ca-app-pub-7148117075170097~1645865453"; //not used yet i am using other shit
                                                                      //make one for IOS too

    //change to actual ids on launch

    //actual ad
    string Interstitial_Ad_ID = "ca-app-pub-7148117075170097/9759518568"; //for android

    readonly string ios_interstitial_ad_id = "ca-app-pub-7148117075170097/8669234490";
    readonly string android_iterstiotial_ad_id = "ca-app-pub-7148117075170097/9759518568";

    //actual ad
    //android
    string Rewarded_Ad_ID = "ca-app-pub-7148117075170097/5820273553";

    readonly string ios_rewarded_ad_id = "ca-app-pub-7148117075170097/8268945092";
    readonly string android_rewarded_ad_id = "ca-app-pub-7148117075170097/5820273553";

    private InterstitialAd _interstitial;
    private RewardedAd _rewardBaseVideo;

    public PauseMenu _pausemenu; //asigned trough pause menu start individual of all levels

    public int _typeOfAD; //0 extra life, 1 double rewards, 2 extra money, 3 play again crowd sim
    public bool adAvailable;

    public bool playTestMode; //off if launch

    //for rewarded ads

    public bool adPlaying;
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(instance);
        }
        else
        {
            Destroy(gameObject);
        }

        if (playTestMode)
        {
            //testing the game
            Rewarded_Ad_ID = "ca-app-pub-3940256099942544/5224354917";
            Interstitial_Ad_ID = "ca-app-pub-3940256099942544/1033173712";
        }
        else
        {
            //launch

        }

        adAvailable = false;
        adPlaying = false;
    }

    void Start()
    {
        if(_notInternetText != null)
        _notInternetText.text = "";

        if(_pausemenu != null)
        _pausemenu = GetComponent<PauseMenu>();


        MobileAds.Initialize(initStatus => { });

        RequestRewardBasedVideo();
        RequestInterstitial();
    }

    public void RequestInterstitial()
    {

        // Initialize an InterstitialAd.
        this._interstitial = new InterstitialAd(Interstitial_Ad_ID);

        // Called when an ad request has successfully loaded.
        this._interstitial.OnAdLoaded += HandleOnAdLoaded;
        // Called when an ad request failed to load.
        this._interstitial.OnAdFailedToLoad += HandleOnAdFailedToLoad;
        // Called when an ad is shown.
        this._interstitial.OnAdOpening += HandleOnAdOpened;
        // Called when the ad is closed.
        this._interstitial.OnAdClosed += HandleOnAdClosed;
        // Called when the ad click caused the user to leave the application.
        this._interstitial.OnAdLeavingApplication += HandleOnAdLeavingApplication;

        //create an empty ad request
        AdRequest request = new AdRequest.Builder().Build();
        this._interstitial.LoadAd(request);

    }

    public void RequestRewardBasedVideo()
    {
        if(adAvailable == false)
        {
            this._rewardBaseVideo = new RewardedAd(Rewarded_Ad_ID);

            // Called when an ad request has successfully loaded.
            this._rewardBaseVideo.OnAdLoaded += HandleRewardedAdLoaded;
            // Called when an ad request failed to load.
            this._rewardBaseVideo.OnAdFailedToLoad += HandleRewardedAdFailedToLoad;
            // Called when an ad is shown.
            this._rewardBaseVideo.OnAdOpening += HandleRewardedAdOpening;
            // Called when an ad request failed to show.
            this._rewardBaseVideo.OnAdFailedToShow += HandleRewardedAdFailedToShow;
            // Called when the user should be rewarded for interacting with the ad.
            this._rewardBaseVideo.OnUserEarnedReward += HandleUserEarnedReward;
            // Called when the ad is closed.
            this._rewardBaseVideo.OnAdClosed += HandleRewardedAdClosed;

            // Create an empty ad request.
            AdRequest request = new AdRequest.Builder().Build();
            // Load the rewarded ad with the request.
            this._rewardBaseVideo.LoadAd(request);
        }
        else
        {
            Debug.Log("already has ad");
        }
    }

    public void ShowVideoRewardAd()
    {
        if (this._rewardBaseVideo.IsLoaded())
        {
            adPlaying = true;
            this._rewardBaseVideo.Show();
        }
        else //not loaded debugger
        {
            StartCoroutine(WaitAndMakeTextDisappear(2));

            if (_typeOfAD == 0)
            {
                //extra life
                _pausemenu.RewardNo();
            }
            else
           if (_typeOfAD == 1)
            {
                //double rewards
                _pausemenu.ClosePopUP();
            }
            else
           if (_typeOfAD == 2)
            {
                //extra coins
            }
            if(_typeOfAD == 3)
            {
                CrowdSpawne.instance.AdFailed();
            }
            else
            {
                Debug.Log("idk anything else sir");
            }
        }
    }

    public void ShowInterstititalAd()
    {
        if (this._interstitial.IsLoaded())
        {
            this._interstitial.Show();
        }
        else
        {
            //nimic
            return;
        }
    }

    #region InterstitialHandler
    public void HandleOnAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("ad loaded");
    }

    public void HandleOnAdFailedToLoad(object sender, AdFailedToLoadEventArgs args)
    {
        Debug.Log("ad failed to load");
    }

    public void HandleOnAdOpened(object sender, EventArgs args)
    {
        //pause game progress and audio
        GameObject.FindObjectOfType<AudioManager>().PauseAllMusic();
    }

    public void HandleOnAdClosed(object sender, EventArgs args)
    {
        //resume game progress and audio
        GameObject.FindObjectOfType<AudioManager>().PlayAllMusic();

        //load next reward based video ad
        this.RequestInterstitial();
    }

    public void HandleOnAdLeavingApplication(object sender, EventArgs args)
    {
        MonoBehaviour.print("HandleAdLeavingApplication event received");
       //wont do shit about it im not retarded
    }
    #endregion

    #region RewardedHandler
    public void HandleRewardedAdLoaded(object sender, EventArgs args)
    {
        Debug.Log("reward based video ad loaded");
        adAvailable = true;
    }

    public void HandleRewardedAdFailedToLoad(object sender, AdErrorEventArgs args)
    {
        Debug.Log("reward based video ad failed to load");
        adPlaying = false;

        adAvailable = false;

        StartCoroutine(WaitAndMakeTextDisappear(2));
    }

    public void HandleRewardedAdOpening(object sender, EventArgs args)
    {
        //pause game progress and audio
        adPlaying = true;
        GameObject.FindObjectOfType<AudioManager>().PauseAllMusic();
    }

    public void HandleRewardedAdFailedToShow(object sender, AdErrorEventArgs args)
    {
        MonoBehaviour.print(
            "HandleRewardedAdFailedToShow event received with message: "
                             + args.Message);

        StartCoroutine(WaitAndMakeTextDisappear(2));

        if (_typeOfAD == 0)
        {
            //extra life
            _pausemenu.RewardNo();
        }
        else
           if (_typeOfAD == 1)
        {
            //double rewards
            _pausemenu.ClosePopUP();
        }
        else
           if (_typeOfAD == 2)
        {
            //extra coins
        }
        if (_typeOfAD == 3)
        {
            CrowdSpawne.instance.AdFailed();
        }
        else
        {
            Debug.Log("idk anything else sir");
        }

        adPlaying = false;

        adAvailable = false;

    }

    public void HandleRewardedAdClosed(object sender, EventArgs args)
    {
        //Invoked when the rewarded video ad is closed due to the user 
        //tapping on the close 
        //icon or using the back button.

        adAvailable = false;

        //resume game progress; NO REWARD FOR YOU
        GameObject.FindObjectOfType<AudioManager>().PlayAllMusic();


        if (_typeOfAD == 0)
        {
            //extra life
            _pausemenu.RewardNo();
        }
        else
            if (_typeOfAD == 1)
        {
            //double rewards
            _pausemenu.ClosePopUP();
        }
        else
            if (_typeOfAD == 2)
        {
            //extra coins
        }
        if (_typeOfAD == 3)
        {
            CrowdSpawne.instance.AdFailed();
        }
        else
        {
            Debug.Log("idk anything else sir");
        }

        adPlaying = false;

        //load next reward based video ad
        this.RequestRewardBasedVideo();
    }

    public void HandleUserEarnedReward(object sender, Reward args)
    {

        /*string type = args.Type;
        double amount = args.Amount;
        MonoBehaviour.print(
            "HandleRewardedAdRewarded event received for "
                        + amount.ToString() + " " + type);
        */

        //rewarding user
        Debug.Log("rewarding...");
        if(_typeOfAD == 0)
        {
            //extra life
            _pausemenu.RewardYes();
        }
        else
            if(_typeOfAD == 1)
        {
            //double rewards
            _pausemenu.DoubledCoinsSucces();
        }
        else
            if(_typeOfAD == 2)
        {
            //extra coins
            GameMaster.instance.money += 50;
        }
        if (_typeOfAD == 3)
        {
            CrowdSpawne.instance.AdEnded();
        }
        else
        {
            Debug.Log("idk anything else sir");
        }

        //resume game
        GameObject.FindObjectOfType<AudioManager>().PlayAllMusic();
        adPlaying = false;

        GameMaster.instance.SaveGame();

        //load next reward based video ad
        this.RequestRewardBasedVideo();
    }
    #endregion

    IEnumerator RutinaDeNet()
    {
        if (_notInternetText != null)
            _notInternetText.text = "No Internet Connection";
        yield return new WaitForSecondsRealtime(1f);
        if (_notInternetText != null)
            _notInternetText.text = "";
    }

    private IEnumerator WaitAndMakeTextDisappear(float waitTimeInSeconds)
    {
        yield return new WaitForSeconds(waitTimeInSeconds);
        shouldShowText = false;
    }

    void OnGUI()
    {
        if(shouldShowText)
        {
            var centeredStyle = GUI.skin.GetStyle("Label");
            centeredStyle.alignment = TextAnchor.UpperCenter;
            GUI.Label(new Rect(Screen.width / 2 - 50, Screen.height / 2 - 25, 100, 50), "Ad didn't load properly.", centeredStyle);
        }
    }

}
