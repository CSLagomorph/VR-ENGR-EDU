using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileLifetime : MonoBehaviour
{
    private float elapsed_time = 0f;
    public float duration = 10f;
    private bool startDying = false;
    public Collider collider;
    // Start is called before the first frame update
    void Start()
    {
        Destroy(this.gameObject, duration);
    }

    // Update is called once per frame
    void Update()
    {
    }
}
