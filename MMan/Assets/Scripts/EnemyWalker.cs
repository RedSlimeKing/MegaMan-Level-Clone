using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyWalker : MonoBehaviour
{

    float speed;        // Speed the Enemy moves
    Rigidbody2D rb;     // Rigidbody to control enemy movement with physics

    // Used for flipping Enemy
    public bool isFacingLeft;

    // Used to keep track of Enemy health
    public int health;

    // Used to control animations of Enemy (walk, death)
    Animator anim;

    public AudioSource audioSource;
    public AudioClip deathFX;
    public AudioClip hitFX;
    public AudioClip AttackFX;

    // Use this for initialization
    void Start()
    {


        // Check if 'speed' variable was set in the inspector
        if (speed == 0)
            speed = 2.0f;

        rb = GetComponent<Rigidbody2D>();
        if (!rb)
            rb = gameObject.AddComponent<Rigidbody2D>();

        // Check if 'health' variable was set in the inspector
        if (health <= 0)
            health = 5;

        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");
    }

    // Update is called once per frame
    void Update()
    {
        // Check direction Sprite should be moving in
        if (!isFacingLeft)
            rb.velocity = new Vector2(speed, rb.velocity.y);                // Move Sprite right
        else
            rb.velocity = new Vector2(-speed, rb.velocity.y);        // Move Sprite left
    }

    // Check if Sprite hits another Collider 
    // - At least one of the GameObjects need a Rigidbody2D attached
    void OnCollisionEnter2D(Collision2D c)
    {
        // Check if a GameObject tagged as 'Projectile' hit the Enemy
        if (c.gameObject.tag == "Projectile")
        {
            // Decrement health
            health--;
            anim.SetTrigger("Hit");
            playSound(hitFX);
            // No Animation Event
            if (health <= 0)
                
                die();
            
            return;
        }
        if (c.gameObject.tag == "Player")
        {
            anim.SetTrigger("Attacking");
            playSound(AttackFX);
            return;
        }
        if (c.gameObject.tag == "Killzone")
        {
            playSound(deathFX);
            anim.SetBool("IsDead", true);
        }
        // Only flip if hitting something other than ground
        if (c.gameObject.tag != "Ground")
            flip();
        
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {
        // Check if Sprite hits something tagged as 'EnemyBumper'
        if (c.gameObject.tag == "EnemyBumper")
            flip();                         // Call flip function to flip Sprite
        
    }

    // Function used to flip Sprite
    void flip()
    {
        // Toggle variable
        isFacingLeft = !isFacingLeft;

        // Keep a copy of "localScale" to apply scale flip
        Vector3 scaleFactor = transform.localScale;

        // Change sign of 'x' to cause a flip
        scaleFactor.x *= -1; // or -scaleFactor.x;

        // Assign updated scale to variable to cause flip
        transform.localScale = scaleFactor;
    }
    // Used with Animation Event in Animation of Sprite
    void die()
    {
        playSound(deathFX);
        anim.SetBool("IsDead", true);
        Destroy(gameObject,0.5f);            // Removes Sprite from Scene after animation is completed
    }
    void playSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 1.0f;
        audioSource.Play();
    }
}
