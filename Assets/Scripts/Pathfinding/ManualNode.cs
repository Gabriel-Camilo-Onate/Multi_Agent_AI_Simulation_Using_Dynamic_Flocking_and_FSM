using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ManualNode : Node
{
    protected override void Start()
    {
        base.Start();
        if (_neighbors.Count==0)
        {
            Destroy(gameObject);
        }
    }
}
