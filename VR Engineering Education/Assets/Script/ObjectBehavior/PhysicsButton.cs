using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// A button that can be pressed with the VR player's hands, or if no
// VR device is connected, can be pressed by clicking with the mouse.
public class PhysicsButton : MonoBehaviour
{
    // The percentage of the button that needs to be pushed down in order
    // for the button to actuate. Ex: 0.8 means that 80% of the button's
    // height needs to be pressed down before the button is registered
    // as pressed.
    public float pressLengthMultiplier;

    // The length that the button needs to be pushed down before it is
    // considered pressed. This is automatically set to be the height
    // of the button multiplied by the pressLengthMultiplier. If the
    // pressLengthMultiplier is not set, the default is %80 of the
    // button's height.
    public float pressLength;

    // This will be true if the button is pressed, and false otherwise.
    public bool isPressed;

    // True only on the frame after the button is switched from not
    // pressed to pressed. False otherwise.
    public bool buttonDown;

    private Transform buttonBody;
    private Vector3 startPosition;
    private Rigidbody rigidbody;
    private BoxCollider clickCollider;

    private MeshRenderer renderer;
    private Material defaultMaterial;

    // The material that the button swaps to while being pushed down,
    // but not fully pressed. If this is not set within the Editor, 
    // it will default to the material currently applied to the 
    // ButtonBody.
    public Material pressingMaterial;

    // The material that the button swaps to once it is being pressed.
    // If this is not set within the Editor, it will default to the
    // material currently applied to the ButtonBody.
    public Material pressedMaterial;

    void Start()
    {
        buttonBody = this.transform.Find("ButtonBody");
        startPosition = buttonBody.position;
        rigidbody = buttonBody.GetComponent<Rigidbody>();
        clickCollider = this.GetComponent<BoxCollider>();
        if(pressLengthMultiplier == 0)
        {
            pressLengthMultiplier = 0.8F;
        }
        renderer = buttonBody.GetComponent<MeshRenderer>();
        pressLength = renderer.bounds.size.y * pressLengthMultiplier;
        
        defaultMaterial = renderer.material;
        if(!pressingMaterial)
        {
            pressingMaterial = renderer.material;
        }
        if(!pressedMaterial)
        {
            pressingMaterial = renderer.material;
        }
        buttonDown = false;
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Mathf.Abs(buttonBody.position.y - startPosition.y);
        if(distance >= pressLength)
        {
            buttonBody.position = new Vector3(buttonBody.position.x, startPosition.y - pressLength, buttonBody.position.z);
            if(!isPressed)
            {
                isPressed = true;
                buttonDown = true;
            }
            else
            {
                buttonDown = false;
            }
        }
        else
        {
            isPressed = false;
            buttonDown = false;
        }

        if(buttonBody.position.y > startPosition.y)
        {
            buttonBody.position = new Vector3(buttonBody.position.x, startPosition.y, buttonBody.position.z);
        }

        if(isPressed)
        {
            renderer.material = pressedMaterial;
        }
        else if(buttonBody.position.y != startPosition.y)
        {
            renderer.material = pressingMaterial;
        }
        else
        {
            renderer.material = defaultMaterial;
        }
        clickCollider.size = buttonBody.localScale * 1.1F;
    }

    private void OnMouseDown()
    {
        if(!this.isPressed)
        {
            buttonBody.position = new Vector3(buttonBody.position.x, startPosition.y - pressLength, buttonBody.position.z);
        }
    }

}
