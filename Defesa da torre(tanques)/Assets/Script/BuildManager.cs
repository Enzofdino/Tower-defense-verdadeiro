using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildManager : MonoBehaviour
{
    public static BuildManager Instance;

    [Header("References")]
    [SerializeField] private GameObject[] towerPrefabs;

    private int selectedTower = 0;


    private void Awake()
    {
        Instance = this;
    }

    public GameObject GetselectedTower()
    {
        return towerPrefabs[selectedTower];
    }
}
