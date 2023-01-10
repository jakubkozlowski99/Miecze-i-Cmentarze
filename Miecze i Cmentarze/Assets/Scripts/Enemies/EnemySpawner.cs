using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public bool dead;
    public float timer;
    public int childCount;
    public float respawnCooldown = 5;

    protected virtual void Start()
    {
        LoadSpawner();
        SaveSpawner();
    }

    protected virtual void Update()
    {
        if (dead && !PauseMenu.instance.gameIsPaused)
        {
            timer += Time.deltaTime;
        }
        childCount = transform.childCount;
        if (childCount == 0)
        {
            dead = true;
        }
        if (timer >= respawnCooldown) 
        {
            Instantiate(enemy, transform.position, transform.rotation, transform);
            dead = false;
            timer = 0;
        }
    }

    public void SaveSpawner()
    {
        foreach (SpawnerData spawner in SaveManager.instance.tempSpawners)
        {
            if (spawner.spawnerName == name)
            {
                SaveManager.instance.tempSpawners.Remove(spawner);
                break;
            }
        }
        SaveManager.instance.tempSpawners.Add(new SpawnerData(this));
    }

    public void LoadSpawner()
    {
        foreach (SpawnerData spawner in SaveManager.instance.tempSpawners)
        {
            if (spawner.spawnerName == name)
            {
                dead = spawner.dead;
                if (dead == false)
                {
                    Instantiate(enemy, transform.position, transform.rotation, transform);
                    timer = 0;
                    return;
                }
                timer = spawner.timer + GameManager.instance.gameTimer - spawner.lastTimerState;
                return;
            }
        }
        Instantiate(enemy, transform.position, transform.rotation, transform);
        dead = false;
    }
}
