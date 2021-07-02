using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameRewardsTemplate : ScriptableObject
{
    public int tierNumber;
    public List<TierList> tierResources = new List<TierList>();

    public void GetReward()
    {
        
    }
}

[Serializable]
public class TierList
{
    public float weight;
    public List<ResourcesAll> allResources = new List<ResourcesAll>();
}

[Serializable]
public class ResourcesAll
{
    public List<ResourceTemplate> resourcesAll = new List<ResourceTemplate>();
}
