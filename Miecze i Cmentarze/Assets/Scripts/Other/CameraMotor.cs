using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMotor : MonoBehaviour
{
    public static CameraMotor instance;
    public Transform lookAt;
    public float boundX = 0.3f;
    public float boundY = 0.1f;

    public void Start()
    {
        if (instance != null)
        {
            Destroy(gameObject);
            return;
        }
        instance = this;
        Player player = FindObjectOfType<Player>();
        if (player != null)
        {
            lookAt = player.transform;
            DontDestroyOnLoad(gameObject);
        }
    }

    public void LateUpdate()
    {
        if(lookAt != null)
        {
            Vector3 delta = Vector3.zero;

            float deltaX = 0;

            if (!PauseMenu.instance.gameIsPaused) deltaX = lookAt.position.x - transform.position.x;

            if (deltaX > boundX || deltaX < -boundX)
            {
                if (transform.position.x < lookAt.position.x)
                {
                    delta.x = deltaX - boundX;
                }
                else
                {
                    delta.x = deltaX + boundX;
                }
            }

            float deltaY = 0;

            if (!PauseMenu.instance.gameIsPaused) deltaY = lookAt.position.y - transform.position.y;

            if (deltaY > boundY || deltaY < -boundY)
            {
                if (transform.position.y < lookAt.position.y)
                {
                    delta.y = deltaY - boundY;
                }
                else
                {
                    delta.y = deltaY + boundY;
                }
            }

            transform.position += new Vector3(delta.x, delta.y, 0);
        }
    }
}
