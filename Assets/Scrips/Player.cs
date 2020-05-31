using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D rb;
    private bool grounded;
    public BoxCollider2D groundCheck; //Kollisionsbox som används för att checka om spelaren nuddar marken.
    public BoxCollider2D deathCheck; //Kollisionsbox för spelaren. Används för kollision med allt i banan.
    public int jumpSpeed;
    public int moveSpeed;

    //Denna metoden kollar om spelaren är död. Spelaren är död om man trillar under kartan eller nuddar några spikar. 
    public bool IsDead()
    {
        if (transform.position.y < -10) //Du trillar under kartan om spelarens y-värde är under -10.
        {
            return true;
        }

        Collider2D[] colliders = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("CanKillPlayer")); //Spikar har kollision på layern "CanKillPlayer".

        int length = deathCheck.OverlapCollider(filter, colliders);

        for (int i = 0; i < length; i++)
        {
            if (colliders[i].gameObject != gameObject) //Säkerhetscheck så att vi inte råkar kollidera med oss själva, behövs egentligen inte.
            {
                return true;
            }
        }

        return false;
    }

    //Den här metoden kollar om spelare har kommit till målet.
    public bool HasWon()
    {
        Collider2D[] colliders = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D();
        filter.SetLayerMask(LayerMask.GetMask("Goal")); //Målen har kollision på layern "Goal".

        int length = deathCheck.OverlapCollider(filter, colliders);

        for (int i = 0; i < length; i++)
        {
            if (colliders[i].gameObject != gameObject) //Säkerhetscheck så att vi inte råkar kollidera med oss själva, behövs egentligen inte.
            {
                return true;
            }
        }

        return false;
    }

    //Den checkar om spelaren har kolliderat med en bonus och skickar tillbaka bonusens GameObject.
    public GameObject CheckBonusCollision()
    {
        Collider2D[] colliders = new Collider2D[10];
        ContactFilter2D filter = new ContactFilter2D(); 
        filter.SetLayerMask(LayerMask.GetMask("Bonus")); //Bonusen har kollision på layern "Bonus".

        int length = deathCheck.OverlapCollider(filter, colliders);

        for (int i = 0; i < length; i++)
        {
            if (colliders[i].gameObject != gameObject) //Säkerhetscheck så att vi inte råkar kollidera med oss själva, behövs egentligen inte.
            {
                return colliders[i].gameObject;
            }
        }

        return null;
    }

    //Den här metoden kollar om spelaren nuddar marken och gör så att spelaren kan hoppa om den nuddar marken genom att sätta variablen grounded till true.
    private void CheckGroundCollision()
    {
        grounded = false;

        Collider2D[] colliders = new Collider2D[10];
        int length = groundCheck.OverlapCollider(new ContactFilter2D(), colliders); //Checkar kollision med alla objekt som spelaren kan kollidera med (alltså inte bara en layer).

        for (int i = 0; i < length; i++)
        {
            if (colliders[i].gameObject != gameObject) //Säkerhetscheck så att vi inte råkar kollidera med oss själva, behövs egentligen inte.
            {
                grounded = true;
                break;
            }
        }
    }

    //När spelaren skapas hämtar vi komponenten som hanterar spelarens fysik så att vi kan använda den senare i update.
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody2D>();
    }

    //Uppdaterar spelaren varje frame och checkar om den ska röra på sig eller hoppa.
    void Update()
    {
        if (!GameController.Paused) //Updatera bara spelaren om spelet inte är pausat.
        {
            CheckGroundCollision();

            float direction = Input.GetAxis("Horizontal"); //Vi kollar om spelaren vill röra sig åt höger eller vänster. Metoden ger ett värde mellan -1 och 1. När direction är negativt betyder det att man rör sig åt vänster, och positivt är åt höger.
            rb.velocity = new Vector2(direction * moveSpeed, rb.velocity.y); 

            bool jump = Input.GetButtonDown("Jump"); //Kollar om spelaren vill hoppa.
            if (jump && grounded) //Man kan bara hoppa om man nuddar marken.
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpSpeed);
            }
        }
    }
}
