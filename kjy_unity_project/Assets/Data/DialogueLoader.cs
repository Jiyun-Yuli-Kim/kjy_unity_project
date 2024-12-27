using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Networking;

public class DialogueLoader : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    
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
    
        // 2차원 배열 생성
        string[,] DialogueTable = new string[lines.Length, lines[0].Split(',').Length];
    
        // 행 바꾸기
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            
            // 해당 행의 열마다 데이터 넣기
            for (int j = 0; j < values.Length; j++)
            {
                //values[j] = values[j].Replace("\\c", ",");
                values[j] = Decode(values[j]);
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
    
    private string Decode(string input)
    {
        string result1 = input.Replace("\\n", "\n");
        string result2 = result1.Replace("\\c", ",");
        string result3 = result2.Replace("!PN!", _player._playerData.PlayerName);
        string result4 = result3.Replace("!CP!", _player.partnerCp);
        return result4;
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