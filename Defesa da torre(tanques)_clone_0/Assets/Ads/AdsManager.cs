using UnityEngine;
using UnityEngine.Advertisements;
using System.Collections;

// Classe responsável por gerenciar os anúncios do Unity Ads
public class AdManager : MonoBehaviour, IUnityAdsInitializationListener, IUnityAdsShowListener
{
    // Singleton para acesso global ao AdManager
    public static AdManager instance;

    // ID do projeto Unity Ads
    private string gameId = "5730170";

    // Define se o modo de teste está ativado
    private bool testMode = true;

    // IDs para diferentes tipos de anúncios no painel do Unity Ads
    private string interstitialAdId = "Interstitial_Android"; // ID do anúncio intersticial
    private string bannerAdId = "Banner_Android"; // ID do anúncio de banner
    private string rewardedAdId = "Rewarded_Android"; // ID do anúncio recompensado
    private string dontskipId = "noskip"; // ID do anúncio não pulável

    // Alterna entre exibição de anúncios intersticiais e não puláveis
    private bool showInterstitialNext = true;

    // Coroutine para controlar a exibição de banners em loop
    private Coroutine bannerLoopCoroutine;

    // Indica se um anúncio intersticial está sendo exibido
    private bool isShowingInterstitial = false;

    // Indica se o jogo está pausado devido à exibição de um anúncio
    public bool isGamePausedByAd = false;

    // Delegate para definir ações de recompensa ao assistir anúncios
    public delegate void gifts();
    private gifts recompensa;

    // Método chamado ao inicializar o script. Configura o singleton
    private void Awake()
    {
        instance = this; // Define esta instância como o singleton
    }

    // Método chamado no início do script. Inicializa os anúncios
    void Start()
    {
        InitializeAds(); // Inicializa os anúncios do Unity Ads
    }

    // Inicializa o Unity Ads com o game ID e o modo de teste
    private void InitializeAds()
    {
        if (!Advertisement.isInitialized) // Verifica se os anúncios não foram inicializados
        {
            Advertisement.Initialize(gameId, testMode, this); // Inicializa os anúncios
        }
    }

    // Callback chamado ao finalizar a inicialização dos anúncios
    public void OnInitializationComplete()
    {
        Debug.Log("Unity Ads initialization complete."); // Mensagem de sucesso no console

        // Inicia o loop de banners após a inicialização bem-sucedida
        bannerLoopCoroutine = StartCoroutine(BannerLoop());
    }

    // Callback chamado quando ocorre um erro na inicialização do Unity Ads
    public void OnInitializationFailed(UnityAdsInitializationError error, string message)
    {
        Debug.LogError($"Unity Ads Initialization Failed: {error} - {message}"); // Log de erro
    }

    // Coroutine que exibe e oculta banners de forma cíclica
    private IEnumerator BannerLoop()
    {
        while (true) // Loop infinito para gerenciar os banners
        {
            if (!isShowingInterstitial) // Exibe o banner somente se nenhum intersticial estiver sendo exibido
            {
                ShowBannerAd(); // Exibe o banner
                yield return new WaitForSeconds(10); // Mantém o banner visível por 10 segundos
                HideBannerAd(); // Oculta o banner
            }

            yield return new WaitForSeconds(5); // Aguarda 5 segundos antes de repetir
        }
    }

    // Exibe um banner na posição definida
    private void ShowBannerAd()
    {
        Advertisement.Banner.SetPosition(BannerPosition.TOP_CENTER); // Define a posição do banner
        Advertisement.Banner.Load(bannerAdId, new BannerLoadOptions
        {
            loadCallback = OnBannerLoaded, // Callback para sucesso no carregamento
            errorCallback = OnBannerError // Callback para erro no carregamento
        });
    }

    // Callback chamado quando um banner é carregado com sucesso
    private void OnBannerLoaded()
    {
        Advertisement.Banner.Show(bannerAdId); // Exibe o banner
        Debug.Log("Banner ad loaded and displayed."); // Mensagem de sucesso no console
    }

    // Callback chamado ao ocorrer erro no carregamento do banner
    private void OnBannerError(string message)
    {
        Debug.LogError($"Failed to load banner ad: {message}"); // Log de erro no console
    }

    // Oculta o banner atualmente exibido
    private void HideBannerAd()
    {
        Advertisement.Banner.Hide(); // Oculta o banner
    }

