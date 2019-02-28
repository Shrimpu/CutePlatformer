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
    [Space]
    public Texture leftTex;
    public Texture rightTex;

    BoxCollider2D col;
    Material mat;

    void Start()
    {
        col = GetComponent<BoxCollider2D>();
        mat = GetComponent<Renderer>().material;

        mat.mainTextureScale = new Vector2(transform.localScale.x, 1);
    }

    void FixedUpdate()
    {
        mat.mainTextureOffset += Vector2.left * (left ? -speed : speed) * Time.deltaTime;
        if (left)
            mat.mainTexture = leftTex;
        else
            mat.mainTexture = rightTex;
        MovePassangers();
    }

    void MovePassangers()
    {
        HashSet<Transform> movedPassangers = new HashSet<Transform>();

        for (int i = -1; i < 2; i++)
        {
            Collider2D[] players = Physics2D.OverlapAreaAll(col.bounds.max + Vector3.up * effectHeight, col.bounds.min + Vector3.up * col.bounds.size.y);

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
            Gizmos.DrawLine(col.bounds.max + Vector3.up * effectHeight, col.bounds.min + Vector3.up * col.bounds.size.y + Vector3.up * effectHeight);
            Gizmos.DrawLine(col.bounds.max + Vector3.up * effectHeight, col.bounds.max);
            Gizmos.DrawLine(col.bounds.min + Vector3.up * col.bounds.size.y, col.bounds.min + Vector3.up * col.bounds.size.y + Vector3.up * effectHeight);
        }
    }
}
