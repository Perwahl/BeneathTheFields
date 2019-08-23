using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityTemplateProjects;

public class DungeonCell : MonoBehaviour
{
    public enum CellType {standard =0, startCell =1, endCell=2, branchEnd=3}
    public CellType cellType;

    public CellConnection[] connectionPoints;
    public Transform[] monsterSpawnPoints;

    public CapsuleCollider boundsCollider;
    public DungeonCellContent content;
    public bool start;
    public bool blocked;

    public CellConnection RandomConnection()
    {
        var freeConnections = connectionPoints.Where(c => !c.blocked && !c.connected).ToArray();
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

    internal void Populate(DungeonFloorBlueprint floor, CellType type)
    {
        cellType = type;
        content = floor.GetCellContent(type);        
    }

    public void SpawnMonsters(SimpleCameraController player)
    {
        List<Transform> freeSpawnPoints = new List<Transform>();
        content.monsters = new List<Monster>();
        freeSpawnPoints.AddRange(monsterSpawnPoints);

        foreach(MonsterBlueprint monster in content.monsterBlueprints)
        {
            var randomSpawn = freeSpawnPoints[UnityEngine.Random.Range(0, freeSpawnPoints.Count)];
            freeSpawnPoints.Remove(randomSpawn);
            var m = Instantiate(monster.monsterPrefab, randomSpawn);
            content.monsters.Add(m);
            m.player = player.transform;
        }
    }

    internal IEnumerator Connect(DungeonCell foreignCell)
    {
        var localConnectPoint = RandomConnection();
        var foreignConnectPoint = foreignCell.RandomConnection();
        if (foreignCell.blocked) yield break;

        Vector3 localConnectDirection = localConnectPoint.transform.position - transform.position;
        Vector3 foreignConnectDirection = foreignConnectPoint.transform.position - foreignCell.transform.position;

        transform.position = foreignCell.transform.position;
        float angleDiff = Vector3.SignedAngle(localConnectDirection, foreignConnectDirection, transform.up);

        // Debug.Log(angleDiff);

        transform.Rotate(transform.up, angleDiff);
        transform.position = foreignConnectPoint.transform.position + (foreignConnectDirection.normalized * localConnectDirection.magnitude);
        var heightDiff = foreignConnectPoint.HeightDiff() - localConnectPoint.HeightDiff();
        transform.position = new Vector3(transform.position.x, transform.position.y + heightDiff, transform.position.z);

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

        if (hitColliders.Length > 2)
        {
            Debug.Log(gameObject.name + " not valid");
                        
            return false;
        }

        return true;
    }
}
