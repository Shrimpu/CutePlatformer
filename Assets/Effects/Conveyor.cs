using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider2D))]
public class Conveyor : MonoBehaviour
{
    public float speed = 1.5f;
    public bool left;
    public float effectHeight = 0.3f;
    public LayerMask playerMask;

    BoxCollider2D col;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
    }

    void FixedUpdate()
    {
        MovePassangers();
    }

    void MovePassangers()
    {
        HashSet<Transform> movedPassangers = new HashSet<Transform>();

        for (int i = -1; i < 2; i++)
        {
            //Vector2 origin = transform.position + new Vector3(i * roofDimensions.x / 2, roofDimensions.y + roof.offset.y);
            //RaycastHit2D[] hit = Physics2D.RaycastAll(origin, Vector3.up, 0.2f, playerMask);
            //Debug.DrawLine(origin, origin + (Vector2.up * 0.1f), Color.blue);
            Collider2D[] players = Physics2D.OverlapAreaAll(col.bounds.max + Vector3.up * col.bounds.size.y * effectHeight, col.bounds.min + Vector3.up * col.bounds.size.y);

            for (int j = 0; j < players.Length; j++)
            {
                if (players[j].CompareTag("PlayerCollider"))
                {
                    if (!movedPassangers.Contains(players[j].transform))
                    {
                        players[j].transform.parent.transform.parent.transform.Translate(new Vector3(speed * Time.deltaTime * (left ? -1f : 1f), 0), Space.World);
                        movedPassangers.Add(players[j].transform);
                    }
                }
            }
        }
    }

    private void OnDrawGizmos()
    {
        if (col != null)
        {
            Gizmos.DrawLine(col.bounds.max + Vector3.up * col.bounds.size.y * effectHeight, col.bounds.min + Vector3.up * col.bounds.size.y + Vector3.up * effectHeight);
            Gizmos.DrawLine(col.bounds.max + Vector3.up * col.bounds.size.y * effectHeight, col.bounds.max);
            Gizmos.DrawLine(col.bounds.min + Vector3.up * col.bounds.size.y, col.bounds.min + Vector3.up * col.bounds.size.y + Vector3.up * effectHeight);

        }
    }
}
