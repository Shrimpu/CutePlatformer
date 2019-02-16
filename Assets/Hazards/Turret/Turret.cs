using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Turret : MonoBehaviour
{
    public bool isStatic;
    public float staticRotation;
    public float fireRate = 1f;
    public float range = 50f;
    public float gunDistanceFromBody = 0.4f;
    public GameObject projectilePrefab;
    public Transform Gun;
    public Transform firePoint;
    public LayerMask raycastObstructions;

    float nextShot;
    bool hasTarget;
    GameObject[] players = new GameObject[0];
    Transform target;

    private void Start()
    {
        nextShot = Time.time + (1 / fireRate);
        PlayerManager.PlayersInSceneChanged += UpdateTargets;
        OnPlayerDeath.PlayerKilled += CheckIfTargetKilled;
    }

    private void Update()
    {
        if (!isStatic)
        {
            if (target != null && hasTarget)
            {
                Gun.transform.localPosition = TargetPosition();
                Gun.transform.rotation = Quaternion.Euler(0, 0, ZRotation(target) * Mathf.Rad2Deg);
                Vector2 direction = target.position - transform.position;
                bool hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, raycastObstructions);
                if (hit)
                {
                    target = null;
                    hasTarget = false;
                    SearchClosestPlayer();
                }
            }
            else
            {
                SearchClosestPlayer();
            }
        }
        else if (isStatic)
        {
            Gun.transform.localPosition = new Vector2(Mathf.Sin(-staticRotation * Mathf.Deg2Rad), Mathf.Cos(-staticRotation * Mathf.Deg2Rad)) * gunDistanceFromBody;
        }
        Shoot();
    }

    void Shoot()
    {
        if (!isStatic)
        {
            if (target != null)
            {
                if (CalculateNextShot())
                {
                    SpawnProjectile();
                }
            }
            else
                nextShot = Time.time + (1 / fireRate);
        }
        else
        {
            if (CalculateNextShot())
            {
                SpawnProjectile();
            }
        }
    }

    bool CalculateNextShot()
    {
        if (nextShot < Time.time)
        {
            nextShot = Time.time + (1 / fireRate);
            return true;
        }
        return false;
    }

    void SpawnProjectile()
    {
        if (projectilePrefab != null && firePoint != null && !isStatic)
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, ZRotation(target) * Mathf.Rad2Deg));
        else if (projectilePrefab != null && firePoint != null && isStatic)
            Instantiate(projectilePrefab, firePoint.position, Quaternion.Euler(0, 0, ZRotation(Gun) * Mathf.Rad2Deg));
    }

    void SearchClosestPlayer()
    {
        float distance = range;
        Transform targetCandidate = null;

        foreach (GameObject player in players)
        {
            if (player.activeInHierarchy == true)
            {
                Vector2 direction = player.transform.position - transform.position;
                bool hit = Physics2D.Raycast(transform.position, direction, direction.magnitude, raycastObstructions);
                Debug.DrawRay(transform.position, direction, Color.red, 0.01f);
                if (!hit)
                {
                    if (direction.magnitude < distance)
                    {
                        distance = direction.magnitude;
                        targetCandidate = player.transform;
                    }
                }
            }
        }
        if (targetCandidate != null)
        {
            target = targetCandidate;
            hasTarget = true;
        }
    }

    void UpdateTargets()
    {
        players = GameObject.FindGameObjectsWithTag("Player");
        if (players.Length != PlayerManager.players)
        {
            players = null;
            UpdateTargets();
        }
    }

    void CheckIfTargetKilled(GameObject player)
    {
        if (player.transform == target)
        {
            target = null;
            hasTarget = false;
            SearchClosestPlayer();
        }
    }

    public float ZRotation(Transform targetPos)
    {
        Vector2 dir = targetPos.position - transform.position;
        Quaternion angle = Quaternion.FromToRotation(Vector2.up, dir);
        Vector3 _angle = angle.ToEulerAngles(); // this green line can fuck right off. it works perfectly
        return _angle.z;
    }

    public Vector2 TargetPosition()
    {
        float angle = ZRotation(target);
        Vector2 pos = new Vector2(Mathf.Sin(-angle), Mathf.Cos(-angle)) * gunDistanceFromBody;
        return pos;
    }

    private void OnDrawGizmos()
    {
        if (target != null)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, target.position);
        }
    }
}
