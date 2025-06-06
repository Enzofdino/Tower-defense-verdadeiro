using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class Turret : MonoBehaviour, Iatacavel // Classe que representa uma torre que pode atacar inimigos.
{
    [SerializeField] public Transform turretRotationPoint; // Ponto de rota��o da torre.

    [SerializeField] protected float targetingrange = 5f; // Dist�ncia m�xima que a torre pode atacar.

    [SerializeField] protected LayerMask enemyMask; // M�scara de camada para identificar inimigos.

    [SerializeField] protected GameObject bulletPrefab; // Prefab do proj�til que a torre ir� disparar.

    [SerializeField] protected Transform firingPoint; // Ponto onde o proj�til ser� instanciado.

    [SerializeField] public float rotationspeed = 10f; // Velocidade de rota��o da torre.

    [SerializeField] private float bps = 1f; // Disparos por segundo (bps) que a torre pode fazer.

    protected Transform target; // Refer�ncia ao alvo atual que a torre est� atacando.

    protected float timeUntilFire; // Tempo restante at� o pr�ximo disparo.

    public virtual void Atacar() // M�todo virtual que pode ser sobrescrito por classes derivadas para implementar o ataque.
    {
        // M�todo a ser implementado em subclasses.
    }

  /*  private void OnDrawGizmosSelected() // M�todo chamado para desenhar gizmos na cena para visualiza��o.
    {
        Handles.color = Color.cyan; // Define a cor do gizmo.
        Handles.DrawWireDisc(transform.position, transform.forward, targetingrange); // Desenha um disco indicando o alcance de ataque da torre.
    }*/

    private void Update() // M�todo chamado uma vez por quadro para atualizar o estado da torre.
    {
        if (target == null) // Verifica se n�o h� um alvo definido.
        {
            Findtarget(); // Tenta encontrar um novo alvo.
            return; // Sai do m�todo se n�o houver alvo.
        }

        RotateTowardsTarget(); // Faz a torre girar em dire��o ao alvo.

        if (!checktargetisrange()) // Verifica se o alvo est� fora do alcance.
        {
            target = null; // Reseta o alvo se estiver fora do alcance.
        }
        else // Se o alvo ainda estiver dentro do alcance.
        {
            timeUntilFire += Time.deltaTime; // Atualiza o tempo at� o pr�ximo disparo.

            if (timeUntilFire >= 1f / bps) // Verifica se � hora de disparar.
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


    protected virtual void Shoot() // M�todo virtual que pode ser sobrescrito por classes derivadas para implementar o disparo.
    {
        GameObject bulletObj = Instantiate(bulletPrefab, firingPoint.position, Quaternion.identity); // Instancia o proj�til no ponto de disparo.

        Bullet bulletScript = bulletObj.GetComponent<Bullet>(); // Obt�m o script do proj�til.
        bulletScript.SetTarget(target); // Define o alvo do proj�til.
    }

    private bool checktargetisrange() // Verifica se o alvo est� dentro do alcance da torre.
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
                    // Se houver empate, escolhe o inimigo mais pr�ximo da torre (opcional)
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
