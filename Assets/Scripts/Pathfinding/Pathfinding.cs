using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pathfinding : MonoBehaviour
{
    public List<Node> ConstructPathToNearPlace(Node startingNode, Node goalNode)  //acá hago algo similar al bfs,
                                                                                  //en el sentido de ir chequeando los vecinos inmediatos
                                                                                  //empezando siempre por el más cercano
                                                                                  //y chequeando si puedo hacer un camino con theta* hasta el
                                                                                  //devolviendo así el camino posible al punto más cercano
                                                                                  //de la posición objetivo original
                                                                                  //y null si no es posible acceder a ningún nodo
    {
        if (startingNode==null || goalNode == null) return null;

        Queue<Node> frontier = new Queue<Node>();
        HashSet<Node> visited = new HashSet<Node>();
        frontier.Enqueue(goalNode);
        visited.Add(goalNode);
        while (frontier.Count > 0)
        {
            Node current = frontier.Dequeue();

            foreach (var next in current.GetClosestNeighbors())
            {
                if (visited.Contains(next)) continue;
                visited.Add(next);
                var path = ConstructPathThetaStar(startingNode, next);
                if (path != null)
                {
                    return path;
                }
                else
                {
                    frontier.Enqueue(next);
                }
            }
       }
        return null;
    }
    public List<Node> ConstructPathAStar(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return null;

        PriorityQueue frontier = new PriorityQueue();
        frontier.Put(startingNode, 0);

        Dictionary<Node, Node> cameFrom = new Dictionary<Node, Node>();
        cameFrom.Add(startingNode, null);

        Dictionary<Node, float> costSoFar = new Dictionary<Node, float>();
        costSoFar.Add(startingNode, 0);

        while (frontier.Count > 0)
        {
            Node current = frontier.Get();

            if (current == goalNode)
            {
                List<Node> path = new List<Node>();
                Node nodeToAdd = current;

                while (nodeToAdd != null)
                {
                    path.Add(nodeToAdd);
                    nodeToAdd = cameFrom[nodeToAdd];
                }
                return path;
            }

            foreach (var next in current.GetNeighbors())
            {
                if(!InSight(current.transform.position,next.Key.transform.position))continue;
                float dist = Vector3.Distance(goalNode.transform.position, next.Key.transform.position);

                float newCost = costSoFar[current] + next.Key.cost;
                float priority = newCost + dist;
                if (!cameFrom.ContainsKey(next.Key))
                {
                    frontier.Put(next.Key, priority);
                    cameFrom.Add(next.Key, current);
                    costSoFar.Add(next.Key, newCost);
                }
                else
                {
                    if (newCost < costSoFar[next.Key])
                    {
                        frontier.Put(next.Key, priority);
                        cameFrom[next.Key] = current;
                        costSoFar[next.Key] = newCost;
                    }
                }
            }
        }
        return null;
    }
    public List<Node> ConstructPathThetaStar(Node startingNode, Node goalNode)
    {
        if (startingNode == null || goalNode == null) return null;

        List<Node> path = ConstructPathAStar(startingNode, goalNode);
        if (path == null) return null;

        int current = 0;
        int nextNext = current + 2;
        while (nextNext < path.Count)
        {
            if (InSight(path[current].transform.position, path[nextNext].transform.position))
            {
                path.RemoveAt(current + 1);
            }
            else
            {
                current++;
                nextNext++;
            }
        }
        path.Reverse();
        return path;
    }
    public bool InSight(Vector3 start, Vector3 end)
    {
        return !Physics.Raycast(start, end - start, Vector3.Distance(start, end), GameManager.instance.wallLayer);
    }

}
