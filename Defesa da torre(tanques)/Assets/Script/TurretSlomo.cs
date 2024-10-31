using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

// Classe que representa uma torre que desacelera inimigos
public class TurretSlomo : Turret
{
    [SerializeField] private float aps = 4f; // Ataques por segundo
    [SerializeField] private float FreezeTime = 5f; // Tempo em que os inimigos ficarão congelados

    private void Update()
    {
        RotateTowardsTarget(); // Rotaciona a torre em direção ao alvo
        timeUntilFire += Time.deltaTime; // Atualiza o tempo até o próximo ataque

        if (timeUntilFire >= 1f / aps) // Verifica se é hora de atacar
        {
            FreezeEnemies(); // Congela os inimigos
            timeUntilFire = 0f; // Reseta o temporizador de ataque
        }
    }

    private void FreezeEnemies()
    {
        // Detecta inimigos dentro da área de alcance
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0) // Se houver inimigos
        {
            for (int i = 0; i < hits.Length; i++)
            {
                RaycastHit2D hit = hits[i];
                EnemyMovement em = hit.transform.GetComponent<EnemyMovement>(); // Obtém o componente de movimentação do inimigo

                em.UpdateSpeed(0.5f); // Reduz a velocidade do inimigo
                StartCoroutine(ResetEnemySpeed(em)); // Inicia a coroutine para resetar a velocidade
            }
        }
    }

    private IEnumerator ResetEnemySpeed(EnemyMovement em)
    {
        yield return new WaitForSeconds(FreezeTime); // Espera pelo tempo de congelamento
        em.ResetSpeed(); // Restaura a velocidade original do inimigo
    }

    private void RotateTowardsTarget()
    {
        if (target == null)
        {
            return; // Se não houver alvo, não faz nada
        }

        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime);
    }
}
