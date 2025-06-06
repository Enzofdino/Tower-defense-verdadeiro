using System;
using UnityEngine;
using System.Collections.Generic;
using UnityEngine.VFX;
using Unity.Netcode;
using System.Collections;

public class PlayerCamera : NetworkBehaviour
{
    public override void OnNetworkSpawn()
    {

        if (!IsOwner)
        {
            Destroy(this.GetComponentInChildren<Camera>());
        }
        else
        {
            GetComponent<Rigidbody>().isKinematic = true;
            Invoke("AlteraPos", 5f);
        }


        base.OnNetworkSpawn();
    }
    public void AlteraPos()
    {
        this.gameObject.transform.position = new Vector3(0, 5, 0);
        GetComponent<Rigidbody>().isKinematic = false;
    }
    private IEnumerator RespawnPlayerAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);

        // Define a posição acima do mapa
        Vector3 respawnPosition = new Vector3(0, 10, 0);

        // Instancia o novo jogador
        GameObject newPlayer = Instantiate(this.gameObject, respawnPosition, Quaternion.identity);

        // Spawna na rede com o mesmo dono
        newPlayer.GetComponent<NetworkObject>().SpawnAsPlayerObject(OwnerClientId);
    }
}
