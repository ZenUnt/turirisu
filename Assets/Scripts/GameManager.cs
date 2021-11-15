using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System;
using GoogleMobileAds.Api;

public class GameManager : MonoBehaviour
{
    public Text textScore;
    public Text textTime;
    public GameObject[] FishPrefabs;
    public GameObject FinishedUI;
    public Text textScoreResult;
    public Text textHighScore;
    public Text textNewHighScoreLabel;

    private const int MAX_FISH_NUM = 6; // 同時に存在していい最大の魚の数
    private const float GAME_TIME = 60f; // 1ゲームのプレイ時間
    private const string KEY_HIGH_SCORE = "HIGH_SCORE"; // ハイスコア記録用キー
    private const string APP_ID = "ca-app-pub-7704991391324961~5723780640";
    private const string ADS_ID = "ca-app-pub-7704991391324961/9532514784";

    private int RspanTime = 1200; // 魚が発生する時間間隔(ミリ秒)
    private int score;
    private int numScoreFish; // 釣り上げた魚の数
    private int numFish; // 現在泳いでいる魚の数
    // 魚生成間隔用
    private DateTime lastCreateFishTime;
    private TimeSpan timeSpan;
    // 残り時間用
    private float second;
    private float oldSecond; // 前に更新した時の秒数
    private BannerView bannerView;


    public enum GAME_MODE {
        PLAY,
        FINISH,
    };

    private GAME_MODE gameMode = GAME_MODE.PLAY;

    void Start() {
        score = 0;
        numScoreFish = 0;
        numFish = 3;
        lastCreateFishTime = DateTime.UtcNow;
        second = 0f;
        oldSecond = 0f;

        // Initialize the Google Mobile Ads SDK.
        MobileAds.Initialize(APP_ID);
    }

    void FixedUpdate() {
        if (gameMode == GAME_MODE.PLAY) {
            timeSpan = DateTime.UtcNow - lastCreateFishTime;
            // RESPAN_TIME秒毎にブロックを生成
            if (timeSpan >= TimeSpan.FromMilliseconds(RspanTime)) {
                if (numFish < MAX_FISH_NUM) {
                    CreateNewFish();
                    lastCreateFishTime = DateTime.UtcNow;
                }
            }

            second += Time.deltaTime;
            if (second >= GAME_TIME) {
                // ゲーム終了
                GameOver();
            }
            if (second - oldSecond > 1) {
                oldSecond = second;
                textTime.text = ((int)GAME_TIME - (int)second).ToString();
            }
        }
   }

    // 新しい魚の生成
    public void CreateNewFish() {
        
        int ran = 0;
        if (numScoreFish <= 3) {
            ran = UnityEngine.Random.Range(0, 3);
        } else if (numScoreFish <= 10) {
            ran = UnityEngine.Random.Range(0, 4);
        } else if (numScoreFish <= 15) {
            ran = UnityEngine.Random.Range(2, 5);
        } else {
            ran = UnityEngine.Random.Range(3, 5);
        }
        GameObject fish = (GameObject)Instantiate(FishPrefabs[ran]);
        fish.transform.localPosition = new Vector3(
            4,
            UnityEngine.Random.Range(-4f, -1.5f),
            0f);
        numFish++;
    }

    public void GameOver() {
        RequestBanner();
        textTime.text = "0";
        gameMode = GAME_MODE.FINISH;
        FinishedUI.SetActive(true);
        textScoreResult.text = score.ToString();
        int highScore = PlayerPrefs.GetInt(KEY_HIGH_SCORE);
        textHighScore.text = highScore.ToString();

        if (score > highScore) {
            highScore = score;
            PlayerPrefs.SetInt(KEY_HIGH_SCORE, score);
            textNewHighScoreLabel.text = "NEW RECORD!!!";
        }
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

    public void AddFishNum() {
        numFish++;
    }

    public void MinusFishNum() {
        numFish--;
    }

    public void AddScoreFishNum() {
        numScoreFish++;
    }

    public void MinusScoreFishNum() {
        numScoreFish--;
    }

    public GAME_MODE GetGameMode() {
        return gameMode;
    }

    public void AddScore(int num) {
        score += num;
        textScore.text = score.ToString();
    }

    public void ChangeRespanTime(int time) {
        RspanTime = time;
    }

    public void RestartGame() {
        bannerView.Destroy();
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
