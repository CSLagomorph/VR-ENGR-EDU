using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PathTracer : MonoBehaviour
{
    private List<Vector3> vertices;
    private float fadeCounter;
    private LineRenderer line;
    public float resolution = 0.2F;
    public float fadeDelay = 0.005F;

    void Start()
    {
        vertices = new List<Vector3>();
        line = this.GetComponent<LineRenderer>();
        fadeCounter = 0.0F;
        AddCurrentPositionToLine();
    }

    void Update()
    {
        if(Vector3.Distance(this.transform.position, line.GetPosition(line.positionCount - 1)) > this.resolution)
        {
            AddCurrentPositionToLine();
        }

        if(fadeDelay > 0)
        {
            fadeCounter += Time.deltaTime;
            if(fadeCounter >= fadeDelay)
            {
                fadeCounter = 0.0F;
                RemoveOldestPoint();
            }
        }

    }


    void AddCurrentPositionToLine()
    {
        if(fadeCounter >= fadeDelay)
        {
            vertices.RemoveAt(0);
            fadeCounter = 0.0F;
        }
        vertices.Add(this.transform.position);
        line.positionCount = vertices.Count;
        line.SetPositions(vertices.ToArray());
    }

    void RemoveOldestPoint()
    {
        if(vertices.Count > 0)
        {
            vertices.RemoveAt(0);
        }
        line.positionCount--;
        line.SetPositions(vertices.ToArray());
    }
}
