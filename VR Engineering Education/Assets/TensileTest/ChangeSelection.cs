using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeSelection : MonoBehaviour
{
    public PhysicsButton changeButton;
    public GameObject definedButton;
    public TensileManager tMan;
    //public GameObject metalRod;
    //public string[] allMetalRods = { "Copper", "CastIron" };
    public ArrayList allMetalRods = new ArrayList();

    //All metals prefab Gameobjects go here
    public GameObject copper;
    public GameObject castIron;

    public GameObject crystal;
    
    //public GameObject copperTop;
    //public GameObject copperBot;
    private GameObject currentMetalRod;
    private GameObject currentTop;
    private GameObject currentBot;
    public int currentMetal;
    // Use this for initialization
    void Start()
    {
        changeButton = GameObject.Find("ChangeMaterialButton").GetComponentInChildren<PhysicsButton>();
        definedButton = this.gameObject;
        //metalRod = GameObject.FindGameObjectWithTag("MetalRod");
        //allMetalRods[0] = "Copper";
        //allMetalRods[1] = "CastIron";
        allMetalRods.Add("Copper");
        allMetalRods.Add("CastIron");
        currentMetalRod = copper;
        currentMetal = 0;
        Debug.Log(allMetalRods.Count);
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(allMetalRods.Length);
        var ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit Hit;

        //if (Input.GetMouseButtonDown(0))
        //{
            //if (Physics.Raycast(ray, out Hit) && Hit.collider.gameObject == gameObject)
            if(changeButton.buttonDown)
            {

                Destroy(GameObject.FindGameObjectWithTag(allMetalRods[currentMetal].ToString()));
                Destroy(GameObject.FindGameObjectWithTag("Crystal"));

                ++currentMetal;
                if (currentMetal > (allMetalRods.Count - 1))
                {
                    currentMetal = 0;
                }

                
                //Destroy(currentMetalRod);

                //This is where new metal types are added. Just add an elseif
                if (("All" + allMetalRods[currentMetal]) == copper.name)
                {
                    currentMetalRod = copper;
                    //currentTop = copperTop;
                    //currentBot = copperBot;
                }
                else if (("All" + allMetalRods[currentMetal]) == castIron.name)
                {
                    currentMetalRod = castIron;
                }
                
                GameObject newMetal = Instantiate<GameObject>(currentMetalRod);
                newMetal.transform.position = new Vector3(0, 2, -9);
                newMetal.tag = allMetalRods[currentMetal].ToString();
                newMetal.name = "All" + allMetalRods[currentMetal].ToString();
                tMan.setup();

                GameObject newCrystal = Instantiate<GameObject>(crystal);
                newCrystal.transform.position = new Vector3(0, -10, -9);
                newCrystal.tag = "Crystal";
                newCrystal.name = "CrystalTest";



                //newMetal;

                //tMan.metalRod = null;
                //Destroy(GameObject.FindGameObjectWithTag("MetalRod"));
                //Destroy(GameObject.FindGameObjectWithTag("TopPiece"));
                //Destroy(GameObject.FindGameObjectWithTag("BottomPiece"));
                //GameObject newRod = Instantiate<GameObject>(currentMetalRod);
                //newRod.transform.position = new Vector3(0, 10, 0);
                //newRod.tag = "MetalRod";
                //GameObject newTop = Instantiate<GameObject>(currentTop);
                //newTop.transform.position = new Vector3(0, 10.1f, 0);
                //newTop.tag = "TopPiece";
                //GameObject newBot = Instantiate<GameObject>(currentBot);
                //newBot.transform.position = new Vector3(0, 10, 0);
                //newBot.tag = "BotPiece";
            }
        //}
    }
}
