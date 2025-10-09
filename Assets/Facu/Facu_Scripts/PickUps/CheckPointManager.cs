using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CheckPointManager : MonoBehaviour
{
    private List<string> _deletedPickUps = new List<string>();
    private List<string> _deletedEnemies = new List<string>();

    private void Awake()
    { 
          SceneManager.sceneLoaded += UpdateCheckPoint;
    }     
    public void ResetAll()
    {
        _deletedPickUps.Clear();
        _deletedEnemies.Clear();
    }

    public void UpdateCheckPoint(Scene SceneManager , LoadSceneMode mode)
    {
        foreach (string pickUp in _deletedPickUps)
        {
            GameObject.Find(pickUp).SetActive(false);
        }
        foreach (string enemy in _deletedEnemies)
        {
            GameObject.Find(enemy).SetActive(false);
        }

    }


   public void DeletePickUp(string pickUp)
    {
         _deletedPickUps.Add(pickUp); 
    }
    public void DeleteEnemy(string enemy)
    {
        _deletedEnemies.Add(enemy);
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= UpdateCheckPoint;
    }
}



