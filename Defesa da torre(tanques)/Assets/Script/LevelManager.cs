using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelManager : MonoBehaviour
{
    public static LevelManager instance;

    public Transform startpoint;
    public Transform[] Caminho;
    private void Awake()
    {
        instance = this;
    }
}
