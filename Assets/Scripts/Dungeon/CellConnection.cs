﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CellConnection : MonoBehaviour
{
    public Transform heightOffset;
    public Transform cameraPosition;
    
    public bool connected;
    public bool blocked;
    public CellConnection connectedTo;
    public DungeonCell owner;
    public bool isCurrent;

    internal float HeightDiff()
    {
        return heightOffset.localPosition.y*0.1f;
    }
}
