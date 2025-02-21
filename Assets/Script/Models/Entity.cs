using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class Entity
{
    [Header("Name")]
    public string name;

    [Header("health")]
    public int currentHealt;

    public int maxHealt;
}
