using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This script controls the behaviour of hazardous area GameObjects.
// Author: Reuel Terezakis
public class DamageZone : MonoBehaviour
{
    // Triggers when another object is within its collison box.
    void OnTriggerStay2D(Collider2D other)
    {
        // Checks for and accesses the RubyController component of the Collider entering the trigger.
        RubyController controller = other.GetComponent<RubyController>();
        if (controller != null)
        {
                controller.ChangeHealth(-1); // Calls the ChangeHealth function from within the RubyController.
        }

    }
}
