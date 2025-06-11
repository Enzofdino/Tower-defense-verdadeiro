using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public static Bullet instance;

    private void Awake()
    {
        instance = this;
    }

    [Header("References")]
    [SerializeField] private Rigidbody2D rb; // Rigidbody da bala
    [SerializeField] private AudioClip somtiro;

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f;
    [SerializeField] public float bulletdamage = 1f;
    
    [SerializeField] private float lifetime = 5f;

    private Vector2 direction; // Direção fixa da bala após disparo

    private void Start()
    {
        Destroy(gameObject, lifetime); // Destroi após certo tempo
        rb.velocity = direction * bulletSpeed; // Inicia movimento já com a direção definida
    }

    // Método chamado no disparo para definir a direção e tocar o som
    public void SetTarget(Transform _target)
    {
        // Calcula a direção uma única vez no momento do disparo
        direction = (_target.position - transform.position).normalized;

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && somtiro != null)
        {
            audio.PlayOneShot(somtiro);
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Tenta aplicar dano se o objeto tiver o script Health
        Health enemyHealth = other.gameObject.GetComponent<Health>();
        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletdamage);

            EnemyMovement enemyMovement = other.gameObject.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.FlashOnHit();
            }
        }

        Destroy(gameObject); // Destrói a bala após a colisão
    }
}
