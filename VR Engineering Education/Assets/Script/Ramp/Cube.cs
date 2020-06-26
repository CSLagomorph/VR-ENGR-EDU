using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PhysicsLibrary;
public class Cube : MonoBehaviour
{
    //public float oldacceleration;
    public float acceleration;
    //public float oldvelocity;
    public float velocity;
    private Rigidbody rigid;
    // Start is called before the first frame update
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

        // Make sure that the object always spawn on top of the ramp. Adjust dynamically with the ramp height. 
        // Runtime ramp height change implement in the future.
        transform.position = new Vector3(transform.position.x, RampController.rampController.rampHeight, transform.position.z);

        // Align the cube with the ramp. Adjust dynamically with the ramp height. If not, cube experiences a bump due to mesh collision with the ramp.
        float angle = Mathf.Asin(RampController.rampController.rampHeight / RampController.rampController.rampHypotenuse);
        transform.Rotate(0f, 0f, -1 * angle * Mathf.Rad2Deg);

        /* Obsolete, equation has been centralized to PhysicsLib
        Acceleration of the cube
         Reference link: https://youtu.be/S54We3NRi9Y
        oldacceleration = 9.81f * ((RampController.rampController.rampHeight / RampController.rampController.rampHypotenuse) -
            0.08f * (RampController.rampController.rampLength / RampController.rampController.rampHypotenuse));
        Velocity of the cube. Derived from http://hyperphysics.phy-astr.gsu.edu/hbase/sphinc.html with no rotational kinetic energy.
        oldvelocity = Mathf.Sqrt(2f * 9.81f * RampController.rampController.rampHeight);
        */

        acceleration = PhysicsLib.GetRampSphericalAccel(0f, RampController.rampController.rampHeight, RampController.rampController.rampHypotenuse);
        velocity = PhysicsLib.GetRampSphericalVelocity(0f, RampController.rampController.rampHeight);
    }

    // Update is called once per frame
    void Update()
    {
        // Refer to PhysicHelper script for more details.
        PhysicsLib.ApplyForceToReachVelocity(rigid, Vector3.right * velocity, acceleration);
    }
}
