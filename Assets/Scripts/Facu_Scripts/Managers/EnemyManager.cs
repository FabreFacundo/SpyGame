using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class EnemyManager : MonoBehaviour
{
    private Enemy[] _enemies;


    public Enemy[] Enemies { get { return _enemies; } }


    void Start()
    {
        FindEnemies();
        GameManager.instance.EnemyManager = this;
    }

    public void FindEnemies()
    {
        _enemies = FindObjectsByType<Enemy>(FindObjectsSortMode.InstanceID);
    }

}
