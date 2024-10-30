using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 

{
    public static LevelManager instance;    

    public Transform startPoint;    

    public Transform[] path;
    public int currency;

    private void Awake()   

    {
        instance = this; 
    }
    private void Start()
    {
        currency = 100;
    }

    public bool IncreaseCurrency(int amount)
    {
        amount = 50;
        if(amount <= currency)
        {
            currency +=amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item");
            return false;
        }
    }
    public bool SpendCurrency(int amount)
    {
        
        if (amount <= currency)
        {
            currency -= amount;
            return true;
        }
        else
        {
            Debug.Log("You do not have enough to purchase this item");
            return false;
        }
    }
}
