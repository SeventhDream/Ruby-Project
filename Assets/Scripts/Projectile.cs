using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Projectile : MonoBehaviour
{
    // Creates a class variable to store the Rigidbody and access them from within the script.
    Rigidbody2D rigidbody2D;

    // Allows to assign a GameObject with the ParticleSystem component type to this setting (no need to GetComponent later).
    public ParticleSystem projectileParticlePrefab;

    // Awake is called When object is created.
    void Awake()
    {
        // Gets the Rigidbody2D attached to the GameObject.
        rigidbody2D = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        // Destroys projectile when a certain distance away from the centre of the world.
        if (transform.position.magnitude > 100.0f) // Or calculate distance from the character: (Vector3.Distance(a.b)); or use a timer.
        {
            Destroy(gameObject);
        }
    }

    // Launches the projectile using the in-built physics system.
    public void Launch(Vector2 direction, float force)
    {
        // Applied force in direction multiplied by the force.
        rigidbody2D.AddForce(direction * force);
    }

    // Detects collision
    void OnCollisionEnter2D(Collision2D other)
    {
        // Get info on the GameObject with which the projectile collided.
        EnemyController e = other.collider.GetComponent<EnemyController>();
            if (e != null)
            {
            e.Fix(); // Calls the EnemyController function to 'fix' the enemy GameObject.
            }
        ParticleSystem projectileParticleSystem = Instantiate(projectileParticlePrefab, rigidbody2D.position, Quaternion.identity);
        Destroy(gameObject); // Destroys projectile upon collision.
    }
}
