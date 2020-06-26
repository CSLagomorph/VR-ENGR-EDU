using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A component used to force an object with a collider to ignore
// collisions with objects that aren't part of or attached to
// the player.
public class IgnoreNonPlayerCollision : MonoBehaviour
{
    private void OnCollisionEnter(Collision collision)
    {
        int index = Array.IndexOf(PlayerInfo.PlayerTransform.GetComponentsInChildren<Collider>(), collision.collider);
        if(index <= -1)
        {
            ContactPoint contact = collision.GetContact(0);
            Physics.IgnoreCollision(contact.thisCollider, contact.otherCollider);
        }
    }
}
