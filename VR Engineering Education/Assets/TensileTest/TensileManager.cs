using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TensileManager : MonoBehaviour
{
    public GameObject metalRod;
    public SkinnedMeshRenderer metSMesh;
    public MeshRenderer metMesh;
    public GameObject topPiece;
    public SkinnedMeshRenderer topSMesh;
    public MeshRenderer topMesh;
    public GameObject botPiece;
    public SkinnedMeshRenderer botSMesh;
    public MeshRenderer botMesh;
    public GameObject crystal;
    private bool skinnedMesh;

    // Start is called before the first frame update
    void Start()
    {
        setup();
        
        //metalRod = null;
    }

    // Update is called once per frame
    void Update()
    {
        if(metalRod != null && metalRod.GetComponent<Animator>().GetCurrentAnimatorStateInfo(0).IsName("Snapped"))
        {
            //Debug.Log("haha");

            if (skinnedMesh)
            {
                metSMesh.enabled = false;
                topSMesh.enabled = true;
                botSMesh.enabled = true;
            }
            else
            {
                metMesh.enabled = false;
                topMesh.enabled = true;
                botMesh.enabled = true;
            }

            
        }
        else if (GameObject.FindGameObjectWithTag("MetalRod") != null)
        {
            setup();
        }
        else
        {
            //do nothing
        }
    }

    public void setup()
    {
        metalRod = GameObject.FindGameObjectWithTag("MetalRod");
        topPiece = GameObject.FindGameObjectWithTag("TopPiece");
        botPiece = GameObject.FindGameObjectWithTag("BottomPiece");

        crystal = GameObject.FindGameObjectWithTag("Crystal");

        if (metalRod.GetComponent<SkinnedMeshRenderer>() != null)
        {
            metSMesh = metalRod.GetComponent<SkinnedMeshRenderer>();
            topSMesh = topPiece.GetComponent<SkinnedMeshRenderer>();
            botSMesh = botPiece.GetComponent<SkinnedMeshRenderer>();
            topSMesh.enabled = false;
            botSMesh.enabled = false;
            skinnedMesh = true;
        }
        else if (metalRod.GetComponent<MeshRenderer>() != null)
        {
            metMesh = metalRod.GetComponent<MeshRenderer>();
            topMesh = topPiece.GetComponent<MeshRenderer>();
            botMesh = botPiece.GetComponent<MeshRenderer>();
            topMesh.enabled = false;
            botMesh.enabled = false;
            skinnedMesh = false;
        }
    }
}
