using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controlls the behaviour of CollectibleAmmo GameObjects.
// Author: Reuel Terezakis

public class AmmoCollectible : MonoBehaviour
{
    // Stores audio clip.
    public AudioClip collectedClip;

    // Triggers when another object collides with it.
    void OnTriggerEnter2D(Collider2D other)
    {
        // Checks if collided with PC.
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
            // Checks if PC is below max. ammo.
            if (controller.ammo < controller.maxAmmo)
            {
                controller.ChangeAmmo(5); // Calls the ChangeAmmo function from within the RubyController.
                Destroy(gameObject); // GameObject destroyed after collision.

                controller.PlaySound(collectedClip); // Plays audio clip using PC as source.
            }
        }

    }
}
