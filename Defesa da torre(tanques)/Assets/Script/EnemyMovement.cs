using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Rigidbody2D rb;
    [Header("Attibutes")]
    [SerializeField] private float movespeed = 2f;

    private Transform target;
    private int pathindex = 0;
    private void Start()
    {
        target = LevelManager.instance.Caminho[pathindex];
    }
    private void Update()
    {
        if(Vector2.Distance(target.position, transform.position) <= 0.1f)
        {
            pathindex++;

           
            if (pathindex == LevelManager.instance.Caminho.Length)
            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject);
                return;

            }
            else
            {

                target = LevelManager.instance.Caminho[pathindex];
            }
        }

    }
    private void FixedUpdate()
    {
        Vector2 direction = (target.position - transform.position).normalized;
        rb.velocity = direction * movespeed;
    }
}
