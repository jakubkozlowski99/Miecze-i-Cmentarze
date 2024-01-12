using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditorInternal;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;

    public bool dead;
    public float timer;
    public int childCount;
    public float respawnCooldown = 5;

    public bool flipped = false; //false = right, true = left

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
            if (flipped) transform.GetChild(0).localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
            dead = false;
            timer = 0;
        }
    }

    public void SaveSpawner()
    {
        var spawner = Array.Find(SaveManager.instance.tempSpawners.ToArray(), spawner => spawner.spawnerName == name);

        if (spawner != null) SaveManager.instance.tempSpawners.Remove(spawner);

        SaveManager.instance.tempSpawners.Add(new SpawnerData(this));
    }

    public void LoadSpawner()
    {
        bool shouldLoad = Array.Exists(SaveManager.instance.tempSpawners.ToArray(), spawner => spawner.spawnerName == name);

        if (shouldLoad)
        {
            SpawnerData spawner = Array.Find(SaveManager.instance.tempSpawners.ToArray(), spawner => spawner.spawnerName == name);

            dead = spawner.dead;

            if (!dead)
            {
                Instantiate(enemy, new Vector2(spawner.posX, spawner.posY), new Quaternion(0, 0, 0, 0), transform);
                timer = 0;

                transform.GetChild(0).localScale = new Vector3(spawner.scaleX, enemy.transform.localScale.y, enemy.transform.localScale.z);
            }

            else timer = spawner.timer + GameManager.instance.gameTimer - spawner.lastTimerState;
        }
        else
        {
            Instantiate(enemy, transform.position, transform.rotation, transform);
            if (flipped) transform.GetChild(0).localScale = new Vector3(-enemy.transform.localScale.x, enemy.transform.localScale.y, enemy.transform.localScale.z);
            dead = false;
        }
    }

}
