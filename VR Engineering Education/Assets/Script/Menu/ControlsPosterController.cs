using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsPosterController : MonoBehaviour
{
    private Transform pcControls;
    private Transform vrControls;
    // Start is called before the first frame update
    void Start()
    {
        pcControls = this.transform.Find("PC Controls");
        vrControls = this.transform.Find("VR Controls");
    }

    // Update is called once per frame
    void Update()
    {
        if(PlayerInfo.VRDeviceName == PlayerInfo.NO_DETECTED_DEVICE)
        {
            pcControls.gameObject.SetActive(true);
            vrControls.gameObject.SetActive(false);
        }
        else
        {
            pcControls.gameObject.SetActive(false);
            vrControls.gameObject.SetActive(true);
        }
    }
}
