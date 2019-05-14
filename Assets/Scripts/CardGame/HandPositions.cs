using UnityEngine;
using System.Collections;
using TMPro;
using System.Collections.Generic;

[ExecuteInEditMode]
public class HandPositions : MonoBehaviour
{
    public BezierCurve vertexCurve;
    public Transform[] allPoints;
    public List<Transform> points;   

    private float offset;
    private int count;   
    
    //void OnRenderObject()
    //{
    //    SetPositions();
    //}

    public List<Transform> GetPositions(int count)
    {
        this.count = count;       
        var counter = 0;
        points.Clear();

        for (int i = 0; i < 10; i++)
        {
            if (counter < count)
            {
                points.Add(allPoints[i]);
                allPoints[i].gameObject.SetActive(true);
            }
            else
            {
                allPoints[i].gameObject.SetActive(false);
            }
            counter++;
        }

        int positionCount = points.Count;

        if (positionCount >= 0)
        {
            offset = 0.08f + ((12-positionCount)*0.01f);
            float range = (positionCount - 1) * offset;
            var t = (1 - range) / 2;

            for (int i = 0; i < positionCount; i++)
            {
                //  points[i].rotation = Quaternion.identity;
                  points[i].rotation = transform.rotation;
                points[i].position = vertexCurve.GetPoint(t);
                var vector = vertexCurve.GetDirection(t);
                var angle = Vector3.Angle(points[i].right, vector);

                if(i >= positionCount/2)
                {
                    angle *= -1;
                }

                points[i].Rotate(points[i].transform.forward, angle);
              //  points[i].Rotate(Vector3.forward, angle);

                t += offset;
            }
        }
        return points;
    }
}