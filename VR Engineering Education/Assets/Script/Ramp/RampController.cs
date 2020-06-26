using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This class is used to create and update a ramp used in physics simulations.
// The ramp is a simple mesh, comprised of 6 vertices and 6 triangles. To
// adjust the size of the ramp, change the rampHeight, rampWidth, and rampLength
// respectively.
public class RampController : MonoBehaviour
{
    public float rampHeight;
    public float rampWidth;
    public float rampLength;
    public float rampHypotenuse;
    public Transform ramp;
    public Mesh rampMesh;
    private Vector3[] vertices;
    private List<int> triangles;

    private float defaultHeight = 2.0F;
    private float defaultWidth = 5.0F;
    private float defaultLength = 5.0F;

    public static RampController rampController;

    private void Awake()
    {
        rampController = this;
        rampHeight = defaultHeight;
        rampWidth = defaultWidth;
        rampLength = defaultLength;
        rampHypotenuse = Mathf.Sqrt(Mathf.Pow(rampHeight, 2) + Mathf.Pow(rampLength, 2));
    }

    void Start()
    {
        Initialize();
    }

    void Update()
    {
        UpdateMesh();
    }

    private void Initialize()
    {
        // Initializes a bunch of variables we need to generate the mesh.
        vertices = new Vector3[6];
        triangles = new List<int>();
        rampMesh = new Mesh();
        ramp = new GameObject("Ramp", typeof(MeshFilter), typeof(MeshRenderer), typeof(MeshCollider)).transform;
        //rampHeight = defaultHeight;
        //rampWidth = defaultWidth;
        //rampLength = defaultLength;

        // Creates the link between the ramp object and the adjustable mesh.
        ramp.GetComponent<MeshFilter>().mesh = rampMesh;

        // Sets the material to the defualt Unity material.
        ramp.GetComponent<MeshRenderer>().material = new Material(Shader.Find("Diffuse"));

        PhysicMaterial physicMaterial = new PhysicMaterial();
        physicMaterial.dynamicFriction = 0;
        physicMaterial.staticFriction = 0;
        ramp.GetComponent<MeshCollider>().sharedMaterial = physicMaterial;

    }

    private void UpdateMesh()
    {
        // Clears the vertices and triangles generated from the last update, and regenerates
        // them using current values.
        rampMesh.Clear();
        GenerateVertices();
        GenerateTriangles();

        // Creates the mesh from the generated vertices and triangles.
        rampMesh.vertices = vertices;
        rampMesh.triangles = triangles.ToArray();

        // Recalculating normals allows the ramp to properly register lighting updates.
        rampMesh.RecalculateNormals();

        // Updates the collider used by the ramp to reflect changes made to ramp mesh.
        ramp.GetComponent<MeshCollider>().sharedMesh = null;
        ramp.GetComponent<MeshCollider>().sharedMesh = rampMesh;
    }

    private void GenerateVertices()
    {
        // Note: The vertices are based on the origin, since changes made
        // to the ramp's transform will also be made to the mesh.
        vertices[0] = new Vector3(0.0F, 0.0F, 0.0F);
        vertices[1] = new Vector3(0.0F, 0.0F, rampWidth);
        vertices[2] = new Vector3(0.0F, rampHeight, 0.0F);
        vertices[3] = new Vector3(0.0F, rampHeight, rampWidth);
        vertices[4] = new Vector3(rampLength, 0.0F, 0.0F);
        vertices[5] = new Vector3(rampLength, 0.0F, rampWidth);
    }

    private void GenerateTriangles()
    {
        // Clearing triangles is necessary here because the mesh will render
        // all triangles in the list, even if they were generated previously.
        triangles.Clear();

        // Triangles that make up the back of the ramp
        AddTriangle(0, 1, 2);
        AddTriangle(1, 3, 2);

        // Triangles that make up the sides of the ramp
        AddTriangle(0, 2, 4);
        AddTriangle(1, 5, 3);

        // Triangles that make up the top of the ramp
        AddTriangle(5, 4, 3);
        AddTriangle(3, 4, 2);
    }

    // Helper method used to make reading the creation of triangles a bit easier.
    private void AddTriangle(int vertex1, int vertex2, int vertex3)
    {
        triangles.Add(vertex1);
        triangles.Add(vertex2);
        triangles.Add(vertex3);
    }

}
