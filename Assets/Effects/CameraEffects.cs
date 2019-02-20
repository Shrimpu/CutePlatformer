using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraEffects : MonoBehaviour
{
    public void Shake(float intensity = 0.1f, int shakes = 2)
    {
        StartCoroutine(ShakeCamera(intensity, shakes));
    }

    IEnumerator ShakeCamera(float intensity, int shakes)
    {
        for (int i = 0; i < shakes; i++)
        {
            transform.localPosition = new Vector3(Random.Range(-intensity, intensity), Random.Range(-intensity, intensity));
            yield return null;
            transform.localPosition = Vector3.zero;
        }
    }
}

