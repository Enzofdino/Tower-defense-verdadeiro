using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    public static AdManager instance;
    private string gameId = "5730170"; // Seu ID do projeto
    private bool testMode = true;
    private string interstitialAdId = "Interstitial_Android";
    private string bannerAdId = "Banner_Android"; // Verifique se este é o ID correto no painel do Unity Ads
    private string rewardedAdId = "Rewarded_Android"; // ID do anúncio recompensado
    private string naoPulavelId = "noskip"; // ID do anúncio não pulável
    private bool showInterstitialNext = true; // Controla qual tipo de anúncio será mostrado: intersticial ou não pulavel

    private Coroutine bannerLoopCoroutine; // Armazena a coroutine do BannerLoop
    private bool isShowingInterstitial = false; // Indica se o intersticial está sendo exibido
    private System.Action adCompletedAction; // Ação após a exibição do anúncio
    public bool isGamePausedByAd = false; // Indica se o jogo está pausado por causa de um anúncio


    private delegate void gifts();
    private gifts recompensa;
    private gifts reviver;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        InitializeAds();
        recompensa = Recompensa;
        reviver = Reviver;
    }

    private void InitializeAds()
    {
        if (!Advertisement.isInitialized)
        {
            Advertisement.Initialize(gameId, testMode, this);
        }
    }

    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete.");

        // Inicia o loop de banners após a inicialização
        bannerLoopCoroutine = StartCoroutine(BannerLoop());
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}");
    }

    private IEnumerator BannerLoop()
    {
        while (true)
        {
            if (!isShowingInterstitial) // Só exibe o banner se o intersticial não estiver ativo
            {
                ShowBannerAd();
                yield return new WaitForSeconds(10); // Exibe o banner por 10 segundos
                HideBannerAd(); // Oculta o banner
            }

            yield return new WaitForSeconds(5); // Espera 5 segundos antes de exibir novamente
        }
    }

    private void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);
        Advertisement.Banner.Load(bannerAdId, new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        });
    }

    private void OnBannerLoaded()
    {
        Advertisement.Banner.Show(bannerAdId);
        Debug.Log("Banner ad loaded and displayed.");
    }

    private void OnBannerError(string message)
    {
        Debug.LogError($"Failed to load banner ad: {message}");
    }

    private void HideBannerAd()
    {
        Advertisement.Banner.Hide();
    }

    private void Naopulavel()
    {
        Advertisement.Show(naoPulavelId, this); // Exibe o anúncio não pulável
    }

    void Recompensa()
    {
        if (LevelManager.instance != null)
        {
            LevelManager.instance.RewardCurrency(); // Dá a recompensa ao jogador
        }
        else
        {
            Debug.LogError("LevelManager.main is null. Cannot reward currency.");
        }
    }

    public void ShowInterstitialAd()
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o tempo do jogo
            isGamePausedByAd = true; // Marca que o jogo está pausado por causa do anúncio
        }
        Advertisement.Show(interstitialAdId, this); // Exibe o anúncio intersticial
        isShowingInterstitial = true; // Define que o intersticial está sendo exibido
    }


    public void ShowRewardedAd(System.Action actionAfterAd)
    {
        if (!isGamePausedByAd)
        {
            Time.timeScale = 0; // Pausa o tempo do jogo
            isGamePausedByAd = true; // Marca que o jogo está pausado por causa do anúncio
        }
        adCompletedAction = actionAfterAd; // Armazena a ação que será executada após o anúncio
        Advertisement.Show(rewardedAdId, this); // Exibe o anúncio recompensado
    }


    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            if (placementId == rewardedAdId)
            {
                adCompletedAction?.Invoke(); // Executa a ação armazenada (Recompensa ou Reviver)
            }
            else if (placementId == interstitialAdId)
            {
                isShowingInterstitial = false; // Define que o intersticial foi fechado
            }
        }

        // Restaura o tempo normal apenas se o jogo estava pausado por um anúncio
        if (isGamePausedByAd)
        {
            Time.timeScale = 1;
            isGamePausedByAd = false; // Reseta o estado de pausa por anúncio
        }
    }


    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Unity Ads failed to show: {placementId}, Error: {error}, Message: {message}");
    }

    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Unity Ads started showing: {placementId}");
        Advertisement.Banner.Hide(); // Esconde o banner durante o anúncio
    }

    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Unity Ads clicked: {placementId}");
        Advertisement.Banner.Show(bannerAdId); // Exibe o banner novamente após o clique
    }

    public void Reviver()
    {
        LevelManager.instance.Reiniciar(); // Ação de reviver o jogador
    }

    public void BotaoRecompensa()
    {
        ShowRewardedAd(Recompensa); // Mostra o anúncio e dá a recompensa
    }

    public void BotaoReviver()
    {
        ShowRewardedAd(Reviver); // Passa o método Reviver para ser executado após o anúncio
    }

    public void ShowNextAd()
    {
        if (showInterstitialNext)
        {
            ShowInterstitialAd();  // Mostra o anúncio intersticial
        }
        else
        {
            Naopulavel();  // Mostra o anúncio não pulável
        }

        // Alterna o tipo de anúncio para o próximo
        showInterstitialNext = !showInterstitialNext;
    }
}
