using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ParseTemplate
{
    private static RewardTypeTemplate[] templates;

    protected static RewardTypeTemplate GetCellType(string name)
    {
        templates = Resources.LoadAll<RewardTypeTemplate>("");

        return FindTemplate(name.Replace(" ", String.Empty));
    }

    protected static RewardTypeTemplate FindTemplate(string name)
    {
        var num = 0;

        foreach (var item in templates)
        {
            if (item.eType.ToString() == name)
            {
                return item;
            }

            num++;
        }
        
        Debug.Log(num);
        
        return null;
    }
    
    protected static (string name, int count) GetDataCell(string cell)
    {
        var divider = cell.IndexOf(':');


        var nameRes = cell.Substring(0, divider + 1).Replace("\"", String.Empty).Replace(":", String.Empty); //Выбираем имя и удаляем ненужные символы
        var countRes = cell.Substring(divider + 1).Replace("\"", String.Empty).Replace(":", String.Empty).Replace(" ", String.Empty); //Выбираем количество и удаляем ненужные символы
        
        Debug.Log(nameRes +" nameRes " + countRes);

        return (nameRes, int.Parse(countRes));
    }

    protected static string GetName(TextAsset file)
    {
        var fileName = file.name;
        var divider = fileName.IndexOf('-');
        fileName = fileName.Substring(divider + 1).Replace(" ", String.Empty);
        
        return fileName;
    }
}
