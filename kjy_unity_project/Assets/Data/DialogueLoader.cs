using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DialogueLoader : MonoBehaviour
{
    public string dialogueURL;
    public UnityEvent OnKindLoaded;
    public UnityEvent OnIdolLoaded;
    public UnityEvent OnCrankyLoaded;

    public const string KindDialogue =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vR8uX0llujfHBKqUOCZ92p80anVPJEmy9HNbHRY5buq3ICGfkflCrZvvJMj6yy6etR6dDfayBMg56N1/pub?gid=0&single=true&output=csv";
    public const string IdolDialogue =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vR8uX0llujfHBKqUOCZ92p80anVPJEmy9HNbHRY5buq3ICGfkflCrZvvJMj6yy6etR6dDfayBMg56N1/pub?gid=565515136&single=true&output=csv";

    public const string CrankyDialogue =
        "https://docs.google.com/spreadsheets/d/e/2PACX-1vR8uX0llujfHBKqUOCZ92p80anVPJEmy9HNbHRY5buq3ICGfkflCrZvvJMj6yy6etR6dDfayBMg56N1/pub?gid=104577247&single=true&output=csv";
   
    public string[,] DialogueData { get; private set; }

    private void Awake()
    {
        // StartLoad(KindDialogue);
    }

    public void StartLoad(string dialogueURL)
    {
        StartCoroutine(DownLoadRoutine(dialogueURL));
    }

    public IEnumerator DownLoadRoutine(string urlPath)
    {
        UnityWebRequest request = UnityWebRequest.Get(urlPath);
        yield return request.SendWebRequest();

        if (request.result == UnityWebRequest.Result.Success)
        {
            string recievedData = request.downloadHandler.text;
            Debug.Log(recievedData);
            DialogueData = ProcessCSV(recievedData);
            if (urlPath == KindDialogue)
            {
                OnKindLoaded.Invoke();
            }
            if (urlPath == IdolDialogue)
            {
                OnIdolLoaded.Invoke();
            }
            if (urlPath == CrankyDialogue)
            {
                OnIdolLoaded.Invoke();
            }
            
            // ShowCSVData(DialogueData);
        }

        else
        {
            Debug.Log("데이터를 로드할 수 없습니다");
        }
        
    }

    public string[,] ProcessCSV(string data)
    {
        // 행별로 나눔
        string[] lines = data.Split('\n');
    
        string[,] DialogueTable = new string[lines.Length, lines[0].Split(',').Length];
    
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            for (int j = 0; j < values.Length; j++)
            {
                values[j] = values[j].Replace("\\c", ",");
                DialogueTable[i, j] = values[j];
            }
        }
    
        // 여기서 처리된 2차원배열
        return DialogueTable;
    }

    public void ShowCSVData(string[,] dataTable)
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