    // Exibe um anúncio intersticial
    public void ShowInterstitialAd()
    {
        if (!isGamePausedByAd) // Verifica se o jogo não está pausado
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true; // Marca que o jogo foi pausado por um anúncio
        }
        Advertisement.Show(interstitialAdId, this); // Exibe o anúncio intersticial
        isShowingInterstitial = true; // Marca que um intersticial está sendo exibido
    }

    // Exibe um anúncio não pulável
    private void dontskip()
    {
        if (!isGamePausedByAd) // Verifica se o jogo não está pausado
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true; // Marca que o jogo foi pausado por um anúncio
        }
        Advertisement.Show(dontskipId, this); // Exibe o anúncio não pulável
        isShowingInterstitial = true; // Marca que um intersticial está sendo exibido
    }

    // Alterna entre exibir um intersticial ou um anúncio não pulável
    public void ShowNextAd()
    {
        if (showInterstitialNext) // Verifica qual tipo de anúncio exibir
        {
            ShowInterstitialAd(); // Exibe um anúncio intersticial
        }
        else
        {
            dontskip(); // Exibe um anúncio não pulável
        }

        showInterstitialNext = !showInterstitialNext; // Alterna o tipo de anúncio para a próxima exibição
    }

    // Exibe um anúncio recompensado e define a ação a ser executada após assistir
    public void ShowRewardedAdForAction(gifts action)
    {
        recompensa = action; // Define a ação de recompensa
        ShowRewardedAd(); // Exibe o anúncio recompensado
    }

    // Exibe um anúncio recompensado
    public void ShowRewardedAd()
    {
        if (!isGamePausedByAd) // Verifica se o jogo não está pausado
        {
            Time.timeScale = 0; // Pausa o jogo
            isGamePausedByAd = true; // Marca que o jogo foi pausado por um anúncio
        }
        Advertisement.Show(rewardedAdId, this); // Exibe o anúncio recompensado
    }

    // Callback chamado ao finalizar a exibição de um anúncio
    public void OnUnityAdsShowComplete(string placementId, UnityAdsShowCompletionState showCompletionState)
    {
        if (showCompletionState == UnityAdsShowCompletionState.COMPLETED) // Verifica se o anúncio foi concluído
        {
            if (placementId == rewardedAdId) // Verifica se é um anúncio recompensado
            {
                recompensa?.Invoke(); // Executa a ação de recompensa
            }
            else if (placementId == interstitialAdId) // Verifica se é um intersticial
            {
                isShowingInterstitial = false; // Define que nenhum intersticial está sendo exibido
            }
        }

        if (isGamePausedByAd) // Verifica se o jogo estava pausado por um anúncio
        {
            Time.timeScale = 1; // Retoma o jogo
            isGamePausedByAd = false; // Marca que o jogo não está mais pausado
        }
    }

    // Callback chamado se ocorrer erro ao exibir o anúncio
    public void OnUnityAdsShowFailure(string placementId, UnityAdsShowError error, string message)
    {
        Debug.LogError($"Unity Ads failed to show: {placementId}, Error: {error}, Message: {message}"); // Log de erro
    }

    // Callback chamado quando um anúncio começa a ser exibido
    public void OnUnityAdsShowStart(string placementId)
    {
        Debug.Log($"Unity Ads started showing: {placementId}"); // Log de início de exibição
        Advertisement.Banner.Hide(); // Oculta o banner enquanto o anúncio está ativo
    }

    // Callback chamado quando um anúncio é clicado
    public void OnUnityAdsShowClick(string placementId)
    {
        Debug.Log($"Unity Ads clicked: {placementId}"); // Log de clique no anúncio
    }

    // Ação para reviver o jogador após um anúncio
    public void revives()
    {
        if (LevelManager.instance != null) // Verifica se há uma instância do LevelManager
        {
            LevelManager.instance.Reiniciar(); // Revive o jogador
        }
    }

    // Ação para recompensar o jogador após um anúncio
    public void reward()
    {
        if (LevelManager.instance != null) // Verifica se há uma instância do LevelManager
        {
            LevelManager.instance.RewardCurrency(); // Dá a recompensa ao jogador
        }
    }

    // Método vinculado a um botão para exibir um anúncio recompensado que dá uma recompensa
    public void buttonreward()
    {
        ShowRewardedAdForAction(reward); // Exibe o anúncio com ação de recompensa
    }

    // Método vinculado a um botão para exibir um anúncio recompensado que revive o jogador
    public void buttonRevive()
    {
        ShowRewardedAdForAction(revives); // Exibe o anúncio com ação de reviver
    }

    // Método chamado ao destruir o objeto. Cancela o loop de banners
    private void OnDestroy()
    {
        if (bannerLoopCoroutine != null) // Verifica se há uma coroutine de banner em execução
        {
            StopCoroutine(bannerLoopCoroutine); // Para a coroutine
        }
    }
}
