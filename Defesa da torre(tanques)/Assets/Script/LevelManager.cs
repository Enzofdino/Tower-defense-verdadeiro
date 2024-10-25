using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour 

{
    public static LevelManager instance;    

    public Transform startPoint;    

    public Transform[] path;   

    private void Awake()   

    {
        instance = this; 
    }
}
