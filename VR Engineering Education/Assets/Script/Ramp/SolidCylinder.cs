using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsLibrary;
public class SolidCylinder : MonoBehaviour
{
    public float acceleration;
    public float velocity;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        // Make sure that the object always spawn on top of the ramp. Adjust dynamically with the ramp height. 
        // Runtime ramp height change implement in the future.
        transform.position = new Vector3(transform.position.x, RampController.rampController.rampHeight, transform.position.z);

        // Obsolete, equation has been centralized to PhysicsLib
        //acceleration = (9.81f * (RampController.rampController.rampHeight / RampController.rampController.rampHypotenuse)) / (1.5f);
        //velocity = Mathf.Sqrt((1.333f) * Mathf.Abs(Physics.gravity.y) * RampController.rampController.rampHeight);

        // Acceleration and velocity of the sphere. Derived formulas from this link: http://hyperphysics.phy-astr.gsu.edu/hbase/hoocyl.html
        // Coefficient of 1 / 2 = 0.5f
        velocity = PhysicsLib.GetRampSphericalVelocity(0.5f, RampController.rampController.rampHeight);
        acceleration = PhysicsLib.GetRampSphericalAccel(0.5f, RampController.rampController.rampHeight, RampController.rampController.rampHypotenuse);
    }

    // Update is called once per frame
    void Update()
    {
        // Refer to PhysicsLib script for more details.
        PhysicsLib.ApplyForceToReachVelocity(rigid, Vector3.right * velocity, acceleration);
    }
}
