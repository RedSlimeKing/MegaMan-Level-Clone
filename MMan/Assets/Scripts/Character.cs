using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Rigidbody2D))]
public class Character : MonoBehaviour
{
    // Makes a private reference to Rigidbody2D Component
    Rigidbody2D rb;

    // Makes a public reference to Rigidbody2D Component\
    public Rigidbody2D rb2;

    // Variable for player
    public float speed;
    public int health;
    public int score;
    private bool doubleJump;
    private bool HavedoubleJump;

    // Variable to control jumpForce of GameObject
    public float jumpForce = 10.0f;
    public float BoostTimer = 5.0f;

    //healthbar var
    public RectTransform healthBar;
    int scaleFactor;
    // Variables to tell if Character should jump or not
    public bool isGrounded;
    public LayerMask isGroundLayer;
    public Transform groundCheck;
    public float groundCheckRadius;

    // Used to switch and play animations
    Animator anim;

    public Text scoreText;
    // Handles Projectile spawning
    public Transform projectileSpawnPoint;  // Where to create projectile
    public Projectile projectile;           // What projectile to create
    public float projectileForce;           // How fast projectile should move
    public float fireRate;
    float timeSinceLastFire = 0;

    // Used for flipping Character
    public bool isFacingLeft;
    public bool isAlive;

    //used for controlling ladder
    public bool onLadder;
    public float climbSpeed;
    public float climbVelocity;
    private float gravityStore;

    //used for audio
    public AudioSource audioSource;
    public AudioClip jumpFX;
    public AudioClip shootFX;
    public AudioClip deathFX;
    public AudioClip hitFX;
    public AudioClip powerUpFX;

    // Use this for initialization
    void Start()
    {
        // Used to get and save a reference to the Rigidbody2D Component
        rb = GetComponent<Rigidbody2D>();

        gravityStore = rb.gravityScale;
        climbSpeed = 5f;

        scoreText = GameObject.Find("Score").GetComponent<Text>();
        scoreText.text = "Score: " + score.ToString();

        // Check if 'fireRate' was assigned a value
        if (fireRate == 0)
            fireRate = 0.2f;
        if (speed == 0)
            speed = 8.0f;
        
        if (health == 0)
        {
            isAlive = true;
            health = 100;
        }
        // Check if groundCheckRadius variable was set in the inspector
        if (groundCheckRadius == 0)
            groundCheckRadius = 0.1f;
        
        // Check if groundCheck variable was set in the inspector
        if (!groundCheck)
            Debug.LogError("Ground Check not set in Inspector.");

        // Used to get and save a reference to the Animator Component
        anim = GetComponent<Animator>();

        // Check if Animator was added
        if (!anim)
            Debug.Log("Animator not found.");

        // Check if projectileSpawnPoint variable was set in the inspector
        if (!projectileSpawnPoint)
            Debug.LogError("Projectile Spawn Point not set in Inspector.");
        

        // Check if projectile variable was set in the inspector
        if (!projectile)
            Debug.LogError("Projectile not set in Inspector.");
        
        // Check if projectileForce variable was set in the inspector
        if (projectileForce == 0)
        {
            projectileForce = 15.0f;
        }
        audioSource.GetComponent<AudioSource>();
        //healthbar code
        scaleFactor = (int)healthBar.sizeDelta.y / health;
    }

    // Update is called once per frame
    void Update()
    {
        if (!isAlive)
        {
            GameManager.Instance.GameOver();
            return;
        }
            // - Gives -1, 0, +1
            float moveValue = Input.GetAxisRaw("Horizontal");
            scoreText.text = "Score: " + score.ToString();                                                  //<------------------------------------------
            // Check if Character is touching anything labeled as Ground/Platform/Jumpable
            isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, isGroundLayer);

