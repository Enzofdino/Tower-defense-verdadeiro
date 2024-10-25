
using System;
using UnityEngine;

[Serializable]
public class Tower

{
    public string name;    

    public GameObject prefab;

    public Tower(string _name, int _cost, GameObject _prefab)

    {
        name = _name;

        prefab = _prefab;

    }
}