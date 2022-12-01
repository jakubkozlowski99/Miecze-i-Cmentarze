using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SortChange : MonoBehaviour
{
    private SpriteRenderer render;
    public float pivotCorrection = 0.1f;
    
    protected void Start()
    {
        render = GetComponent<SpriteRenderer>();
    }

    protected void Update()
    {
        if (GameManager.instance.player.transform.position.y + pivotCorrection > transform.position.y && render.sortingLayerName == "Decorations3")
        {
            render.sortingLayerName = "Decorations4";
        }
        else if (GameManager.instance.player.transform.position.y + pivotCorrection < transform.position.y && render.sortingLayerName == "Decorations4")
            render.sortingLayerName = "Decorations3";
    }
}
