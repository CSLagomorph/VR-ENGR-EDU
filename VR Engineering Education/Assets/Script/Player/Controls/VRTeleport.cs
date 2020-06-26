using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VRTeleport : MonoBehaviour
{

    // The max number of vertices used to draw the line.
    private const int maxVerticesCount = 100;

    // The distance between vertices in the line.
    private const float vertexSpacing = 0.05F;

    // The line used to represent the teleporter's arc.
    private LineRenderer line;

    // The list of vertices used by the LineRenderer to draw
    // the line.
    private List<Vector3> lineVertices;

    // Whether or not the teleporter is active.
    private bool isActive;

    // Whether or not a surface is detected by the teleporter.
    private bool surfaceDetected = false;

    // The position on the surface that was detected by the teleporter.
    private Vector3 surfacePosition = Vector3.zero;

    // The normal of the surface detected by the teleporter.
    private Vector3 surfaceNormal = Vector3.zero;

    // The object that will be used to mark the target position
    // for teleportation.
    public Transform targetMarker;

    // The player's transform.
    public Transform playerTransform;


    // The initial angle of the teleporter arc.
    public float angle = 45.0F;

    // This value is used to control the distance of the launch
    // arc.
    public float distance = 8.0F;

    private void Awake()
    {
        line = this.GetComponent<LineRenderer>();
        line.enabled = false;
        targetMarker.gameObject.SetActive(false);
    }

    private void FixedUpdate()
    {
        if(isActive)
        {
            UpdateLine();
        }    
    }

    private void UpdateLine()
    {
        // Reset surface detection.
        surfaceDetected = false;

        // List for storing the vertices that are calculated to make up the
        // line.
        List<Vector3> vertices = new List<Vector3>();

        // Calculate the starting velocity of the line. This is used when adding
        // each vertex by multiplying this by the vertexSpacing field.
        Vector3 velocity = Quaternion.AngleAxis(-angle, this.transform.right) * this.transform.forward * distance;

        // Set the starting vertex to the position of the teleporter, and add it to the
        // list of vertices.
        Vector3 currentPosition = this.transform.position;
        vertices.Add(currentPosition);

        // While no surface is detected, and the max length of the line has not been reached
        while(!surfaceDetected && vertices.Count < maxVerticesCount)
        {
            // Get the next position for a vertex along the teleport arc and add it to
            // the list of vertices.
            Vector3 nextPosition = currentPosition + velocity * vertexSpacing;
            vertices.Add(nextPosition);

            // Update the velocity to account for the next vertex.
            velocity += Physics.gravity * vertexSpacing;
            
            // Check for a valid teleporting surface between the current vertex and
            // the position of the next vertex. If one is hit, store information about
            // the position of that surface to use when teleporting.
            RaycastHit hit;
            if(Physics.Linecast(currentPosition, nextPosition, out hit, 1 << LayerMask.NameToLayer("TeleportationTarget")))
            {
                surfaceDetected = true;
                surfacePosition = hit.point;
                surfaceNormal = hit.normal;
            }
            
            // Advance the current vertex position to the next vertex position for
            // the next pass of the loop.
            currentPosition = nextPosition;
        }

        // Update the visibility of the target marker based on whether or not a
        // surface is detected.
        targetMarker.gameObject.SetActive(surfaceDetected);

        // If a surface is detected, update the position of the target marker to
        // be on that surface.
        if(surfaceDetected)
        {
            targetMarker.position = surfacePosition + surfaceNormal * 0.04F;
            targetMarker.transform.up = surfaceNormal;
        }


        // Update the LineRenderer to display the line made up of all the
        // vertices grabbed this Update.
        line.positionCount = vertices.Count;
        line.SetPositions(vertices.ToArray());
        
    }

    // Call this to teleport the playerTransform to the targetMarker's position.
    public void Teleport()
    {
        if(surfaceDetected)
        {
            playerTransform.position = surfacePosition + surfaceNormal * 0.04F;
        }
    }


    // Sets the teleporter to be active or inactive.
    public void SetActive(bool isActive)
    {
        this.isActive = isActive;
        targetMarker.gameObject.SetActive(isActive);
        line.enabled = isActive;

    }

}
