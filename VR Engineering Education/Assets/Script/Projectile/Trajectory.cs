using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Trajectory : MonoBehaviour
{
    public static Trajectory trajectoryManager;

    public PhysicsButton launchButton;
    public GameObject target;

    // Projectile control values
    public float duration = 0f;
    public float gravity = Mathf.Abs(Physics.gravity.y);

    // Projectile object
    public GameObject platform;
    public Rigidbody projectile;
    public Vector3 currentVelocity;

    // Angle control
    public float angle;
    public Slider angleSlider = null;

    /*
    // Correct Answer
    public float correctVx = 0f;
    public float correctVy = 0 
    */

    // Question values
    public float givenDistanceX = 0f;
    public float givenTime = 0f;
    public float givenHeight = 0f;
    public float givenDistanceY = 0f;

    //Users control y values
    public Text Vy;
    public Text correctAnswer;
    void Awake()
    {
        trajectoryManager = this;
        angle = (float)angleSlider.value;
    }

    // Start is called before the first frame update
    void Start()
    {
        launchButton = GameObject.Find("LaunchButton").GetComponent<PhysicsButton>();
        rotateCannon(target.transform.position, angle);
    }

    // Update is called once per frame
    void Update()
    {
        rotateCannon(target.transform.position, angle);
        if (launchButton.buttonDown)
        {
            LaunchProjectileOnStudentInput();
        }
    }

    public void LaunchProjectile()
    {
        Vector3 correctVelocity = calculateTrajectoryVelocity(target.transform.position, transform.position);
        Rigidbody obj = Instantiate(projectile, transform.position, Quaternion.identity);
        obj.GetComponent<ProjectileLifetime>().duration = duration + 2f;
        obj.velocity = correctVelocity;
        correctAnswer.text = "Correct Vy is: " + Math.Round(correctVelocity.y, 2).ToString();
    }

    private void LaunchProjectileOnStudentInput()
    {
        float studentVy;
        float.TryParse(Vy.text, out studentVy);

        Vector3 studentVelocity = calculateTrajectoryVelocity(target.transform.position, transform.position);
        Rigidbody obj = Instantiate(projectile, transform.position, Quaternion.identity);
        obj.GetComponent<ProjectileLifetime>().duration = duration + 2f;
        studentVelocity = new Vector3(studentVelocity.x, studentVy, studentVelocity.z);
        obj.velocity = studentVelocity;
    }

    public void getVelocity()
    {
        angle = (float)angleSlider.value;
        currentVelocity = calculateTrajectoryVelocity(target.transform.position, transform.position);
    }

    private Vector3 calculateTrajectoryVelocity(Vector3 target, Vector3 origin)
    {
        angle = (float)angleSlider.value;
        // Define gravity
        float gravity = Math.Abs(Physics.gravity.y);

        // Define the distance x and y
        Vector3 distance = target - origin;
        Vector3 horizontalDistance = distance;
        horizontalDistance.y = 0f;

        // Actual length of the distance
        float Dy = distance.y;
        float Dxz = horizontalDistance.magnitude;

        float height = (Dxz * Mathf.Tan(angle * Mathf.Deg2Rad)) / 4;

        Vector3 Vy = Vector3.up * Mathf.Sqrt(2 * gravity * height);
        float time = Mathf.Sqrt(2 * height / gravity) + Mathf.Sqrt(-2 * (Dy - height) / gravity);
        Vector3 Vxz = horizontalDistance / time;

        Vector3 result = Vxz + Vy;

        /*
        // Correct values
        correctVo = result.magnitude;
        correctVy = Vy.magnitude;
        correctVx = Vxz.magnitude;
        */

        // Given values
        givenDistanceX = Dxz;
        givenHeight = height;
        givenTime = time;
        givenDistanceY = Dy;
        duration = time;

        //return result;
        return result;
    }

    private void rotateCannon(Vector3 target, float angle)
    {
        platform.transform.LookAt(target);
        platform.transform.rotation = new Quaternion(0f, platform.transform.rotation.y, 0f, platform.transform.rotation.w);
    }

    public void randomizeQuestion()
    {
        target.transform.position = new Vector3(UnityEngine.Random.Range(platform.transform.position.x - 20.0f, platform.transform.position.x + 20.0f), 0.0001f, UnityEngine.Random.Range(platform.transform.position.z - 20.0f, platform.transform.position.z + 20.0f));
        currentVelocity = calculateTrajectoryVelocity(target.transform.position, transform.position);
    }

    // This region contains non working (at least not as intended) visualization of the trajectory
    #region Not Working
    /*
    Vector3 calculatePositionTime(Vector3 Vi, float time)
    {
        Vector3 Vxz = Vi;
        Vi.y = 0f;

        Vector3 result = transform.position + Vxz * time;
        float Sy = (-0.5f) * Mathf.Abs(Physics.gravity.y) * Mathf.Pow(time, 2) + Vi.y * time + transform.position.y;

        result.y = Sy;

        return result;
    }

    void visualLine(Vector3 Vi)
    {
        for (int i = 0; i < lineSegment; i++)
        {
            Vector3 pos = calculatePositionTime(Vi, i / lineSegment);
            lineRenderer.SetPosition(i, pos);
        }
    }

    private void RenderTrajectory(Vector3 target, Vector3 origin)
    {
        lineRenderer.positionCount = resolution + 1;
        lineRenderer.SetPositions(calculateTrajectory(target, origin));
    }

    // Creare Vector3 array positions for trajectory
    private Vector3[] calculateTrajectory(Vector3 target, Vector3 origin)
    {
        Vector3[] result = new Vector3[resolution + 1];
        float Vo = initialVelocity.x;
        float distance = ((Mathf.Pow(Vo, 2)) * (Mathf.Sin(2 * angle * Mathf.Deg2Rad))) / gravity;

        for (int i = 0; i <= resolution; i++)
        {
            float t = (float)i / (float)resolution;
            result[i] = calculateTrajectoryInTime(t, distance);
        }
        return result;
    }

    private Vector3 calculateTrajectoryInTime(float t, float maxDistance)
    {
        float x = t * maxDistance;
        float y = x * Mathf.Tan(angle * Mathf.Deg2Rad) - ((gravity * x * x) / (2 * initialVelocity.x * initialVelocity.x * Mathf.Cos(angle * Mathf.Deg2Rad) * Mathf.Cos(angle * Mathf.Deg2Rad)));
        return new Vector3(x, y, 0f);
    }
    */
    #endregion
}
