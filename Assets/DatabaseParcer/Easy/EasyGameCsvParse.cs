using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EasyGameCsvParse : ParseTemplate 
{
    public static List<EasyGameListResources> GenerationList(TextAsset csv)
    {

        List<string> allLines = csv.text.Split('\n').ToList();

        var withWeight = allLines[0].Contains("Weight");
        var withTimer = allLines[0].Contains("Timer");
        var withType = allLines[0].Contains("Type");

        int timerIndex = 0;
        string timer = "00:00:00";
        
        if (withTimer)
        {
            var a = allLines[0].Split(',');

            for (int i = 0; i < a.Length; i++)
            {
                a[i].Replace("\"", String.Empty);
                
                Debug.Log(a[i]);

                if (a[i] == "Timer")
                {
                    timerIndex = i;
                }
            }
            
        }
        
        
        allLines.RemoveAt(0);

        var listResources = new List<EasyGameListResources>();
        
        foreach (var item in allLines)
        {
            List<string> splitCell = item.Split(',').ToList();

            if (withType)
            {
                splitCell.RemoveAt(0);
            }

            

            string weight = "0";
                
            if (withWeight)
            {
                weight = splitCell[splitCell.Count - 1];
                
                splitCell.RemoveAt(splitCell.Count - 1);
            }
            
            if (withTimer)
            {
                timer = splitCell[timerIndex];
                splitCell.RemoveAt(timerIndex);
            }

            foreach (var cell in splitCell)
            {
                var resourceTemplates = new EasyGameListResources();
                
                if (cell.Contains('&'))
                {
                    List<string> resource = cell.Split('&').ToList();
                    foreach (var nums in resource)
                    {
                            
                        var dataCell = GetDataCell(nums);
                            
                        var tempTemplate = GetCellType(dataCell.name);

                        var resourceTemplate = new EasyGameResources();
                        resourceTemplate.rewardTypeTemplate = tempTemplate;
                        resourceTemplate.weight = float.Parse(weight.Replace('.', ','));
                        resourceTemplate.timer = timer;

                        resourceTemplate.countReward = dataCell.count;  //Ресурс в список ресурсов
                        resourceTemplates.resources.Add(resourceTemplate);
                    }
                }
                else
                {
                    var dataCell = GetDataCell(cell);

                    var tempTemplate = GetCellType(dataCell.name);

                    var resourceTemplate = new EasyGameResources();
                    resourceTemplate.rewardTypeTemplate = tempTemplate;
                    resourceTemplate.weight = float.Parse(weight.Replace('.', ','));
                    resourceTemplate.timer = timer;

                    resourceTemplate.countReward = dataCell.count; //Добавили данные ресурса
                    resourceTemplates.resources.Add(resourceTemplate); //Ресурс положили в список ресурсов
                        
                }
                listResources.Add(resourceTemplates);

                
            }
        }
        return listResources;
    }
}
