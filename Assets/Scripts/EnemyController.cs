using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the behaviour of enemy GameObjects.
// Author: Reuel Terezakis
public class EnemyController : MonoBehaviour
{
    public Quest quest;
    private RubyController rubyController;

    // Creates a class variable to store the Rigidbody2D, and Animator respectively and access them from within the script.
    Rigidbody2D rigidbody2D;
    Animator animator;

    AudioSource audioSource;
    public AudioClip hitClip;
    public AudioClip fixedClip;

    // Allows to assign a GameObject with the ParticleSystem component type to this setting (no need to GetComponent later).
    public ParticleSystem smokeEffect;

    // Enemies start broken.
    bool broken = true;

    // Enemy speed.
    public float speed;

    // Starting direction.
    public bool startLeft;


    // Patrol timer
    public bool vertical; // Boolean
    public float changeTime = 3.0f; // Time before the enemy’s direction is reversed.
    float timer; // Current value of the timer.
    int direction;

    // Start is called before the first frame update.
    void Start()
    {
        // Gets the Rigidbody2D and animator attached to the GameObject.
        rigidbody2D = GetComponent<Rigidbody2D>();
        animator = GetComponent<Animator>();
        audioSource = GetComponent<AudioSource>();
        // Initialise patrol timer.
        timer = changeTime;
        // Starting direction.
        if (!startLeft)
        {
            direction = 1;
        }
        else
        {
            direction = -1;
        }
    }

    // Update is called once per frame.
    void Update()
    {

        // When fixed, the enemy will stop moving. 
        if (!broken)
        {
            return;
        }

        // Countdown and change direction when vertical timer reaches zero.
        timer -= Time.deltaTime;

        if (timer < 0)
        {
            direction = -direction;
            timer = changeTime;
        }

        // Stores the x- and y- coordinates of the Rigidbody in a Vector2-type variable.
        Vector2 position = rigidbody2D.position;

        // Modifies either the stored x- and y- coordinates (depends on speed, frame rendering, and 'vertical' boolean).
        if (vertical)
        {
            // Modifies the stored y-coordinate.
            position.y = position.y + Time.deltaTime * speed * direction;

            // Sends parameter values to the animator.
            animator.SetFloat("Move X", 0);
            animator.SetFloat("Move Y", direction);
        }
        else
        {
            // Modifies the stored x-coordinate.
            position.x = position.x + Time.deltaTime * speed * direction;

            // Sends parameter values to the animator.
            animator.SetFloat("Move X", direction);
            animator.SetFloat("Move Y", 0);
        }

        // Updates enemy's Rigidbody x- and y- cooordinates in Unity.
        rigidbody2D.MovePosition(position);

    }

    // Damages PC upon collision and if they stay in contact.
    void OnCollisionStay2D(Collision2D other)
    {
        // Get info on the GameObject with which the enemy collided.
        RubyController player = other.gameObject.GetComponent<RubyController>(); // A Collision2D doesn’t have a GetComponent function.

        if (player != null)
        {
            player.ChangeHealth(-1); // Calls the ChangeHealth function from within the RubyController.
        }
    }

    // 'Fixes' the enemy when hit with a PC's projectile Gameobject.
    public void Fix()
    {
        broken = false;
        rigidbody2D.simulated = false; // enemy can no longer collide with other GameObjects
        animator.SetTrigger("Fixed"); // triggers 'fixed' animation
        smokeEffect.Stop(); // Stops the smoke.
        audioSource.Stop();
        audioSource.PlayOneShot(hitClip);
        audioSource.PlayOneShot(fixedClip);
        rubyController = FindObjectOfType<RubyController>();
        rubyController.QuestEnemy();
       

    }
}
