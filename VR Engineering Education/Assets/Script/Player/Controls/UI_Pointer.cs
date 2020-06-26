using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
public class UI_Pointer : MonoBehaviour
{
    public float length = 5.0f;

    public EventSystem eventSystem = null;
    public bool IsOn = true;

    public BaseInputModule inputModule = null;
    private LineRenderer lineRenderer = null;

    void OnEnable() {
        SceneManager.sceneLoaded += OnSceneLoaded;    
    }
    private void Start()
    {
        lineRenderer = this.GetComponent<LineRenderer>();
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<StandaloneInputModule>();
        Canvas[] canvases = (Canvas[])GameObject.FindObjectsOfType(typeof(Canvas));
        foreach(Canvas canvas in canvases)
        {
            canvas.worldCamera = GetComponent<Camera>();
        }
    }

    private void Update()
    {
        if(IsOn)
        {
            UpdateLength();
        }
        
    }

    private void UpdateLength()
    {
        lineRenderer.SetPosition(0, this.transform.position);
        lineRenderer.SetPosition(1, GetEnd());
    }

    private Vector3 GetEnd()
    {
        float distance = GetCanvasDistance();
        Vector3 endPosition = CalculateEnd(this.length);

        if(distance != 0)
        {
            return CalculateEnd(distance);
        }

        return endPosition;
    }

    private float GetCanvasDistance()
    {
        PointerEventData eventData = new PointerEventData(eventSystem);
        eventData.position = inputModule.inputOverride.mousePosition;

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(eventData, results);

        RaycastResult closestHit = FindClosestValidHit(results);
        float distanceToHit = Mathf.Clamp(closestHit.distance, 0.0f, this.length);
        return distanceToHit;
    }

    private RaycastResult FindClosestValidHit(List<RaycastResult> results)
    {
        foreach(RaycastResult result in results)
        {
            if(!result.gameObject)
            {
                continue;
            }
            return result;
        }
        return new RaycastResult();
    }

    private Vector3 CalculateEnd(float length)
    {
        return this.transform.position + (transform.forward * length);
    }

    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        EventSystem.current.GetComponent<StandaloneInputModule>().inputOverride = this.GetComponent<PointerInput>();
        eventSystem = EventSystem.current;
        inputModule = eventSystem.GetComponent<StandaloneInputModule>();
        Canvas[] canvases = (Canvas[])GameObject.FindObjectsOfType(typeof(Canvas));
        foreach(Canvas canvas in canvases)
        {
            canvas.worldCamera = this.GetComponent<Camera>();
        }
    }

}
