using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CastIronPull : MonoBehaviour
{
    public Transform cubeTrans;
    public int time;
    public float xPos;
    public float yPos;
    public float zPos;

    // Start is called before the first frame update
    void Start()
    {
        cubeTrans = this.gameObject.transform;
        time = 0;
        xPos = cubeTrans.localPosition.x;
        yPos = cubeTrans.localPosition.y + 1;
        zPos = cubeTrans.localPosition.z;
    }

    // Update is called once per frame
    void Update()
    {
        ++time;
        if (time == 1000)
        {
            cubeTrans.localPosition = new Vector3(xPos, yPos, zPos);
        }
    }
}
