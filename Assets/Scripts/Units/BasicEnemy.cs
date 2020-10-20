﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PolyNav;

namespace BioTower.Units
{
[SelectionBase]
public class BasicEnemy : Unit
{
    [SerializeField] private Transform target;
    [SerializeField] private PolyNavAgent agent;

    private void Awake()
    {
        agent = GetComponent<PolyNavAgent>();
    }
}
}
