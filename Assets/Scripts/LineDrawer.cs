using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineDrawer : MonoBehaviour
{
    private LineRenderer lineRenderer;
    // Start is called before the first frame update
    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();

        // Ensure the LineRenderer component is not null
        if (lineRenderer == null)
        {
            Debug.LogError("LineRenderer component not found on this GameObject.");
        }

        // Set some default properties of the LineRenderer (you can adjust these in the Inspector as well)
        lineRenderer.startWidth = 0.05f;
        lineRenderer.endWidth = 0.05f;
        // lineRenderer.material.color = Color.red;
    }

    public void DrawLine(Vector3 startPoint, Vector3 endPoint)
    {
        // Set the positions of the LineRenderer
        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, startPoint);
        lineRenderer.SetPosition(1, endPoint);
    }

    public void ClearLine()
    {
        // Reset the position count to zero
        lineRenderer.positionCount = 0;
    }

    public void SetLineStyle(float start_width, float end_width, Color c)
    {
        lineRenderer.startWidth = start_width;
        lineRenderer.endWidth = end_width;
        lineRenderer.material.color = c;
    }

    // Update is called once per frame
    void Update()
    {

    }
}
