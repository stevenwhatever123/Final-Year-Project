using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovableArea : MonoBehaviour
{
    [Tooltip("The Nodes making up the path")]
    public List<Transform> point = new List<Transform>();

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.cyan;
        for (int i = 0; i < point.Count; i++)
        {
            int nextIndex = i + 1;
            if(nextIndex >= point.Count)
            {
                nextIndex -= point.Count;
            }

            Gizmos.DrawLine(point[i].position, point[nextIndex].position);
            Gizmos.DrawSphere(point[i].position, 0.1f);
        }
    }

    public List<Transform> getPoint(){
        return point;
    }
}
