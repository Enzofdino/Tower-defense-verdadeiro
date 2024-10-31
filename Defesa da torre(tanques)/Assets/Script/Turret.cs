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

    private void OnDrawGizmosSelected() // Método chamado para desenhar gizmos na cena para visualização.
    {
        Handles.color = Color.cyan; // Define a cor do gizmo.
        Handles.DrawWireDisc(transform.position, transform.forward, targetingrange); // Desenha um disco indicando o alcance de ataque da torre.
    }

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

    private void RotateTowardsTarget() // Método para rotacionar a torre em direção ao alvo.
    {
        if (target == null) // Se não há alvo, exibe um aviso no console.
        {
            Debug.LogWarning("Nenhum alvo detectado para rotacionar.");
            return; // Sai do método se não houver alvo.
        }

        // Calcula o ângulo entre a torre e o alvo.
        float angle = Mathf.Atan2(target.position.y - transform.position.y, target.position.x - turretRotationPoint.position.x) * Mathf.Rad2Deg - 90;

        // Cria uma rotação desejada para a torre em direção ao alvo.
        Quaternion targetRotation = Quaternion.Euler(new Vector3(0f, 0f, angle));
        turretRotationPoint.rotation = Quaternion.RotateTowards(turretRotationPoint.rotation, targetRotation, rotationspeed * Time.deltaTime); // Rotaciona a torre suavemente em direção ao alvo.
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

    private void Findtarget() // Método para encontrar um novo alvo para a torre.
    {
        // Realiza um círculo de detecção para encontrar inimigos dentro do alcance.
        RaycastHit2D[] hits = Physics2D.CircleCastAll(transform.position, targetingrange, (Vector2)transform.position, 0f, enemyMask);

        if (hits.Length > 0) // Se houver alvos detectados.
        {
            target = hits[0].transform; // Define o primeiro alvo detectado como o alvo atual.
        }
    }
}
