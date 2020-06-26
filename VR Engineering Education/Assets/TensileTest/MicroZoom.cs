using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MicroZoom : MonoBehaviour
{

    public PhysicsButton zoomButton;
    public TensileManager tMan;
    private bool zoomed;

    // Start is called before the first frame update
    void Start()
    {
        zoomButton = GameObject.Find("ZoomButton").GetComponentInChildren<PhysicsButton>();
        zoomed = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (zoomButton.buttonDown && !zoomed)
        {
            tMan.metalRod.transform.position = new Vector3(tMan.metalRod.transform.position.x, -10, tMan.metalRod.transform.position.z);
            tMan.topPiece.transform.position = new Vector3(tMan.topPiece.transform.position.x, -10, tMan.topPiece.transform.position.z);
            tMan.botPiece.transform.position = new Vector3(tMan.botPiece.transform.position.x, -10, tMan.botPiece.transform.position.z);

            tMan.crystal.transform.position = new Vector3(tMan.crystal.transform.position.x, 2, tMan.crystal.transform.position.z);
            zoomed = true;
        }
        else if (zoomButton.buttonDown && zoomed)
        {
            tMan.metalRod.transform.position = new Vector3(tMan.metalRod.transform.position.x, 2, tMan.metalRod.transform.position.z);
            tMan.topPiece.transform.position = new Vector3(tMan.topPiece.transform.position.x, 2.1f, tMan.topPiece.transform.position.z);
            tMan.botPiece.transform.position = new Vector3(tMan.botPiece.transform.position.x, 2, tMan.botPiece.transform.position.z);

            tMan.crystal.transform.position = new Vector3(tMan.crystal.transform.position.x, -10, tMan.crystal.transform.position.z);
            zoomed = false;
        }
    }
}
