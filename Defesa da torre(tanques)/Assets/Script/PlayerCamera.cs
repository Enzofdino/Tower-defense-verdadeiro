using System.Collections;
using UnityEngine;
using Unity.Netcode;

public class PlayerCamera : NetworkBehaviour
{
    [Header("Referências da Cena")]
    public Canvas hudCanvas;         // Canvas do HUD (vida, botões, etc)
    public Canvas gameOverCanvas;   // Canvas de Game Over
    public Camera playerCamera;

    void Start()
    {
        // Garante que os Canvas e a Câmera estão corretamente referenciados
        if (hudCanvas == null || gameOverCanvas == null || playerCamera == null)
        {
            Debug.LogError("Referências de UI ou Câmera não foram atribuídas!");
        }
    }

    public override void OnNetworkSpawn()
    {
        if (!IsOwner)
        {
            // Destrói apenas os elementos que devem ser exclusivos do jogador local
            if (hudCanvas != null) Destroy(hudCanvas.gameObject);
            if (gameOverCanvas != null) Destroy(gameOverCanvas.gameObject);
            if (playerCamera != null) Destroy(playerCamera.gameObject);
        }
        else
        {
            // Inicializações do jogador local
            GetComponent<Rigidbody>().isKinematic = true;
            Invoke(nameof(AlteraPos), 5f);
        }
    }


    void AlteraPos()
    {
        transform.position = new Vector3(0, 5, 0);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private IEnumerator RespawnPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        if (IsServer) // Apenas o servidor pode spawnar jogadores
        {
            Vector3 respawnPosition = new Vector3(0, 10, 0);
            GameObject newPlayer = Instantiate(NetworkManager.Singleton.NetworkConfig.PlayerPrefab, respawnPosition, Quaternion.identity);
            newPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
        }
    }

}
