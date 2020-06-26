using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VectorArrow : MonoBehaviour
{
    public enum VectorType
    {
        Position,
        DirectionAndMagnatude
    }

    // The model component of the arrow.
    private Transform Model;

    // The transform that represents the position of the tip of the arrow.
    private Transform tipTransform;

    // The transform that represents the position of the base of the arrow.
    private Transform baseTransform;

    // The position of the tip of the arrow. If this is set, it will keep the base of the
    // arrow in the same location, and rotate and scale the arrow to adjust the tip's
    // position.
    public Vector3 TipPosition
    {
        get
        {
            if(!tipTransform)
            {
                tipTransform = this.transform.Find("Tip");
            }
            return tipTransform.position;
        }

        set
        {
            if(!tipTransform)
            {
                tipTransform = this.transform.Find("Tip");
            }
            tipTransform.position = value;
            UpdateArrowTransform();
        }
    }

    // The position of the base of the arrow. If this is set, it will keep the tip of the
    // arrow in the same location, and rotate and scale the arrow to adjust the base's
    // position.
    public Vector3 BasePosition
    {
        get
        {
            if(!baseTransform)
            {
                baseTransform = this.transform.Find("Base");
            }
            return baseTransform.position;
        }

        set
        {
            if(!baseTransform)
            {
                baseTransform = this.transform.Find("Base");
            }
            baseTransform.position = value;
            UpdateArrowTransform();
        }
    }

    public VectorType vectorType;
    
    void Start()
    {
        tipTransform = this.transform.Find("Tip");
        baseTransform = this.transform.Find("Base");
        Model = this.transform.Find("Model");
    }

    private void UpdateArrowTransform()
    {
        // Calculate the distance between the tip of the arrow and the base of the arrow. This is
        // the new length of the arrow.
        float distanceDelta = Vector3.Distance(BasePosition, TipPosition);
        Model.localScale = new Vector3(Model.localScale.x, Model.localScale.y, distanceDelta);
        //this.transform.localScale = new Vector3(this.transform.localScale.x, this.transform.localScale.y, distanceDelta);

        // Find the spot that is between the tip and base of the arrow and set the arrow's position
        // to it.
        Vector3 middlePoint = (BasePosition + TipPosition) / 2.0F;
        Model.position = middlePoint;
        //this.transform.position = middlePoint;

        // Determine the rotation required to allign the arrow model with the tip position and the
        // base position.
        Vector3 rotation = TipPosition - BasePosition;
        Model.rotation = Quaternion.LookRotation(rotation);
        //this.transform.rotation = Quaternion.LookRotation(rotation);
    }
}
