using System.Collections;
using System.Collections.Generic;
using UnityEngine;


// Add this script to any object that the player should be able to interact with.
// The object is required to have a Rigidbody attached to it, and if it doesn't,
// one is added automatically. If there is a VR headset connected, the player will
// be able to interact with this object using the hand controllers. If there is no
// VR headset connected, instead the player can interact with the objects using the
// mouse.

[RequireComponent(typeof(Rigidbody))]
public class Interactable : MonoBehaviour
{
    // If true, the player can grab and move this object.
    public bool CanGrab;
    public bool CanCollide;
    public ObjectInteraction activeHand;

    private Rigidbody rigidbody;
    private Vector3 screenPoint;
	private Vector3 offset;

    void Start()
    {
        this.rigidbody = this.GetComponent<Rigidbody>();
    }
		
	void OnMouseDown()
    {
        if(!CanGrab)
        {
            return;
        }
        this.rigidbody.velocity = Vector3.zero;
		screenPoint = Camera.main.WorldToScreenPoint(gameObject.transform.position);
		offset = gameObject.transform.position - Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z));
	}
		
	void OnMouseDrag()
    {
        if(!CanGrab)
        {
            return;
        }
		Vector3 cursorPoint = new Vector3(Input.mousePosition.x, Input.mousePosition.y, screenPoint.z);
		Vector3 cursorPosition = Camera.main.ScreenToWorldPoint(cursorPoint) + offset;
		transform.position = cursorPosition;
	}
}
