using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    static public EnemyMovement instance;
    private void Awake()
    {
        instance = this;
    }

    [SerializeField] private Rigidbody2D rb; // Rigidbody do inimigo
    [SerializeField] public float moveSpeed = 2f; // Velocidade de movimento do inimigo
    
    private Transform target; // Alvo atual
    private int pathIndex = 0; // �ndice do caminho
    private float baseSpeed; // Velocidade base do inimigo
    [SerializeField] private SpriteRenderer spriteRenderer; // arraste o SpriteRenderer no Inspector
    [SerializeField] private Color hitColor = Color.white; // cor do flash
    [SerializeField] private float flashDuration = 0.1f; // dura��o do flash
    private Color originalColor; // para restaurar depois


    private void Start()
    {
        baseSpeed = moveSpeed; // Armazena a velocidade base
        target = LevelManager.instance.path[pathIndex]; // Define o primeiro alvo
        
      
    
        if (spriteRenderer == null)
            spriteRenderer = GetComponent<SpriteRenderer>();

        originalColor = spriteRenderer.color;
    


}
    public int GetProgress()
    {
        return pathIndex;
    }

    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f) // Verifica se chegou ao alvo
        {
            Updatedestiny();
           
        }
    }

    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized; // Calcula a dire��o
        rb.velocity = direction * moveSpeed; // Move o inimigo
     
    
    }

    protected virtual void Updatedestiny()
    {
        pathIndex++; // Incrementa o �ndice do caminho
        if (pathIndex >= LevelManager.instance.path.Length)
        {
            LevelManager.instance.GameOver();
            HandleDeath();
        }

        else
        {
            target = LevelManager.instance.path[pathIndex]; // Atualiza o alvo para o pr�ximo ponto
        }

    }


    public void UpdateSpeed(float newSpeed) // Atualiza a velocidade do inimigo
    {
        moveSpeed = newSpeed;
    }

    public void ResetSpeed() // Reseta a velocidade do inimigo
    {
        moveSpeed = baseSpeed;
    }

    public void HandleDeath()
    {
        EnemySpawner.onEnemyDestroy.Invoke();
        Destroy(gameObject);
    }
    public void FlashOnHit()
    {
        StopAllCoroutines(); // interrompe piscadas anteriores
        StartCoroutine(FlashCoroutine());
    }

    private IEnumerator FlashCoroutine()
    {
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(flashDuration);
        spriteRenderer.color = originalColor;
    }



}
