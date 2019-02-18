using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LineRenderer))]
public class DrawLines : MonoBehaviour
{
    private MovingSawblade movingSawblade;
    private LineRenderer lineRenderer;
    public float lineWidth = 0.1f;

    void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        lineRenderer.positionCount = transform.childCount;
        for (int i = 0; i < transform.childCount; i++)
        {
            lineRenderer.SetPosition(i, transform.GetChild(i).position);
        }
        lineRenderer.SetWidth(lineWidth, lineWidth);

        movingSawblade = transform.parent.GetComponent<MovingSawblade>();
    }

    void Update()
    {
        if (movingSawblade.closedLoop)
            lineRenderer.loop = true;
        else
            lineRenderer.loop = false;
    }
}
