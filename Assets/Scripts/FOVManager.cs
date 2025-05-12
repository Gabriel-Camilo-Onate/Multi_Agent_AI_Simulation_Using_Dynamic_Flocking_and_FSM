using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class FOVManager : MonoBehaviour
{
    public static FOVManager instance;
    public Dictionary<Entity,int>viewableEntities = new Dictionary<Entity, int>();

    private void Start()
    {
        if(instance==null)
        {
            instance = this;
        }
        else if(instance != this)
        {
            Destroy(gameObject);
        }
        
    }
    public void SuscribeToViewableEntities(int team, Entity entity)
    {
        viewableEntities.Add(entity,team);
    }
    public bool IsInFieldOfView(int team, float viewRadius, float viewAngle, Entity owner)
    {
        foreach (var entity in viewableEntities)
        {
            if (entity.Value == team)
                continue;
            Vector3 dir = entity.Key.transform.position - owner.transform.position;
            if (dir.magnitude > viewRadius) continue;

            if (Vector3.Angle(owner.transform.forward, dir) < viewAngle / 2)
            {
                if (!Physics.Raycast(owner.transform.position, dir, out RaycastHit hit, dir.magnitude, GameManager.instance.wallLayer))
                {
                    owner.SetTargetToAttack(entity.Key.transform,true);
                    return true;
                }
                else
                {
                    owner.SetTargetToAttack(entity.Key.transform,false);
                    return false;
                }
            }
            else
            {
                owner.SetTargetToAttack(entity.Key.transform, false);
                return false;
            }
        }
        return false;
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        foreach (var entity in viewableEntities)
        {
            if (entity.Key.gizmosOnOff)
            {
                if (entity.Key.IsMinion())
                {
                    Gizmos.DrawWireSphere(entity.Key.transform.position, GameManager.instance.minionViewRadius);
                    Vector3 lineA = GetVectorFromAngle(GameManager.instance.minionViewAngle  / 2 + entity.Key.transform.eulerAngles.y);
                    Vector3 lineB = GetVectorFromAngle(-GameManager.instance.minionViewAngle / 2 + entity.Key.transform.eulerAngles.y);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(entity.Key.transform.position, entity.Key.transform.position + lineA * GameManager.instance.minionViewRadius);
                    Gizmos.DrawLine(entity.Key.transform.position, entity.Key.transform.position + lineB * GameManager.instance.minionViewRadius);
                    Gizmos.color = Color.red;
                }
                else
                {
                    Gizmos.DrawWireSphere(entity.Key.transform.position, GameManager.instance.leaderViewRadius);
                    Vector3 lineA = GetVectorFromAngle(GameManager.instance.leaderViewAngle / 2 + entity.Key.transform.eulerAngles.y);
                    Vector3 lineB = GetVectorFromAngle(-GameManager.instance.leaderViewAngle / 2 + entity.Key.transform.eulerAngles.y);
                    Gizmos.color = Color.blue;
                    Gizmos.DrawLine(entity.Key.transform.position, entity.Key.transform.position + lineA * GameManager.instance.leaderViewRadius);
                    Gizmos.DrawLine(entity.Key.transform.position, entity.Key.transform.position + lineB * GameManager.instance.leaderViewRadius);
                    Gizmos.color = Color.red;
                }
            }
        }
    }
    Vector3 GetVectorFromAngle(float angle)
    {
        return new Vector3(Mathf.Sin(angle * Mathf.Deg2Rad), 0, Mathf.Cos(angle * Mathf.Rad2Deg));
    }
}
