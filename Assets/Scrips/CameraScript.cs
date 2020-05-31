using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour
{
    public BoxCollider2D bounds; //Kollisionsbox för kameran som den ska stanna inuti.
    public GameObject player;
    private float verticalHalfSize;
    private float horizontalHalfSize;

    //Tar fram kamerans storlek delat på två i bredd och höjd så att det kan användas för att checka om kameran är out of bounds.
    void Start()
    {
        verticalHalfSize = Camera.main.orthographicSize;
        horizontalHalfSize = verticalHalfSize * Screen.width / Screen.height;
    }

    //Checkar om kameran är out of bounds och gör så att den stannar kvar inuti kollisionsboxen "bounds".
    void Update()
    {
        transform.position = player.transform.position;
        Vector2 newPosition = transform.position;

        //Checkar om kamerans vänstra sida är out of bounds och flyttar kameran tillbaka om det skulle vara så.
        if (transform.position.x - horizontalHalfSize < bounds.bounds.min.x)
        {
            newPosition.x = bounds.bounds.min.x + horizontalHalfSize;
        }

        //Checkar om kamerans högra sida är out of bounds och flyttar kameran tillbaka om det skulle vara så.
        if (transform.position.x + horizontalHalfSize > bounds.bounds.max.x)
        {
            newPosition.x = bounds.bounds.max.x - horizontalHalfSize;
        }

        //Checkar om kamerans top är out of bounds och flyttar kameran tillbaka om det skulle vara så.
        if (transform.position.y + verticalHalfSize > bounds.bounds.max.y)
        {
            newPosition.y = bounds.bounds.max.y - verticalHalfSize;
        }

        //Checkar om kamerans botten är out of bounds och flyttar kameran tillbaka om det skulle vara så.
        if (transform.position.y - verticalHalfSize < bounds.bounds.min.y)
        {
            newPosition.y = bounds.bounds.min.y + verticalHalfSize;
        }

        transform.position = new Vector3(newPosition.x, newPosition.y, -10);
    }
}
