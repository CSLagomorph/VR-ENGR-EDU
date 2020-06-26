using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Stop_Force : MonoBehaviour
{

    public GameObject definedButton;
    public GameObject liftPlatform;
    public GameObject increaseButton;
    Increase_Force increaseForce;
    public Rigidbody liftRB;
    public UnityEvent OnClick = new UnityEvent();
    //int forceAmount = 0;
    public bool stopBool;

    // Use this for initialization
    void Start()
    {
        definedButton = this.gameObject;
        //liftPlatform = GameObject.Find("Lift_Platform");
        increaseButton = GameObject.Find("Increase_Button");
        increaseForce = increaseButton.GetComponent<Increase_Force>();
        liftRB = liftPlatform.GetComponent<Rigidbody>();
        stopBool = false;
    }

    // Update is called once per frame
    void Update()
    {
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked");
                stopBool = true;
                increaseForce.increaseBool = false;
                liftRB.isKinematic = false;
                OnClick.Invoke();
            }
        }
    }
}
