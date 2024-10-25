using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform startpoint;
    public Transform[] Caminho;

    public int currency;
    private void Awake()
    {
        instance = this;
    }
    public void Start()
    {

        currency = 100;
        
    }
    public void IncreaseCurrency(int amount)
    {
        currency += amount;
    }
    public bool spendCurrency(int amount)
    {
        if(amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("voce não possui dinheiro para comprar a torreta");
            return false;
        }
             
    }
}
