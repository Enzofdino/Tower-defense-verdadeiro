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
            // Destrói apenas os elementos de interface e câmera dos jogadores que não são o dono do objeto
            if (hudCanvas != null) Destroy(hudCanvas.gameObject);
            if (gameOverCanvas != null) Destroy(gameOverCanvas.gameObject);
            if (playerCamera != null) Destroy(playerCamera.gameObject);
        }
        else
        {
            // Se for o dono, inicia normalmente
            GetComponent<Rigidbody>().isKinematic = true;
            Invoke(nameof(AlteraPos), 5f);
        }

        base.OnNetworkSpawn();
    }

    void AlteraPos()
    {
        transform.position = new Vector3(0, 5, 0);
        GetComponent<Rigidbody>().isKinematic = false;
    }

    private IEnumerator RespawnPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        Vector3 respawnPosition = new Vector3(0, 10, 0);
        GameObject newPlayer = Instantiate(gameObject, respawnPosition, Quaternion.identity);
        newPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
    }
}
