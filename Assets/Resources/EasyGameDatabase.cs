using System;
using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

[CreateAssetMenu(menuName = "Game Database/Easy Game")]
public class EasyGameDatabase : ScriptableObject
{
    public TextAsset csv;
    public List<EasyGameListResources> resources = new List<EasyGameListResources>();
    
    
    public void GenerationList()
    {
        resources = EasyGameCsvParse.GenerationList(csv);
    }
}

[Serializable]
public class EasyGameListResources
{
    public List<EasyGameResources> resources = new List<EasyGameResources>();
}

[Serializable]
public class EasyGameResources
{
    public RewardTypeTemplate rewardTypeTemplate;
    public int countReward;
    public string timer;
    public float weight;
}

[CustomEditor(typeof(EasyGameDatabase))]
public class CvsEasyEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        EasyGameDatabase csv = (EasyGameDatabase)target;

        if (GUILayout.Button("Generate assets"))
        {
            csv.GenerationList();
        }
    }
}