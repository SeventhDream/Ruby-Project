using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the movement and other variables relating to the Ruby GameObject.
// Author: Reuel Terezakis

public class RubyController : MonoBehaviour
{
    public Quest quest;
    public bool isTalking = false;
    private NonPlayerCharacter nonPlayerCharacter;

    private EnemyController enemyController;
    // One Shot audio.
    public AudioSource audioSource;
    public AudioSource audioSource2;
    public AudioClip playerHit;
    public AudioClip throwCog;
    public AudioClip footsteps;
    public AudioClip attack;
    bool Play = false;

    // Creates a class variable to store the Rigidbody and Animator respectively and access them from within the script.
    Rigidbody2D rigidbody2D;
    Animator animator;

    // Stores the look direction.
    Vector2 lookDirection = new Vector2(0, 1);

    // Allows to assign a GameObject with the ParticleSystem component type to this setting (no need to GetComponent later).
    public ParticleSystem hitParticlePrefab;
    public ParticleSystem healthParticlePrefab;
    public ParticleSystem ammoParticlePrefab;

    // Projectile variable.
    public GameObject projectilePrefab;

    // PC Ammunition
    public int maxAmmo = 10;
    public int ammo { get { return currentAmmo; } } // Stores the currentAmmo value in a public property.
    int currentAmmo;

    // PC health.
    public int maxHealth = 5;
    public int health { get { return currentHealth; } } // Stores the currentHealth value in a public property.
    int currentHealth;
    
    // PC speed.
    public float speed;

    // PC invicibility cooldown.
    public float timeInvincible = 1.0f; // Sets the amount of time PC is invicible after taking damage.
    bool isInvincible; // Boolean variable.
    float invincibleTimer; // Tracks reamining invincibility time.

    // Start is called before the first frame update.
    void Start()
    {
        // Initialises PC's ammo to be at maximum.
        currentAmmo = 0;

        // Initialises PC's health to be at maximum.
        currentHealth = maxHealth;

        // Gets the Rigidbody2D and animator and audio source attached to the GameObject.
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();

        // No footsteps.
        audioSource2.Stop();
    }

    // Update is called once per frame.
    void Update()
    {
            // Stores the value of both the horizontal and vertical axis inputs respectively (-1 to 1).
            float horizontal = Input.GetAxis("Horizontal");
            float vertical = Input.GetAxis("Vertical");

            // Stores horizontal and vertical input amount.
            Vector2 move = new Vector2(horizontal, vertical);

        if (isTalking == true)
        {
            move.x = 0.0f;
            move.y = 0.0f;
        }

        // Check to see if move.x or move.y is NOT equal to zero.
        if (!Mathf.Approximately(move.x, 0.0f) || !Mathf.Approximately(move.y, 0.0f)) // Approximately takes imprecision into account when comparing float numbers.
        {
                // Set look direction to PC's move vector.
                lookDirection.Set(move.x, move.y); // Could also have written 'lookDirection = move;'
                lookDirection.Normalize(); // Make lookDirection's length equal one (only direction is important).
                if (Play == false)
                {
                    audioSource2.Play();
                    Play = true;

                }
        }
            else if (Play == true)
            {
                audioSource2.Stop();
                Play = false;
            }

            // Send relevant data to the Animator.
            animator.SetFloat("Look X", lookDirection.x);
            animator.SetFloat("Look Y", lookDirection.y);
            animator.SetFloat("Speed", move.magnitude);

            // Stores the x- and y- coordinates of the Rigidbody in a Vector2-type variable.
            Vector2 position = rigidbody2D.position;

            // Modifies the stored x- and y- coordinates (depends on frame rendering).
            position = position + move * speed * Time.deltaTime;

            // Updates PC's Rigidbody x- and y- cooordinates in Unity.
            rigidbody2D.MovePosition(position);
        
        // Counts down invincibility timer.
        if (isInvincible)
        {
            invincibleTimer -= Time.deltaTime;
            if (invincibleTimer < 0)
                isInvincible = false;
        }

        //Initialises projectile launch upon key press.
        if (Input.GetKeyDown(KeyCode.C) && isTalking == false && UIHealthBar.instance.canShoot == true)
        {
            Launch();
        }

        // Interact with nearby NPCs with key press.
            // Stores the collider intersected by the raycast.
            RaycastHit2D hit = Physics2D.Raycast(rigidbody2D.position + Vector2.up * 0.2f, lookDirection, 1.5f, LayerMask.GetMask("NPC"));

        // Check if raycast made a collision.
            if (hit.collider != null)
            {
                NonPlayerCharacter character = hit.collider.GetComponent<NonPlayerCharacter>(); // Access behaviour script on collided GameObject
                if (character != null)
                {
                character.DisplayDialog();
                }
                if (Input.GetKeyDown(KeyCode.X))
                {
                    isTalking = true;
                    character.StartConversation();
                    UIHealthBar.instance.Ammo.SetActive(true);
                    UIHealthBar.instance.AmmoCounter.SetActive(true);
                    UIHealthBar.instance.canShoot = true;


                }
            }

        

        if (currentHealth < 1)
        {
            Destroy(gameObject);
        }
    }


