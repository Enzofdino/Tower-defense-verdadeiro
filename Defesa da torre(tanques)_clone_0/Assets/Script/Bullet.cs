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
    [SerializeField] private Rigidbody2D rb; // Refer�ncia ao Rigidbody2D da bala

    [Header("Attributes")]
    [SerializeField] private float bulletSpeed = 5f; // Velocidade da bala
    [SerializeField] public float bulletdamage = 1f; // Dano causado pela bala
    [SerializeField] private float lifetime = 5f; // Tempo de vida da bala antes de ser destru�da
   


    private Transform target; // Alvo que a bala deve seguir

    private void Start()
    {
        // Destr�i a bala ap�s um tempo definido para evitar que fique na cena indefinidamente
        Destroy(gameObject, lifetime);
    }

    // M�todo para definir o alvo da bala
    public void SetTarget(Transform _target)
    {
        target = _target;

        AudioSource audio = GetComponent<AudioSource>();
        if (audio != null && somtiro != null)
        {
            audio.PlayOneShot(somtiro);
        }
    }

    private void FixedUpdate()
    {
        // Se n�o houver um alvo, n�o faz nada
        if (!target) return;
      

        // Calcula a dire��o em que a bala deve se mover em rela��o ao seu alvo
        Vector2 direction = (target.position - transform.position).normalized;

        // Aplica a velocidade � bala, movendo-a em dire��o ao alvo
        rb.velocity = direction * bulletSpeed;
        
      

    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Tenta obter o componente Health do objeto com o qual a bala colidiu
        Health enemyHealth = other.gameObject.GetComponent<Health>();

        if (enemyHealth != null)
        {
            enemyHealth.TakeDamage(bulletdamage);

            // Faz o inimigo piscar em vermelho
            EnemyMovement enemyMovement = other.gameObject.GetComponent<EnemyMovement>();
            if (enemyMovement != null)
            {
                enemyMovement.FlashOnHit();
            }
        }

        // Destr�i a bala ap�s colidir com qualquer objeto
        Destroy(gameObject);
    }


}
