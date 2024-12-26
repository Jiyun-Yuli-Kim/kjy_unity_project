using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class DialogueLoader : MonoBehaviour
{
    private const string KindDialogue =
        "https://docs.google.com/spreadsheets/d/13v8TbGoXz7ywTqHxwMzNohhG4DBODOhcLutcQ3UUJLk/edit?gid=0#gid=0";

    private const string IdolDialogue =
        "https://docs.google.com/spreadsheets/d/1DS6qp310IcUBQu8Mw01p27nPV_-LGkUwwxcrXMH3nk0/edit?gid=0#gid=0";
        
    public string[,] DialogueData { get; private set; }

    private void Awake()
    {
        DialogueData = LoadCSV();

        for (int i = 1; i < DialogueData.GetLength(0); i++)
        {
            for (int j = 0; j < DialogueData.GetLength(1); j++)
            {
                Debug.Log(DialogueData[i, j]);
            }
        }
    }

    public string[,] LoadCSV()
    {
        string path = Application.dataPath + "/CSV/DataTable";

        if (!Directory.Exists(path))
        {
            Debug.LogError("경로가 없습니다.");
            return null;
        }

        if (!File.Exists(path + "/DialogueCSV.csv"))
        {
            Debug.LogError("파일이 없습니다.");
            return null;
        }

        string file = File.ReadAllText(path + "/DialogueCSV.csv");

        // 행별로 나눔
        string[] lines = file.Split('\n');

        string[,] DialogueTable = new string[lines.Length, lines[0].Split(',').Length];

        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            for (int j = 0; j < values.Length; j++)
            {
                DialogueTable[i, j] = values[j];
            }
        }

        return DialogueTable;
    }
}
