using UnityEngine;
using UnityEngine.Advertisements;

public class AdsManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener, IUnityAdsLoadListener
{
    private string gameId = "5730311";  // Game ID para Android (substitua pelo seu Game ID)
    private bool testMode = true;  // Modo de teste ativo
    private string bannerPlacementId = "Banner_Android";  // ID do banner para Android (substitua pelo seu ID)

    void Start()
    {
        // Inicialização do Unity Ads
        Advertisement.Initialize(gameId, testMode, this);
    }

    // Carregar e mostrar o banner
    public void LoadAndShowBanner()
    {
        BannerLoadOptions options = new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded,
            errorCallback = OnBannerError
        };

        // Configura a posição do banner para o centro superior
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER);  // Posiciona o banner no topo centralizado
        Advertisement.Banner.Load(bannerPlacementId, options);  // Carrega o banner com o ID configurado
    }

    // Callbacks para o banner
    void OnBannerLoaded()
    {
        Advertisement.Banner.Show(bannerPlacementId);  // Exibe o banner quando carregado
        Debug.Log("Banner loaded and displayed at the top center.");
    }

    void OnBannerError(string message)
    {
        Debug.LogError($"Failed to load banner: {message}");  // Mensagem de erro caso o banner falhe
    }

    // Callbacks da inicialização do Unity Ads
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialized successfully.");
        // Carregar e exibir o banner após a inicialização
        LoadAndShowBanner();
    }

    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads initialization failed: {error} - {message}");
    }

    // Callbacks da exibição de anúncios recompensados
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (placementId == "Rewarded_Android" && showCompletionState == UnityAdsShowCompletionState.COMPLETED)
        {
            Debug.Log("Player earned reward!");  // Exibe uma mensagem quando o anúncio for concluído com sucesso
        }
    }

    // Outros callbacks opcionais para o Unity Ads
    public void OnUnityAdsFailedToLoad(string placementId, UnityAdsLoadError error, string message)
    {
        Debug.LogError($"Failed to load ad {placementId}: {error} - {message}");
    }

    public void OnUnityAdsAdLoaded(string placementId)
    {
        if (placementId == "Rewarded_Android")
        {
            Advertisement.Show("Rewarded_Android", this);  // Exibe o anúncio recompensado quando carregado
            Debug.Log("Rewarded Ad loaded and displayed.");
        }
    }

    // Outros callbacks (opcional)
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Failed to show ad {placementId}: {error} - {message}");  // Mensagem de erro ao exibir o anúncio
    }

    public void OnUnityAdsShowStart(string placementId) { }  // Callback de início do anúncio
    public void OnUnityAdsShowClick(string placementId) { }  // Callback de clique no anúncio
}
