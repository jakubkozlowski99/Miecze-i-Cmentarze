using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collidable : MonoBehaviourExtension
{
    public int hitsSize = 10;

    protected ContactFilter2D filter;
    protected BoxCollider2D boxCollider;
    protected Collider2D[] hits;

    protected override void Start()
    {
        base.Start();
        boxCollider = GetComponent<BoxCollider2D>();
        hits = new Collider2D[hitsSize];
}

    protected virtual void Update()
    {
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }

            OnCollide(hits[i]);

            hits[i] = null;
        }
    }

    public bool IsPlayerNearby()
    {
        boxCollider.OverlapCollider(filter, hits);

        for (int i = 0; i < hits.Length; i++)
        {
            if (hits[i] == null)
            {
                continue;
            }

            if (hits[i].name == "Player")
            {
                hits[i] = null;
                return true;
            }
            hits[i] = null;
        }
        return false;
    }

    protected virtual void OnCollide(Collider2D coll)
    {
    }
}