    // Declares a new function to change PC's health.
   public void ChangeHealth(int amount)
   {
        // Checks for invincibility when taking damage.
        if (amount < 0)
        {
            if (isInvincible)
            {
                return; // Exits function.
            }

            // If damage is taken, reset invincibility timer.
            isInvincible = true;
            invincibleTimer = timeInvincible;

            // Triggers the 'Hit' animation.
            animator.SetTrigger("Hit");
            ParticleSystem hitParticleSystem = Instantiate(hitParticlePrefab, rigidbody2D.position, Quaternion.identity); // Creates 'hit' particle effect.
            PlaySound(playerHit); // Plays audio clip using PC as source.

            enemyController = GameObject.FindObjectOfType<EnemyController>(); // Access behaviour script on collided GameObject
            if (enemyController != null)
            {
                PlaySound(attack);
            }
        }

        // Creates 'health' particles when healed
        if (amount > 0)
        {
            ParticleSystem healthParticleSystem = Instantiate(healthParticlePrefab, rigidbody2D.position, Quaternion.identity);
        }
        
        // Ensures health is always between 0 and 'maxHealth'
        currentHealth = Mathf.Clamp(currentHealth + amount, 0, maxHealth);

        // Updates the health bar.
        UIHealthBar.instance.SetValue(currentHealth / (float)maxHealth); // Give the ratio of our currentHealth over our maxHealth to our UIHealthBar SetValue function.
    }

    // Function to change PC's ammo count
    public void ChangeAmmo(int amount)
    {
        if (amount > 0)
        {
            ParticleSystem ammoParticleSystem = Instantiate(ammoParticlePrefab, rigidbody2D.position, Quaternion.identity);
        }

        currentAmmo = Mathf.Clamp(currentAmmo + amount, 0, maxAmmo);
        UIHealthBar.instance.UpdateAmmo(currentAmmo); // Updates ammo count int the UI.
    }

    // Launches a projectile
    void Launch()
    {
        if (currentAmmo > 0)
        {
            // Creates a copy of the projectilePrefab GameObject at the specified position (no rotation).
            GameObject projectileObject = Instantiate(projectilePrefab, rigidbody2D.position + Vector2.up * 0.5f, Quaternion.identity);

            // Gets projectile script from the new object and call its 'Launch' function.
            Projectile projectile = projectileObject.GetComponent<Projectile>();
            projectile.Launch(lookDirection, 300); // Force expressed in Newton units.

            // Triggers the 'Launch' animation.
            animator.SetTrigger("Launch");
            PlaySound(throwCog); // Plays audio clip using PC as source.

            ChangeAmmo(-1);
            
        }
    }

    // Plays an audio clip one time.
    public void PlaySound(AudioClip clip)
    {
        audioSource.PlayOneShot(clip);
    }

    public void DialogueComplete()
    {
        isTalking = false;
    }

    public void QuestEnemy()
    {
        if (quest.isActive)
        {
            quest.goal.EnemyKilled();
            Debug.Log("Unit is killed");
            if (quest.goal.IsReached())
            {
                quest.Complete();
            }
        }
    }
}
