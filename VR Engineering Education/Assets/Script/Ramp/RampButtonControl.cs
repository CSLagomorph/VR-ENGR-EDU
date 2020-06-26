using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RampButtonControl : MonoBehaviour
{

    string CUBE_PREFAB_PATH = "Prefabs/Ramp/RampCube";
    string SPHERE_PREFAB_PATH = "Prefabs/Ramp/RampSphere";
    string CYLINDER_PREFAB_PATH = "Prefabs/Ramp/RampCylinder";
    PhysicsButton sphereButton;
    PhysicsButton cubeButton;
    PhysicsButton cylinderButton;
    PhysicsButton startStopButton;

    GameObject cube;
    GameObject sphere;
    GameObject cylinder;

    private bool isRunning;
    // Start is called before the first frame update
    void Start()
    {
        sphereButton = GameObject.Find("SphereButton").GetComponent<PhysicsButton>();
        cubeButton = GameObject.Find("CubeButton").GetComponent<PhysicsButton>();
        cylinderButton = GameObject.Find("CylinderButton").GetComponent<PhysicsButton>();
        startStopButton = GameObject.Find("StartStopButton").GetComponent<PhysicsButton>();
        isRunning = false;
    }

    // Update is called once per frame
    void Update()
    {
        slowTime();
        if(cylinder == null && cube == null && sphere == null)
        {
            isRunning = false;
        }
        if (startStopButton.buttonDown)
        {
            isRunning = !isRunning;
            if (cylinder != null)
            {
                if (isRunning)
                {
                    cylinder.GetComponent<Rigidbody>().isKinematic = false;
                    cylinder.GetComponent<SolidCylinder>().enabled = true;
                }
                else
                {
                    GameObject.Destroy(cylinder);
                    cylinder = GameObject.Instantiate((GameObject)Resources.Load(CYLINDER_PREFAB_PATH));
                    cylinder.transform.position = new Vector3(cylinder.transform.position.x, RampController.rampController.rampHeight, cylinder.transform.position.z);
                }
            }
            if (cube != null)
            {
                if (isRunning)
                {
                    cube.GetComponent<Rigidbody>().isKinematic = false;
                }
                else
                {
                    GameObject.Destroy(cube);
                    cube = GameObject.Instantiate((GameObject)Resources.Load(CUBE_PREFAB_PATH));
                }
            }
            if (sphere != null)
            {
                if (isRunning)
                {
                    sphere.GetComponent<Rigidbody>().isKinematic = false;
                    sphere.GetComponent<SolidSphere>().enabled = true;
                }
                else
                {
                    GameObject.Destroy(sphere);
                    sphere = GameObject.Instantiate((GameObject)Resources.Load(SPHERE_PREFAB_PATH));
                }
            }
        }
        /*if (isRunning)
        {
            return;
        }*/

        if (sphereButton.buttonDown)
        {
            if (!sphere)
            {
                sphere = GameObject.Instantiate((GameObject)Resources.Load(SPHERE_PREFAB_PATH));
            }
            else
            {
                GameObject.Destroy(sphere);
            }

        }

        if (cubeButton.buttonDown)
        {
            if (!cube)
            {
                cube = GameObject.Instantiate((GameObject)Resources.Load(CUBE_PREFAB_PATH));
            }
            else
            {
                GameObject.Destroy(cube);
            }

        }

        if (cylinderButton.buttonDown)
        {
            if (!cylinder)
            {
                cylinder = GameObject.Instantiate((GameObject)Resources.Load(CYLINDER_PREFAB_PATH));
            }
            else
            {
                GameObject.Destroy(cylinder);
            }

        }

    }

    private void slowTime()
    {
        if (Input.GetKey(KeyCode.K))
        {
            Time.timeScale = 0.5f;
        }
        else
        {
            Time.timeScale = 1f;
        }
    }
}
