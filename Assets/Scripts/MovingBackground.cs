using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingBackground : MonoBehaviour
{
    const float startNend = 20f;
    [SerializeField] int movingSpeed = 1; 

    private void Update()
    {
        movingNcheck();
    }

    private void movingNcheck()
    {
        transform.Translate(Vector2.down * movingSpeed * Time.deltaTime);

        if (transform.position.y <= -startNend)
        {
            transform.position = new Vector2(0, startNend);
        }
    }
}
