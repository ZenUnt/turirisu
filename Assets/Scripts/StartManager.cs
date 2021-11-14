using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using GoogleMobileAds.Api;

public class StartManager : MonoBehaviour
{
    private BannerView bannerView;
    private const string APP_ID = "ca-app-pub-7704991391324961~5723780640";
    private const string ADS_ID = "ca-app-pub-7704991391324961/9532514784";

    void Start()
    {
        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(APP_ID);
        RequestBanner();
    }

    void Update()
    {
        
    }

    public void StartGame() {
        bannerView.Destroy();
        SceneManager.LoadScene("GameScene");
    }

    public void LinkPrivacyPolicy() {
        Application.OpenURL("https://hayatoyagame.com/policy.html");
    }

    private void RequestBanner() {

        // 広告ユニットID
        string adUnitId = ADS_ID;
        // Create a 320x50 banner at the top of the screen.
        bannerView = new BannerView(adUnitId, AdSize.Banner, AdPosition.Bottom);
        // Create an empty ad request.
        AdRequest request = new AdRequest.Builder().Build();
        // Load the banner with the request.
        bannerView.LoadAd(request);
    }
}
