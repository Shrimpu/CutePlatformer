using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovingSawblade : MonoBehaviour
{
    public Transform sawblade;
    public float speed = 0.7f;
    public Transform[] checkpoints;
    [Space]
    public bool closedLoop;

    void Start()
    {
        if (checkpoints.Length != 0)
        {
            sawblade.position = checkpoints[0].position;
            StartCoroutine(MoveSawblade());
        }
    }

    IEnumerator MoveSawblade()
    {
        while (true)
        {
            if (closedLoop)
            {
                for (int i = 0; i < checkpoints.Length; i++)
                {
                    while (sawblade.position != checkpoints[i].position)
                    {
                        sawblade.position = Vector3.MoveTowards(sawblade.position, checkpoints[i].position, speed * Time.deltaTime);
                        yield return null;
                    }
                }
            }
            else
            {
                for (int i = 0; i < checkpoints.Length; i++)
                {
                    while (sawblade.position != checkpoints[i].position)
                    {
                        sawblade.position = Vector3.MoveTowards(sawblade.position, checkpoints[i].position, speed * Time.deltaTime);
                        yield return null;
                    }
                }
                for (int i = checkpoints.Length - 1; i >= 0; i--)
                {
                    while (sawblade.position != checkpoints[i].position)
                    {
                        sawblade.position = Vector3.MoveTowards(sawblade.position, checkpoints[i].position, speed * Time.deltaTime);
                        yield return null;
                    }
                }
            }
        }
    }
}
