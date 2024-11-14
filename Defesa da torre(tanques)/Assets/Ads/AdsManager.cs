using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AdsManager : MonoBehaviour
{
    public InitializedAds initializedAds;
    public BannerAds bannerAds;
    public IntertistialAds IntertistialAds;
    public RewardsAds rewardsAds;

    public static AdsManager instance{ get;private set; }

    private void Awake()
    {
        if(instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        DontDestroyOnLoad(gameObject);


        bannerAds.LoadBannerAd();
        IntertistialAds.LoadInterstitalAD();
        rewardsAds.LoadRewardedAd();

    }
}
