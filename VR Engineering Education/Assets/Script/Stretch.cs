using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stretch : MonoBehaviour
{

    public Transform cubeTrans;
    public GameObject middleCube;
    public Vector3 cubeChange;
    public float distance;
    public float yPos;
    public float xPos;
    public float zPos;
    public float timer;
    public float x = 0;
    // Start is called before the first frame update
    void Start()
    {
        cubeTrans = this.gameObject.transform;
        cubeChange = new Vector3(-0.0001f, 0.0002f, -0.0001f);
        middleCube = GameObject.Find("MiddleCube");
        distance = middleCube.transform.position.y - cubeTrans.position.y;
        yPos = cubeTrans.localPosition.y;
        xPos = cubeTrans.localPosition.x;
        zPos = cubeTrans.localPosition.z;
        timer = 0;
    }

    // Update is called once per frame
    void Update()
    {
        if (cubeTrans.localScale.x >= 0.05 && timer == Mathf.Abs(distance))
        {
            cubeTrans.localScale += cubeChange;
            yPos -= (0.0002f * distance);
            cubeTrans.localPosition = new Vector3(xPos, yPos, zPos);
            timer = 0;
        }
        else
        {
            ++timer;
        }
    }
}
