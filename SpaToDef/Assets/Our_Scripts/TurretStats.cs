﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurretStats : MonoBehaviour
{
    public int sellCost = 50;
    public float range;
    public bool isUpgraded;
    [HideInInspector]
    public TurretBlueprint blueprint;
}
