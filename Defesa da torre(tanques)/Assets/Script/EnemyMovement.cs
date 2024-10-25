using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class EnemyMovement : MonoBehaviour 

{
    [SerializeField] private Rigidbody2D rb;    

    [SerializeField] private float moveSpeed = 2f;    

    private Transform target;    

    private int pathIndex = 0;   

    private float baseSpeed;    

   
    private void Start()
    {
        baseSpeed = moveSpeed; 
        target = LevelManager.instance.path[pathIndex];
    }

  
    private void Update()
    {
        if (Vector2.Distance(target.position, transform.position) <= 0.1f)         
        {
            pathIndex++; 

            if (pathIndex == LevelManager.instance.path.Length)            

            {
                EnemySpawner.onEnemyDestroy.Invoke();
                Destroy(gameObject); 
                return;
            }
            else
            {
                target = LevelManager.instance.path[pathIndex];
            }
        }
    }
    private void FixedUpdate()   

    {
        Vector2 direction = (target.position - transform.position).normalized;         

        rb.velocity = direction * moveSpeed;
    }
    public void UpdateSpeed(float newSpeed)   

    {
        moveSpeed = newSpeed;
    }
    public void ResetSpeed()     

    {
        moveSpeed = baseSpeed; 
    }
}
