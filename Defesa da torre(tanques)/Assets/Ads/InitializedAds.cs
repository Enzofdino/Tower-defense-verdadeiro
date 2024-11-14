using UnityEngine;
using UnityEngine.Advertisements;

public class InitializedAds : MonoBehaviour,IUnityAdsInitializationListener
{
    [SerializeField] private string androidGameId;
    [SerializeField] private string iosGameId;
    [SerializeField] private bool isTesting;

    private string gameId;

    public void OnInitializationComplete()
    {
        Debug.Log("Ads Iniialized");
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        
    }

    private void Awake()
    {
      #if UNITY_IOS
      gameId = iosGameId;
      #elif UNITY_ANDROID
      gameId = androidGameId;
      #elif UNITY_EDITOR
      gameId = androidGameId;
      #endif

        if (!Advertisement.isInitialized && Advertisement.isSupported)
        {
            Advertisement.Initialize(gameId, isTesting, this);
        }

       




    }

}
