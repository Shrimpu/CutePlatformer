using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CircleMovement))]
[RequireComponent(typeof(LineRenderer))]
public class DrawLineSinglePoint : MonoBehaviour
{
    private Transform target;
    private LineRenderer lineRenderer;
    public float lineWidth = 0.1f;

    void Start()
    {
        CircleMovement cm = GetComponent<CircleMovement>();
        if (cm.centerObject != null)
            target = cm.centerObject;
        else
            target = null;

        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.SetWidth(lineWidth, lineWidth);

        lineRenderer.positionCount = 2;
        lineRenderer.SetPosition(0, transform.position);
        if (target != null)
            lineRenderer.SetPosition(1, target.position);
        else
        {
            lineRenderer.SetPosition(1, cm.localCenter);
        }
    }

    void LateUpdate()
    {
        lineRenderer.SetPosition(0, transform.position);
    }
}
