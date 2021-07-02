using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEditor;
using UnityEngine;

public class CsvParse : MonoBehaviour
{
    public TextAsset _csv;

    private string[] _resourcesName = {"Glod", "Metall", "Wood", "DragonTooth", "Stone", "JettonForMill", "JettonForMoles"};
    private void Start()
    {
        GenerationList();
    }

    public void GenerationList()
    {
        var fileName = GetName(_csv);

        
        
        List<string> allLines = _csv.text.Split('\n').ToList();

        allLines.RemoveAt(0);
        
        
        var currentTier = int.Parse(allLines[0].Substring(0, 1));
        var newTier = 0;
        
        for (int i = 0; i < 3; i++)
        {
            if (newTier > currentTier)
            {
                currentTier = newTier;
                // continue;
            }
            
            var game = ScriptableObject.CreateInstance<GameRewardsTemplate>();
            game.tierNumber = currentTier;
            
            
            foreach (var item in allLines)
            {
                List<string> splitCell = item.Split(',').ToList();

                var temp = int.Parse(splitCell[0].Substring(0, 1));
                
                
                if (temp - currentTier == 1)
                {
                    newTier = temp;
                    Debug.Log("nt" + newTier);
                }
                
                if (temp != currentTier)
                {
                    continue;
                }
                
                splitCell.RemoveAt(0);

                var weight = splitCell[splitCell.Count - 1];
                
                splitCell.RemoveAt(splitCell.Count - 1);
            
                var listTemplates = new TierList();
            
                foreach (var cell in splitCell)
                {
                    var resourceTemplates = new ResourcesAll();

                    if (cell.Contains('&'))
                    {
                        List<string> resource = cell.Split('&').ToList();

                        foreach (var nums in resource)
                        {
                            var dataCell = GetDataCell(nums);
                            
                            var resourceTemplate = GetCellType(dataCell.name);

                            resourceTemplate.countReward = dataCell.count;  //Ресурс в список ресурсов
                            resourceTemplates.resourcesAll.Add(resourceTemplate);
                        }
                    }
                    else
                    {
                        var dataCell = GetDataCell(cell);

                        var resourceTemplate = GetCellType(dataCell.name);

                        resourceTemplate.countReward = dataCell.count; //Добавили данные ресурса

                        resourceTemplates.resourcesAll.Add(resourceTemplate); //Ресурс положили в список ресурсов
                    }

                    Debug.Log(float.Parse(weight.Replace('.', ',')) + " parce");
                    listTemplates.weight = float.Parse(weight.Replace('.', ',')); //Устанавливаем значение веса если это значение есть
                    
                    listTemplates.allResources.Add(resourceTemplates); //Список ресурсов положили в тирлист
                }
                
                game.tierResources.Add(listTemplates); //Тирлист положили в список с тирлистами
            }
            
            AssetDatabase.CreateAsset(game, $"Assets/Resources/Games/{fileName}_{currentTier}_Tier_Reward.asset");

        }
    }

    private ResourceTemplate GetCellType(string name)
    {
        var resourceTemplate = new ResourceTemplate();
        
        switch (name)
        {
            case "Glod":
                resourceTemplate.resourceType = TypeResource.Gold;
                break;
            case "Metall":
                resourceTemplate.resourceType = TypeResource.Metall;
                break;
            case "Wood":
                resourceTemplate.resourceType = TypeResource.Wood;
                break;
            case "Stone":
                resourceTemplate.resourceType = TypeResource.Stone;
                break;
            case "DragonTooth":
                resourceTemplate.resourceType = TypeResource.DragonTooth;
                break;
            case "JettonForMill":
                resourceTemplate.resourceType = TypeResource.JettonForMill;
                break;
            case "JettonForMoles":
                resourceTemplate.resourceType = TypeResource.JettonForMoles;
                break;
        }

        return resourceTemplate;
    }
    
    private (string name, int count) GetDataCell(string cell)
    {
        var divider = cell.IndexOf(':');

        var nameRes = cell.Substring(0, divider + 1).Replace("\"", String.Empty).Replace(":", String.Empty); //Выбираем имя и удаляем ненужные символы
        var countRes = cell.Substring(divider + 1).Replace("\"", String.Empty).Replace(":", String.Empty).Replace(" ", String.Empty); //Выбираем количество и удаляем ненужные символы
        
        return (nameRes, int.Parse(countRes));
    }

    private string GetName(TextAsset file)
    {
        var fileName = file.name;
        var divider = fileName.IndexOf('-');
        fileName = fileName.Substring(divider + 1).Replace(" ", String.Empty);
        
        return fileName;
    }
}

[CustomEditor(typeof(CsvParse))]
public class CvsEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        CsvParse csv = (CsvParse)target;

        if (GUILayout.Button("Generate assets"))
        {
            csv.GenerationList();
        }
    }
}
