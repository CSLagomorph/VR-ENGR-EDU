using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PC_PlayerController : MonoBehaviour
{
    public static string TRANSLATION_AXIS = "Vertical";
    public static string STRAFFE_AXIS = "Horizontal";
    public static KeyCode SPRINT_KEY = KeyCode.LeftShift;
    
    public const float PLAYER_HEIGHT = 2.0F;

    public static KeyCode ROTATE_LEFT_KEY = KeyCode.Q;
    public static KeyCode ROTATE_RIGHT_KEY = KeyCode.E;
    private static float WALK_SPEED = 5.0f;
    private static float SPRINT_SPEED = 10.0f;
    private static float ROTATION_SPEED = 50.0f;
    public float movementSpeed;
    public bool isSprinting;
    public bool isMoving;
    public Vector2 lookVector;
    private Vector2 smoothLookVector;
    public Transform playerCamera;

    void Start()
    {
        lookVector = Vector2.zero;
        smoothLookVector = Vector2.zero;
        isSprinting = false;
        isMoving = false;
    }

    // Update is called once per frame
    void Update()
    {
        movePlayer();
        rotatePlayer();

        if(Input.GetKey(SPRINT_KEY))
        {
            isSprinting = true;
        }
        else
        {
            isSprinting = false;
        }
    }

    private void movePlayer()
    {
        movementSpeed = this.isSprinting ? SPRINT_SPEED : WALK_SPEED;
        float translation = Input.GetAxisRaw(TRANSLATION_AXIS) * movementSpeed * Time.deltaTime;
        float straffe = Input.GetAxisRaw(STRAFFE_AXIS) * movementSpeed * Time.deltaTime;
        if(translation > 0 || straffe > 0)
        {
            isMoving = true;
        }
        else
        {
            isMoving = false;
        }
        this.transform.Translate(straffe, 0.0f, translation);
        if(this.transform.position.y != PLAYER_HEIGHT)
        {
            this.transform.position = new Vector3(this.transform.position.x, PLAYER_HEIGHT, this.transform.position.z);
        }
    }

    private void rotatePlayer()
    {
        float direction = 0;
        if(Input.GetKey(ROTATE_LEFT_KEY))
        {
            direction = -1;
        }
        else if(Input.GetKey(ROTATE_RIGHT_KEY))
        {
            direction = 1;
        }
        if (Input.GetMouseButton(1))
        {
            transform.eulerAngles = transform.eulerAngles + new Vector3 (-Input.GetAxis("Mouse Y") * 10.0F, Input.GetAxis("Mouse X") * 10.0F, 0);
            transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
        }
    
        transform.Rotate(Vector3.up * direction * ROTATION_SPEED * Time.deltaTime);

    }
}
