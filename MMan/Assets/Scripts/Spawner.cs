using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour {

    public GameObject EnemyS;
    public GameObject target;
    public Transform SpawnPoint;
    Rigidbody2D rb;                      // Rigidbody to control enemy movement with physics
    
    public float fireRate;
    float timeSinceLastFire = 0;
    // Used for flipping Enemy
    public bool isFacingLeft;
    // Used to keep track of Enemy health
    public int health;
    // Used to control animations of Enemy (walk, death)
    Animator anim;
    
    public AudioSource audioSource;
    public AudioClip deathFX;
    public AudioClip hitFX;
    public AudioClip SpawnsFX;



    // Use this for initialization
    void Start () {
        health = 3;
        fireRate = 6.0f;
        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            rb = gameObject.AddComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
        if (!anim)          // Check if Animator was added
            Debug.Log("Animator not found.");
        target = GameObject.FindGameObjectWithTag("Player");
        // Check if 'Player' was found
        if (!target)
            Debug.Log("Player not found.");
    }
	
	// Update is called once per frame
	void Update () {
        if (target.transform.position.x < transform.position.x && !isFacingLeft)
            flip();
        else if (target.transform.position.x > transform.position.x && isFacingLeft)
            flip();
    }
    void CreateEnemy()
    {
      Instantiate(EnemyS, SpawnPoint.position, Quaternion.identity);
    }
    void OnTriggerStay2D(Collider2D c)
    {
        if (c.gameObject.tag == "Player")
        {
            anim.SetTrigger("Spawn");
            // Check if enough time has passed to fire another one
            if (Time.time > timeSinceLastFire + fireRate)
            {
                playSound(SpawnsFX);
                CreateEnemy();
                // Timestamp current time when projectile was fired
                timeSinceLastFire = Time.time;
            }
        }
    }
    void OnCollisionEnter2D(Collision2D c)
    {
        // Check if a GameObject tagged as 'Projectile' hit the Enemy
        if (c.gameObject.tag == "Projectile")
        {
            health--;               // Decrement health
            anim.SetTrigger("Hit");
            playSound(hitFX);

            if (health <= 0)
                die();

            return;
        }
    }
    // Function used to flip Sprite
    void flip()
    {
        isFacingLeft = !isFacingLeft;                       // Toggle variable
        Vector3 scaleFactor = transform.localScale;         // Keep a copy of "localScale" to apply scale flip
        scaleFactor.x *= -1; 
        transform.localScale = scaleFactor;
    } 
    void die()                          // Used with Animation Event in Animation of Sprite
    {
        playSound(deathFX);
        anim.SetBool("Dead", true);
        Destroy(gameObject, 1);            // Removes Sprite from Scene after animation is completed
    }
    void playSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 1.0f;
        audioSource.Play();
    }
}