            // Check if Jump was pressed (aka Space)
            if (isGrounded)
            {
                anim.SetBool("Jumped",false);
                doubleJump = false;
            }
            if (Input.GetButtonDown("Jump") && isGrounded)
            {
                playSound(jumpFX);
                anim.SetBool("Jumped",true);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);                           // Applies a force in UP direction
            }
            if (Input.GetButtonDown("Jump") && !doubleJump && !isGrounded && HavedoubleJump)
            {
                playSound(jumpFX);
                anim.SetBool("Jumped",true);
                rb.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
                doubleJump = true;
            }
        if (!isGrounded && Input.GetButtonDown("Fire1"))
        {
            playSound(jumpFX);
            playSound(shootFX);
            anim.SetTrigger("Jump/Shoot");
        }
        // Check if "Fire1" was pressed
        if (Input.GetButtonDown("Fire1") && !(Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 0)) { 
            if (Time.time > timeSinceLastFire + fireRate)
            {
                    playSound(shootFX);
                    anim.SetTrigger("fireClicked");
                    fireProjectile();                   // Call function to create projectile GameObject
                    // Timestamp current time when projectile was fired
                    timeSinceLastFire = Time.time;
             }
         }

            // Make player move left or right based off moveValue
            rb.velocity = new Vector2(moveValue * speed, rb.velocity.y);

            anim.SetFloat("Speed",Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x));

            if (Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 0 && Input.GetButtonDown("Jump"))
                anim.SetTrigger("Walk/Jumped");

            if (Input.GetButtonDown("Fire1") && Mathf.Abs(GetComponent<Rigidbody2D>().velocity.x) > 0)
            {
                playSound(shootFX);
                anim.SetTrigger("Walk/Shoot");
                fireProjectile();
            }
            
            //Should Character flip
            if ((moveValue < 0 && !isFacingLeft) || (moveValue > 0 && isFacingLeft))
                flip();

            if (health == 0 || !isAlive)
            {
                anim.SetBool("Dead",true);
                isAlive = false;
            }
        if (onLadder)
        {
            anim.SetBool("OnLadderA", true);
            rb.gravityScale = 0f;
            climbVelocity = climbSpeed * Input.GetAxisRaw("Vertical");
            rb.velocity = new Vector2(rb.velocity.x,climbVelocity);

        }
        if (!onLadder)
        {
            anim.SetBool("OnLadderA", false);
            rb.gravityScale = gravityStore;
        }

        healthBar.sizeDelta = new Vector2(healthBar.sizeDelta.x, health * 1);
    }
    
    void OnTriggerEnter2D(Collider2D c)
    {
        if (c.gameObject.tag == "Win")
        {
            Debug.Log("Winner ");
            GameManager.Instance.Credits();
        }
        if (c.gameObject.tag == "Health")
        {
            playSound(powerUpFX);
            anim.SetTrigger("PowerUp");
            if (health >= 81)
                health = 100;
            else
                health += 20;
            Destroy(c.gameObject);
        }
        if (c.gameObject.tag == "Collectible")
        {
            score += 10;
            // - Can just call Destroy(c.gameObject);
            Destroy(c.gameObject, 0);
        }
    }
    public IEnumerator StopPowerUp()
    {
        // Waits for "BoostTimer" seconds
        yield return new WaitForSeconds(BoostTimer);
        HavedoubleJump = false;
        
    }
    
    void OnCollisionEnter2D(Collision2D c)
    {
        if (c.gameObject.tag == "PowerUp")
        {
            playSound(powerUpFX);
            HavedoubleJump = true;
            anim.SetTrigger("PowerUp");
            StartCoroutine(StopPowerUp());
            Destroy(c.gameObject);
        }
        
        if (c.gameObject.tag == "ProjectileEnemy")
        {
            playSound(hitFX);
            health = health - 10;
            anim.SetTrigger("Hit");
            Destroy(c.gameObject, 0);
        }
        if (c.gameObject.tag == "Enemy")
        {
            playSound(hitFX);
            anim.SetTrigger("Hit");
            health -= 10;
        }
        if (c.gameObject.tag == "Killzone")
        {
            playSound(deathFX);
            Debug.Log("Fall throught world");
            isAlive = false;
            anim.SetBool("Dead", true);
        }
    }

    // Function to be used when a projectile needs to be fired
    void fireProjectile()
    {
        // Create the GameObject (Projectile) at projectileSpawnPoint
        Projectile temp = Instantiate(projectile,
            projectileSpawnPoint.position,
            projectileSpawnPoint.rotation) as Projectile;

        // Check direction Character is facing and fire projectile in that direction
        if (!isFacingLeft)
            temp.GetComponent<Rigidbody2D>().AddForce(Vector2.right * projectileForce, ForceMode2D.Impulse);
        else
            temp.GetComponent<Rigidbody2D>().AddForce(Vector2.left * projectileForce, ForceMode2D.Impulse);
        
    }

    // Function used to flip Character
    void flip()
    {
        isFacingLeft = !isFacingLeft;                           // Toggle variable
        Vector3 scaleFactor = transform.localScale;
        scaleFactor.x *= -1;
        transform.localScale = scaleFactor;
    }
    // Function Used for Playing single sound
    void playSound(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = 1.0f;
        audioSource.Play();
    }
    //Used for loading and saving
}
