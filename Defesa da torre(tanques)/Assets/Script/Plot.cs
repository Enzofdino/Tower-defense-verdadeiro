using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Plot : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private SpriteRenderer sr;
    [SerializeField] private Color hoverColor;

    private GameObject tower;
    private Color startColor;

    
    private void Start()
    {
        startColor = sr.color;
        
    }
    private void OnMouseDown()
    {
        if (tower != null) return;
        Tower towertobuild = BuildManager.Instance.GetselectedTower();
        tower = Instantiate(towertobuild.prefab, transform.position, Quaternion.identity);
    }
    private void OnMouseEnter()
    {
        sr.color = hoverColor;
    }

    private void OnMouseExit()
    {
        sr.color = startColor;
    }
   
}
