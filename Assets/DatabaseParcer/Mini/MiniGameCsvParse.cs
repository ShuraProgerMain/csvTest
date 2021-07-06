using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;


    /// <summary>
    /// Парсит объекты для МИНИ-ИГР
    /// </summary>
public class MiniGameCsvParse : ParseTemplate
{
    public static List<TierList> GenerationList(TextAsset csv)
    {

        List<string> allLines = csv.text.Split('\n').ToList();

        var withWeight = allLines[0].Contains("Weight");
        
        allLines.RemoveAt(0);

        var currentTier = int.Parse(allLines[0].Substring(0, 1));
        var newTier = 0;

        var lists = new List<TierList>();
        
        for (int i = 0; i < 3; i++)
        {
            if (newTier > currentTier)
            {
                currentTier = newTier;
            }

            var tierList = new TierList();
            
            foreach (var item in allLines)
            {
                List<string> splitCell = item.Split(',').ToList();

                var temp = int.Parse(splitCell[0].Substring(0, 1));
                
                
                if (temp - currentTier == 1)
                {
                    newTier = temp;
                }
                
                if (temp != currentTier)
                {
                    continue;
                }
                
                splitCell.RemoveAt(0);

                string weight = "0";
                
                if (withWeight)
                {
                    weight = splitCell[splitCell.Count - 1];
                
                    splitCell.RemoveAt(splitCell.Count - 1);
                }
            
                var listTemplates = new ListAllResources();
            
                foreach (var cell in splitCell)
                {
                    var resourceTemplates = new ListResources();

                    if (cell.Contains('&'))
                    {
                        List<string> resource = cell.Split('&').ToList();

                        foreach (var nums in resource)
                        {
                            var dataCell = GetDataCell(nums);
                            
                            var tempTemplate = GetCellType(dataCell.name);

                            var resourceTemplate = new ResourceTemplate();
                            resourceTemplate.rewardTypeTemplate = tempTemplate;

                            resourceTemplate.countReward = dataCell.count;  //Ресурс в список ресурсов
                            resourceTemplates.resourcesAll.Add(resourceTemplate);
                        }
                    }
                    else
                    {
                        var dataCell = GetDataCell(cell);

                        var tempTemplate = GetCellType(dataCell.name);

                        var resourceTemplate = new ResourceTemplate();
                        resourceTemplate.rewardTypeTemplate = tempTemplate;

                        resourceTemplate.countReward = dataCell.count; //Добавили данные ресурса

                        resourceTemplates.resourcesAll.Add(resourceTemplate); //Ресурс положили в список ресурсов
                    }

                    listTemplates.weight = float.Parse(weight.Replace('.', ',')); //Устанавливаем значение веса если это значение есть
                    
                    listTemplates.allResources.Add(resourceTemplates); //Список ресурсов положили в тирлист
                }
                
                tierList.listAllResources.Add(listTemplates);
                tierList.tierNumber = currentTier;
            }
            
            lists.Add(tierList);
        }

        return lists;
    }

    
}
