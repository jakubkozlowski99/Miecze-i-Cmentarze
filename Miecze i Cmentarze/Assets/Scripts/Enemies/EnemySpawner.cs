using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    private string enemyName;

    public bool dead;
    protected float timer;
    public int childCount;
    public float respawnCooldown = 5;

    protected virtual void Start()
    {
        Instantiate(enemy, transform.position, transform.rotation, transform);
        dead = false;
        enemyName = enemy.name;
    }

    protected virtual void Update()
    {
        if (dead)
        {
            timer += Time.deltaTime;
        }
        childCount = transform.childCount;
        if (childCount == 0)
        {
            dead = true;
        }
        if(timer >= respawnCooldown)
        {
            Instantiate(enemy, transform.position,transform.rotation,transform);
            dead = false;
            timer = 0;
        }
    }

}
