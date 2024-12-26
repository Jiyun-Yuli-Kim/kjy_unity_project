using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Networking;

public class DialogueLoader : MonoBehaviour
{
    private const string KindDialogue =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vR8uX0llujfHBKqUOCZ92p80anVPJEmy9HNbHRY5buq3ICGfkflCrZvvJMj6yy6etR6dDfayBMg56N1/pub?gid=0&single=true&output=csv";

    private const string IdolDialogue =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vT14RTV0DSuTbon86CiecSde7DullmFy3g9reMeRJCoyhvakMuQgtfVwagq8N47fMl1neSdooXMpNsK/pub?gid=0&single=true&output=csv";
        
    public string[,] DialogueData { get; private set; }

    private void Awake()
    {
        StartCoroutine(DownLoadRoutine(KindDialogue));
        ShowCSVData(DialogueData);
    }

    IEnumerator DownLoadRoutine(string urlPath)
    {
        UnityWebRequest request = UnityWebRequest.Get(urlPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string recievedData = request.downloadHandler.text;
            DialogueData = ProcessCSV(recievedData);
        }

        else
        {
            Debug.Log("데이터를 로드할 수 없습니다");
        }
        
    }

    private string[,] ProcessCSV(string data)
    {
        // 행별로 나눔
        string[] lines = data.Split('\n');
    
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

    private void ShowCSVData(string[,] dataTable)
    {
        for (int i = 1; i < dataTable.GetLength(0); i++)
        {
            for (int j = 0; j < dataTable.GetLength(1); j++)
            {
                Debug.Log(dataTable[i, j]);
            }
        }
    }
}

// string path = Application.dataPath + "/CSV/DataTable";
//     
// if (!Directory.Exists(path))
// {
//     Debug.LogError("경로가 없습니다.");
//     return null;
// }
//     
// if (!File.Exists(path + "/DialogueCSV.csv"))
// {
//     Debug.LogError("파일이 없습니다.");
//     return null;
// }