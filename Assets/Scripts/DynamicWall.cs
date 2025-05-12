using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DynamicWall : MonoBehaviour
{
    public void ResizeWall()
    {
        Vector3 positionOne=transform.position;
        Vector3 positionTwo= transform.position;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, transform.right, out hit, 100, GameManager.instance.wallLayer))
        {
            positionOne = hit.point;
        }
        if (Physics.Raycast(transform.position, -transform.right, out hit, 100, GameManager.instance.wallLayer))
        {
            positionTwo = hit.point;
        }
        float distance = Vector3.Distance(positionOne, positionTwo);
        transform.localScale=new Vector3(transform.localScale.x*distance,3,1);
        transform.position = Vector3.Lerp(positionOne,positionTwo,0.5f);
    }
}
