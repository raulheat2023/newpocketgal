using UnityEngine;
using System.Collections.Generic;

public class TrajectoryLine : MonoBehaviour
{
    public LineRenderer lineRenderer; 
    public int maxReflections = 5; // Máximo de rebotes
    public float maxLength = 5f; // Longitud total en Unity Units (~200px)
    public LayerMask collisionMask; // Bordes de la mesa
    private bool isDragging = false;

    void Start()
    {
        lineRenderer.enabled = false; // Oculta la línea al inicio
        lineRenderer.positionCount = 0;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            if (Vector2.Distance(mousePosition, transform.position) < 0.5f)
            {
                isDragging = true;
                lineRenderer.enabled = true;
            }
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            DrawTrajectory();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            lineRenderer.enabled = false; // Oculta la línea al soltar el clic
        }
    }

    void DrawTrajectory()
    {
        List<Vector2> points = new List<Vector2>();
        Vector2 start = transform.position;
        Vector2 direction = (Camera.main.ScreenToWorldPoint(Input.mousePosition) - (Vector3)start).normalized;

        points.Add(start);

        RaycastHit2D hit;
        for (int i = 0; i < maxReflections; i++)
        {
            hit = Physics2D.Raycast(start, direction, maxLength, collisionMask);
            if (hit.collider != null)
            {
                start = hit.point;
                direction = Vector2.Reflect(direction, hit.normal);
                points.Add(start);
            }
            else
            {
                points.Add(start + direction * maxLength);
                break;
            }
        }

        lineRenderer.positionCount = points.Count;
        for (int i = 0; i < points.Count; i++)
        {
            lineRenderer.SetPosition(i, points[i]);
        }
    }
}
