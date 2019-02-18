using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleMovement : MonoBehaviour
{
    public bool useSawAsCenter;
    public bool spinClockwise;
    public float DirectionOfRotaion
    {
        get { return spinClockwise == true ? -1f : 1f; }
    }
    public float rotationSpeed = 1.5f;
    public float radius = 1f;
    [Space]
    public Transform centerObject;

    [HideInInspector]
    public Vector3 localCenter;

    void Awake()
    {
        localCenter = transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (useSawAsCenter)
        {
            Move(localCenter);
        }
        else
        {
            if (centerObject != null)
                Move(centerObject.transform.position);
            else
                Debug.LogWarning("Spinning Sawblade centerObject not assigned!");
        }
    }

    void Move(Vector3 center)
    {
        transform.position = new Vector3(Mathf.Cos(-Time.time * rotationSpeed * DirectionOfRotaion),
            Mathf.Sin(-Time.time * rotationSpeed * DirectionOfRotaion)) * radius + center;
    }
}
