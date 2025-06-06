using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour, Iatacavel // Classe que representa uma torre que pode atacar inimigos.
{
    [SerializeField] public Transform turretRotationPoint; // Ponto de rotação da torre.

    [SerializeField] protected float targetingrange = 5f; // Distância máxima que a torre pode atacar.

    [SerializeField] protected LayerMask enemyMask; // Máscara de camada para identificar inimigos.

    [SerializeField] protected GameObject bulletPrefab; // Prefab do projétil que a torre irá disparar.

    [SerializeField] protected Transform firingPoint; // Ponto onde o projétil será instanciado.

    [SerializeField] public float rotationspeed = 10f; // Velocidade de rotação da torre.

    [SerializeField] private float bps = 1f; // Disparos por segundo (bps) que a torre pode fazer.

    protected Transform target; // Referência ao alvo atual que a torre está atacando.

    protected float timeUntilFire; // Tempo restante até o próximo disparo.

    public virtual void Atacar() // Método virtual que pode ser sobrescrito por classes derivadas para implementar o ataque.
    {
        // Método a ser implementado em subclasses.
    }

  /*  private void OnDrawGizmosSelected() // Método chamado para desenhar gizmos na cena para visualização.
    {
        Handles.color = Color.cyan; // Define a cor do gizmo.
        Handles.DrawWireDisc(transform.position, transform.forward, targetingrange); // Desenha um disco indicando o alcance de ataque da torre.
    }*/

    private void Update() // Método chamado uma vez por quadro para atualizar o estado da torre.
    {
        if (target == null) // Verifica se não há um alvo definido.
        {
            Findtarget(); // Tenta encontrar um novo alvo.
            return; // Sai do método se não houver alvo.
        }

        RotateTowardsTarget(); // Faz a torre girar em direção ao alvo.

        if (!checktargetisrange()) // Verifica se o alvo está fora do alcance.
        {
            target = null; // Reseta o alvo se estiver fora do alcance.
        }
        else // Se o alvo ainda estiver dentro do alcance.
        {
            timeUntilFire += Time.deltaTime; // Atualiza o tempo até o próximo disparo.

            if (timeUntilFire >= 1f / bps) // Verifica se é hora de disparar.
            {
                Shoot(); // Executa o disparo.
                timeUntilFire = 0f; // Reseta o temporizador.
            }
        }
    }

    private void RotateTowardsTarget()
    {
        if (target == null)
        {
            Debug.LogWarning("Nenhum alvo detectado para rotacionar.");
            return;
        }

        float angle = Mathf.Atan2(
            target.position.y - turretRotationPoint.position.y,
            target.position.x - turretRotationPoint.position.x
        ) * Mathf.Rad2Deg;

        Quaternion targetRotation = Quaternion.Euler(0f, 0f, angle - 90f);
        turretRotationPoint.rotation = Quaternion.RotateTowards(
            turretRotationPoint.rotation,
            targetRotation,
            rotationspeed * Time.deltaTime
        );
    }


    protected virtual void Shoot() // Método virtual que pode ser sobrescrito por classes derivadas para implementar o disparo.
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); // Instancia o projétil no ponto de disparo.

        Bullet bulletScript = bulletObj.GetComponent<Bullet>(); // Obtém o script do projétil.
        bulletScript.SetTarget(target); // Define o alvo do projétil.
    }

    private bool checktargetisrange() // Verifica se o alvo está dentro do alcance da torre.
    {
        return Vector2.Distance(target.position, transform.position) <= targetingrange; // Retorna verdadeiro se o alvo estiver dentro do alcance.
    }

    private void Findtarget()
    {
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, Vector2.zero, 0f, enemyMask);

        Transform maisAvancado = null;
        int maiorProgresso = -1;

        foreach (var hit in hits)
        {
            EnemyMovement inimigo = hit.transform.GetComponent<EnemyMovement>();
            if (inimigo != null)
            {
                int progressoAtual = inimigo.GetProgress();

                if (progressoAtual > maiorProgresso)
                {
                    maiorProgresso = progressoAtual;
                    maisAvancado = hit.transform;
                }
                else if (progressoAtual == maiorProgresso)
                {
                    // Se houver empate, escolhe o inimigo mais próximo da torre (opcional)
                    if (maisAvancado != null)
                    {
                        float distAtual = Vector2.Distance(transform.position, hit.transform.position);
                        float distAnterior = Vector2.Distance(transform.position, maisAvancado.position);
                        if (distAtual < distAnterior)
                            maisAvancado = hit.transform;
                    }
                    else
                    {
                        maisAvancado = hit.transform;
                    }
                }
            }
        }

        target = maisAvancado;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireSphere(transform.position, targetingrange);
    }


}
