using System.Collections.Generic;
using UnityEngine;

public class RoomSystem : MonoBehaviour
{
    public bool isPlayerInside = false;
    public bool encounterActive = false;
    public EnemyAI[] enemies;
    public bool[] areEnemiesActive;
    public GameObject[] doors;
    
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        DeactivateDoors();
        areEnemiesActive = new bool[enemies.Length];
    }

    // Update is called once per frame
    void Update()
    {
        if (!encounterActive)
        {
            return;
        }
        
        if (isPlayerInside)
        {
            CheckEnemyStatus();
        }
    }

    public void CheckEnemyStatus()
    {
        int i = 0;
        int n = enemies.Length;
        
        foreach (EnemyAI enemy in enemies)
        {
            if (enemy.gameObject.activeInHierarchy)
            {
                areEnemiesActive[i] = true;
            }
            else
            {
                areEnemiesActive[i] = false;
                n--;
            }

            i++;
        }

        if (n<=0)
        {
            encounterActive = false;
            DeactivateDoors();
        }
        
    }

    public void ActivateDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(true);
        }
    }

    public void DeactivateDoors()
    {
        foreach (GameObject door in doors)
        {
            door.SetActive(false);
        }
    }

    public void OnTriggerEnter(Collider col)
    {
        if (enemies == null)
        {
            return;
        }
        
        if (col.gameObject.CompareTag("Player"))
        {
            isPlayerInside = true;
            encounterActive = true;
            ActivateDoors();
        }
        
    }

    public void OnTriggerExit(Collider col)
    {
        if (col.gameObject.CompareTag("Player"))
        {
            isPlayerInside = false;
            encounterActive = false;
            DeactivateDoors(); 
        }
        
    }
}
