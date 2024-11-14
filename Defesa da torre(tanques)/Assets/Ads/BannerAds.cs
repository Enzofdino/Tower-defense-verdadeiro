using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Advertisements;

public class BannerAds : MonoBehaviour
{

    [SerializeField] private string androidAdUnitId;
    [SerializeField] private string iosAdUnitId;

    private string adUnitId;

    private void Awake()
    {
#if UNITY_IOS
adUnitId = iosAdUnintId;
#elif UNITY_ANDROID
adUnitId = androidAdUnitId;
#endif
        Advertisement.Banner.SetPosition(BannerPosition.BOTTOM_CENTER);
    }

    public void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }


    #region Show Callbacks
    public void ShowBannerAD()
    {
        BannerOptions options = new BannerOptions
        {
            showCallback = BannerShown,
            clickCallback = BannerClicked,
            hideCallback = BannerHidden
        };
        Advertisement.Banner.Show(adUnitId, options);
    }

    private void BannerHidden()
    {
    }

    private void BannerClicked()
    {
    }

    private void BannerShown()
    {
    }
    #endregion 

    #region LoadCallbacks
    public void LoadBannerAd()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = BannerLoaded,
            errorCallback = BannerLoadedError
        };
        Advertisement.Banner.Load(adUnitId, options);
    }

    private void BannerLoadedError(string message)
    {
        throw new NotImplementedException();
    }

    private void BannerLoaded()
    {
        Debug.Log("Banner Ad Loaded");
    }
    #endregion
}
