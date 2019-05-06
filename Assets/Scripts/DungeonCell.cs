using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class DungeonCell : MonoBehaviour
{
    public CellConnection[] connectionPoints;
    
    public CapsuleCollider boundsCollider;
    public bool start;
    public bool blocked;

    private void Update()
    {
      //  angle = Vector3.Angle(transform.forward, connectionPoints[0].localPosition);

    }

    public CellConnection RandomConnection()
    {
        var freeConnections = connectionPoints.Where(c => !c.blocked && !c.connected ).ToArray();
        if (freeConnections.Length > 0)
        {
            var con = freeConnections[UnityEngine.Random.Range(0, freeConnections.Length)];

            return con;
        }
        else
        {
            Debug.Log("No valid connections " + gameObject.name);
            blocked = true;
            return null;
        }
    }

    internal IEnumerator Connect(DungeonCell foreignCell)
    {       
        var localConnectPoint = RandomConnection();
        var foreignConnectPoint = foreignCell.RandomConnection();
        if (foreignCell.blocked) yield break;

        Vector3 localConnectDirection = localConnectPoint.transform.position-transform.position;
        Vector3 foreignConnectDirection = foreignConnectPoint.transform.position - foreignCell.transform.position;

        transform.position = foreignCell.transform.position;
        float angleDiff = Vector3.SignedAngle(localConnectDirection, foreignConnectDirection, transform.up);
        
       // Debug.Log(angleDiff);
                
        transform.Rotate(transform.up, angleDiff); 
        transform.position = foreignConnectPoint.transform.position + (foreignConnectDirection.normalized*localConnectDirection.magnitude);
        var heightDiff = foreignConnectPoint.HeightDiff()-localConnectPoint.HeightDiff();
        transform.position = new Vector3(transform.position.x, transform.position.y+heightDiff, transform.position.z);

        transform.Rotate(transform.up, 180f);

        yield return new WaitForSeconds(0.1f);

        if (ValidConnection())
        {
            localConnectPoint.gameObject.SetActive(true);
            localConnectPoint.connected = true;
            localConnectPoint.connectedTo = foreignConnectPoint;

            foreignConnectPoint.gameObject.SetActive(true);
            foreignConnectPoint.connected = true;
            foreignConnectPoint.connectedTo = localConnectPoint;
        }
         else
        {
            foreignConnectPoint.gameObject.SetActive(true);
            foreignConnectPoint.blocked = true;            
            foreignConnectPoint.GetComponent<Renderer>().material.SetColor("_BaseColor", Color.red);
            yield return Connect(foreignCell);
        }
    }

    private bool ValidConnection()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, 6f);

        if(hitColliders.Length > 2)
        {
            Debug.Log(gameObject.name + " not valid");

            foreach(Collider c in hitColliders)
            {
                Debug.Log(c.gameObject.name);
            }
            return false;
        }

        //foreach(Collider c in hitColliders)
        //{
        //    Debug.Log(c.gameObject.name);
        //}

        
        return true;
    }
}
