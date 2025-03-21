using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;
    public AudioClip somtiro;
    private void Awake()
    {
        instance = this;
    }
    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Referência ao Rigidbody2D da bala

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f; // Velocidade da bala
    [SerializeField] public float bulletdamage = 1f; // Dano causado pela bala
    [SerializeField] private float lifetime = 5f; // Tempo de vida da bala antes de ser destruída

    private Transform target; // Alvo que a bala deve seguir

    private void Start()
    {
        // Destrói a bala após um tempo definido para evitar que fique na cena indefinidamente
        Destroy(gameObject, lifetime);
    }

    // Método para definir o alvo da bala
    public void SetTarget(Transform _target)
    {
        target = _target; // Atribui o alvo recebido à variável target
        gameObject.GetComponent<AudioSource>().Play();
    }

    private void FixedUpdate()
    {
        // Se não houver um alvo, não faz nada
        if (!target) return;
      

        // Calcula a direção em que a bala deve se mover em relação ao seu alvo
        Vector2 direction = (target.position - transform.position).normalized;

        // Aplica a velocidade à bala, movendo-a em direção ao alvo
        rb.velocity = direction * bulletSpeed;
        
      

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Tenta obter o componente Health do objeto com o qual a bala colidiu
        Health enemyHealth = other.gameObject.GetComponent<Health>();

        // Se o objeto tiver um componente Health, aplica dano
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletdamage); // Aplica o dano à saúde do inimigo
            // Aqui você pode adicionar um efeito visual ou sonoro para o impacto
        }

        // Destrói a bala após colidir com qualquer objeto
        Destroy(gameObject);
    }
}
