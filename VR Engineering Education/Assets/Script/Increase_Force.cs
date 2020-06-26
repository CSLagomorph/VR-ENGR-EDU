using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Increase_Force : MonoBehaviour
{

    public GameObject definedButton;
    public GameObject liftPlatform;
    public GameObject stopButton;
    public GameObject forceText;
    public TextMesh forceTM;
    Stop_Force stopForce;
    public Rigidbody liftRB;
    public UnityEvent OnClick = new UnityEvent();
    int forceAmount = 0;
    public bool increaseBool;

    // Use this for initialization
    void Start()
    {
        definedButton = this.gameObject;
        stopButton = GameObject.Find("Stop_Button");
        forceText = GameObject.Find("Force_Text");
        forceTM = forceText.GetComponent<TextMesh>();
        stopForce = stopButton.GetComponent<Stop_Force>();
        liftRB = liftPlatform.GetComponent<Rigidbody>();
        increaseBool = false;
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
                increaseBool = true;
                stopForce.stopBool = false;
                liftRB.isKinematic = false;
                OnClick.Invoke();
            }
        }

        if (increaseBool)
        {
            forceAmount = forceAmount + 1;
            liftRB.AddForce(0, forceAmount, 0);
            forceTM.text = forceAmount.ToString() + " Force";
        }

        if (stopForce.stopBool)
        {
            liftRB.isKinematic = true;
        }
    }
}