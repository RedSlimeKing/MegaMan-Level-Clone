using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{

    // Time for Projectile to live
    public float lifeTime;

    // Use this for initialization
    void Start()
    {

        // Check if lifeTime was assigned a value
        if (lifeTime == 0)
        {
            // Default to a time of 1.0 seconds
            lifeTime = 4.0f;
        }

        // Destroy Projectile after "lifeTime" seconds
        // - Used if the Projectile isnt destroyed on Collision
        Destroy(gameObject, lifeTime);
    }

    void OnCollisionEnter2D(Collision2D c)
    {
        // Ignore collisions between projectile and other projectiles / player and projectile
        if (c.gameObject.tag != "Player" && c.gameObject.tag != "Projectile")
        {
            // Destroy projectile on Collision
            Destroy(gameObject);
        }
    }
}
