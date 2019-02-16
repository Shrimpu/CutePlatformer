using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Thwomp : MonoBehaviour
{
    enum ActionState
    {
        Standby,
        Attacking,
        Recovering
    }

    public bool constantLoop;
    public float fallSpeed = 0.6f;
    public float ascendSpeed = 0.2f;
    public float cooldownTime;
    [Space]
    public LayerMask visibilityMask;
    public LayerMask playerMask;
    public LayerMask groundMask;

    public BoxCollider2D roof;

    Vector2 roofDimensions;

    ActionState state = ActionState.Standby;

    private void Start()
    {
        roofDimensions = roof.size;
    }

    void Update()
    {
        if (state == ActionState.Standby)
        {
            if (!constantLoop)
            {
                RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, visibilityMask);
                if (hit.collider.CompareTag("PlayerCollider"))
                {
                    StartCoroutine(Attack());
                }
            }
            else
            {
                StartCoroutine(Attack());
            }
        }
    }

    void MovePassangers()
    {
        HashSet<Transform> movedPassangers = new HashSet<Transform>();

        for (int i = -1; i < 2; i++)
        {
            Vector2 origin = transform.position + new Vector3(i * roofDimensions.x / 2, roofDimensions.y + roof.offset.y);
            RaycastHit2D[] hit = Physics2D.RaycastAll(origin, Vector3.up, 0.2f, playerMask);
            Debug.DrawLine(origin, origin + (Vector2.up * 0.1f), Color.blue);

            for (int j = 0; j < hit.Length; j++)
            {
                if (hit[j].collider.CompareTag("PlayerCollider"))
                {
                    if (!movedPassangers.Contains(hit[j].transform))
                    {
                        hit[j].transform.Translate(new Vector3(0, ascendSpeed));
                        movedPassangers.Add(hit[j].transform);
                    }
                }
            }
        }
    }

    IEnumerator Attack()
    {
        state = ActionState.Attacking;

        Vector2 startPos = transform.position;
        Vector2 endPos;

        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 35f, groundMask);
        if (hit)
        {
            endPos = hit.point;
        }
        else
        {
            endPos = -transform.up * 35f;
        }

        bool destinationReached = false;
        while (!destinationReached)
        {
            destinationReached = Move(endPos, fallSpeed);
            if (!hit && destinationReached)
            {
                Destroy(gameObject); // it fell of into the void lmao
                yield break; // safety
            }
            yield return null;
        }
        state = ActionState.Recovering;

        destinationReached = false;
        while (!destinationReached)
        {
            destinationReached = Move(startPos, ascendSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(cooldownTime);
        state = ActionState.Standby;
    }

    bool Move(Vector2 destination, float speed)
    {
        Vector2 nextPos = Vector2.MoveTowards(transform.position, destination, speed);

        MovePassangers();
        transform.position = nextPos;
        if (nextPos == destination)
            return true;
        return false;
    }

    private void OnDrawGizmos()
    {
        RaycastHit2D hit = Physics2D.Raycast(transform.position, Vector2.down, 100f, visibilityMask);
        if (hit.collider.CompareTag("PlayerCollider")) Gizmos.color = Color.red;
        else Gizmos.color = Color.white;

        if (hit && hit.distance > 0f)
            Gizmos.DrawLine(transform.position, hit.point);
        else
        {
            Gizmos.color = Color.red;
            Gizmos.DrawLine(transform.position - (Vector3.up * 0.6f) - (Vector3.right * 0.6f),
                transform.position + (Vector3.up * 0.6f) + (Vector3.right * 0.6f));
            Gizmos.DrawLine(transform.position - (Vector3.up * 0.6f) + (Vector3.right * 0.6f),
                transform.position + (Vector3.up * 0.6f) - (Vector3.right * 0.6f));
        }
    }
}
