using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Database/Mini Game")]
public class GameRewardsTemplate : ScriptableObject
{
    public TextAsset csv;

    public List<TierList> _tierLists = new List<TierList>();

    private Dictionary<int, List<ListAllResources>> _resourcesComponents = new Dictionary<int, List<ListAllResources>>();
    
    /// <summary>
    /// Возвращает рандомный лист с НАГРАДАМИ. 
    /// </summary>
    public List<ResourceTemplate> GetReward(int tier, int buildingLevel)
    {
        List<ResourceTemplate> resourceReward =
            _resourcesComponents[tier][UnityEngine.Random.Range(0, _resourcesComponents[tier].Count)].allResources[buildingLevel].resourcesAll;

        return resourceReward;
    }
    
    
    /// <summary>
    /// Возвращает лист с НАГРАДАМИ рандомно в зависимости от веса элементов. 
    /// </summary>
    public List<ResourceTemplate> GetRewardWithWeight(int tier, int buildingLevel)
    {
        var weights = new List<ListAllResources>();

        var nums = new List<float>();
        
        foreach (var item in _resourcesComponents[tier])
        {
            weights.Add(item);
        }

        var templates = weights.GetWeighedRandom(value => value.weight).allResources[buildingLevel].resourcesAll;

        return templates;
    }

    
    /// <summary>
    /// Используется исключительно для колеса фортуны. Логично, что возвращает параметры СЕКЦИЯ и лист с НАГРАДАМИ. 
    /// </summary>
    public (int section, List<ResourceTemplate> resources) GetRewardForWheelFortune(int tier, int buildingLevel)
    {
        var randomComponents = UnityEngine.Random.Range(0, _resourcesComponents[tier].Count);

        var section = _resourcesComponents[tier][randomComponents].allResources[buildingLevel].resourcesAll[0]
            .countReward;
        
        List<ResourceTemplate> resourceReward =
            _resourcesComponents[tier][randomComponents].allResources[buildingLevel].resourcesAll;

        return (section, resourceReward);
    }
    
    

    public void GenerationList()
    {
        _tierLists = MiniGameCsvParse.GenerationList(csv);

        _resourcesComponents.Clear();
        
        foreach (var tier in _tierLists)
        {
            _resourcesComponents.Add(tier.tierNumber, tier.listAllResources);
        }
        
        // GetRewardWithWeight(1, 1);
        // GetRewardForWheelFortune(1, 1);
    }
}

public static class NewExtansion
{
    public static T GetWeighedRandom<T>(this IEnumerable<T> collection, Func<T, double> weight)
    {
        var array = collection.ToArray();
        double sum = array.Sum(value => weight(value));
        double random = UnityEngine.Random.Range(0f, (float) sum);
        foreach (var element in array)
        {
            random -= weight(element);
            if (random <= 0)
            {
                return element;
            }
        }
        throw new Exception();
    }
}

[Serializable]
public class TierList
{
    public int tierNumber;
    public List<ListAllResources> listAllResources = new List<ListAllResources>();
}

[Serializable]
public class ListAllResources
{
    public float weight;
    public List<ListResources> allResources = new List<ListResources>();
}

[Serializable]
public class ListResources
{
    public List<ResourceTemplate> resourcesAll = new List<ResourceTemplate>();
}


[CustomEditor(typeof(GameRewardsTemplate))]
public class CvsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameRewardsTemplate csv = (GameRewardsTemplate)target;

        if (GUILayout.Button("Generate assets"))
        {
            csv.GenerationList();
        }
    }
}
