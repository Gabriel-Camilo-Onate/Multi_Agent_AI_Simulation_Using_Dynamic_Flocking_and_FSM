using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq; //esto lo uso para el ToList() y el order by, que a su vez me permite hacer lo que quiero hacer
                   // de manera más prolija
public abstract class Node : MonoBehaviour
{
    [SerializeField] protected Dictionary<Node,bool> _neighbors = new Dictionary<Node, bool>();
    [SerializeField] protected LayerMask _nodeLayer;

    public int cost = 1;

    protected virtual void Start()
    {
        if (_neighbors.Count == 0)
        {
            Debug.Log("This node, has no neighbors");
        }
    }

    public virtual Dictionary<Node,bool> GetNeighbors()
    {
        return _neighbors;
    }
    public virtual List<Node> GetClosestNeighbors()
    {
        List<Node> closestNeighbors = new List<Node>();

        List<(Node node, float distance)> accessibleNeighbors = new List<(Node, float)>();

        foreach (var neighbor in _neighbors)
        {
            float distance = Vector3.Distance(transform.position, neighbor.Key.transform.position);
            accessibleNeighbors.Add((neighbor.Key, distance));
        }

        var sortedNeighbors = accessibleNeighbors.OrderBy(n => n.distance);

        foreach (var neighbor in sortedNeighbors)
        {
            closestNeighbors.Add(neighbor.node);
        }

        return closestNeighbors;
    }
    public void CheckNeighbors()
    {
        foreach (var neighbor in _neighbors.Keys.ToList()) //el ToList lo uso para poder modificar el diccionario
                                                           //mientras recorro los neighbors
        {
            if (Physics.Raycast(transform.position, neighbor.transform.position-transform.position, out RaycastHit hit, Mathf.Infinity, _nodeLayer, QueryTriggerInteraction.Collide))
            {
                _neighbors[neighbor] = false;
            }
            else
            {
                _neighbors[neighbor] = true;
            }
        }
    }
}
