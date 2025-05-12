using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AutoNode : Node
{
    protected override void Start()
    {
        if (_neighbors.Count==0)
        {
            if (!HasNeighbors())
            {
                Debug.Log("This node, has no neighbors");
                Destroy(gameObject);
            }
        }
        Grid.Instance.AddNode(this);
    }
    public bool HasNeighbors()
    {
        Vector3 direction;
        Quaternion directionRotation = new Quaternion();
        for (int i = 0; i < 360; i++)
        {
            directionRotation = Quaternion.Euler(0, i, 0);
            direction = directionRotation * transform.forward;
            if (Physics.Raycast(transform.position, direction, out RaycastHit hit, Mathf.Infinity, _nodeLayer, QueryTriggerInteraction.Collide))
            {
                AutoNode nodeFound = hit.collider.gameObject.GetComponent<AutoNode>();
                if (nodeFound!= null)
                {
                    if(!_neighbors.ContainsKey(nodeFound)&& GameManager.instance.pathfinding.InSight(transform.position, nodeFound.transform.position))
                        _neighbors.Add(nodeFound,true);
                }
            }
        }
        if (_neighbors.Count > 0)
            return true;
        else
            return false;
    }
}
