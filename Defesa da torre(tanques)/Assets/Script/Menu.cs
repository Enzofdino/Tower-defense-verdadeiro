using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [Header("References")]
    [SerializeField] TextMeshProUGUI currencyUI;
    [SerializeField] Animator animator;

    private bool ismenuOpen = true;

    public void ToggleMenu()
    {
        ismenuOpen = !ismenuOpen;
        animator.SetBool("MenuOpen" , ismenuOpen);
    }
    private void OnGUI()
    {
        currencyUI.text = LevelManager.instance.currency.ToString();
    }
    public void SetTower()
    {

    }
}
