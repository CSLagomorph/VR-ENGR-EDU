using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartTensileTest : MonoBehaviour
{

    public PhysicsButton definedButton;
    public GameObject metalRod;
    public TensileManager tMan;
    public GameObject crystal;
    public GameObject otherRod;
    // Use this for initialization
    void Start()
    {
        definedButton = GameObject.Find("StartTestButton").GetComponentInChildren<PhysicsButton>();
        //crystal = tMan.crystal;
    }

    // Update is called once per frame
    void Update()
    {
        metalRod = GameObject.FindGameObjectWithTag("MetalRod");
        Animator animator = metalRod.GetComponent<Animator>();

        crystal = GameObject.FindGameObjectWithTag("Crystal");
        Animator crystalAnim = crystal.GetComponent<Animator>();

        /*var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        if (Input.GetMouseButtonDown(0))
        {
            if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            {
                Debug.Log("Button Clicked");
                metalRod.GetComponent<Animator>().Play(metalRod.name + "_Tensile_Test");
            }
        }*/

        if(animator.GetCurrentAnimatorStateInfo(0).length <= animator.GetCurrentAnimatorStateInfo(0).normalizedTime)
        {
            if(definedButton.isPressed)
            {
                animator.Play(metalRod.name  + "_Tensile_Test");
                crystalAnim.Play(crystal.name + "_Tensile_Test");
            }
        }
    }
}
