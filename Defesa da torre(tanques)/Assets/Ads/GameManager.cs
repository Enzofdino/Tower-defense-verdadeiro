using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public int score = 0;

    public int gamePlayed = 1;
   public static GameManager Instance { get; private set; }

    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject);

        AdsManager.instance.bannerAds.ShowBannerAD();

        StartCoroutine(DisplayBannerWithDelay());
    }

    private IEnumerator DisplayBannerWithDelay()
    {
        yield return new WaitForSeconds(1f);
        AdsManager.instance.bannerAds.ShowBannerAD();
    }
    private void Update()
    {
        if(score < 0)
        {
            score = 0;
            AdsManager.instance.bannerAds.HideBannerAd();
            SceneManager.LoadScene("EndScene");

            if (gamePlayed % 3 == 0)
            {
                AdsManager.instance.IntertistialAds.ShowInterstitialAd();
            }
        }
        else if(score > 10)
        {
            score = 10;
            AdsManager.instance.bannerAds.HideBannerAd();
            SceneManager.LoadScene("EndScene");

            if(gamePlayed % 3 ==0)
            {
                AdsManager.instance.IntertistialAds.ShowInterstitialAd();
            }
        }
    }
    public void RestartGame()
    {
        score = 0;
        gamePlayed++;
        AdsManager.instance.bannerAds.ShowBannerAD();

        SceneManager.LoadScene("Gameplay");
    }

    public void AddScore()
    {
        score++;
    }

    public void MinusScore()
    {
        score--;
    }
}
