using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mover : Fighter
{
    protected BoxCollider2D boxCollider;
    protected Vector3 moveDelta;
    protected RaycastHit2D hit;
    protected float ySpeed = 1.5f;
    protected float xSpeed = 2.0f;

    protected virtual void Start()
    {
        boxCollider = GetComponent<BoxCollider2D>();
    }

    protected virtual void UpdateMotor(Vector3 input, float ySpeed, float xSpeed)
    {
        moveDelta = new Vector3(input.x * xSpeed, input.y * ySpeed, 0);

        if (moveDelta.x > 0)
            if (transform.localScale.x > 0) transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
            else transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
        else if (moveDelta.x < 0)
        {
            if (transform.localScale.x > 0) transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, 1);
            else transform.localScale = new Vector3(transform.localScale.x, transform.localScale.y, 1);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(0, moveDelta.y), Mathf.Abs(moveDelta.y * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking", "Player", "Enemy"));

        if (hit.collider == null)
        {
            transform.Translate(0, moveDelta.y * Time.deltaTime, 0);
        }

        hit = Physics2D.BoxCast(transform.position, boxCollider.size, 0, new Vector2(moveDelta.x, 0), Mathf.Abs(moveDelta.x * Time.deltaTime), LayerMask.GetMask("Actor", "Blocking", "Player", "Enemy"));

        if (hit.collider == null)
        {
            transform.Translate(moveDelta.x * Time.deltaTime, 0, 0);
        }
    }
}
