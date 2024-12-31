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
            // 1단계: recievedData-현재 데이터가 통으로 들어있음 (',','\n'이 있는 raw data)
            string recievedData = request.downloadHandler.text;
            
            // Debug.Log(recievedData);
            
            // 2단계: DialogueData-2차원 배열로 가공한 데이터
            DialogueData = ProcessCSV(recievedData);
            
            // 3단계: 주민 성격별로 다른 데이터 타입을 로드하기 위해 이벤트를 활용
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
        // 행별로 나눔. lines[]-원소 하나당 한 행의 데이터가 ','를 포함하여 들어있음 
        string[] lines = data.Split('\n');
    
        // 2차원 배열 생성. DialogueTable[,] 1열: 대사 인덱스, 2열: 대사, 3열: 선택, 4열: 다음 인덱스
        string[,] DialogueTable = new string[lines.Length, lines[0].Split(',').Length];
    
        Debug.Log(lines.Length);
        Debug.Log(lines[0].Split(',').Length);
        
        // 행 바꾸기
        for (int i = 1; i < lines.Length; i++)
        {
            string[] values = lines[i].Split(',');
            
            // 해당 행의 열마다 데이터 넣기
            for (int j = 0; j < values.Length; j++)
            {
                Debug.Log(values.Length);
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
        if (input.Contains("@"))
        {
            string result1 = input.Replace("\\n", "\n");
            string result2 = result1.Replace("\\c", ",");
            string result3 = result2.Replace("!PN!", _player._playerData.PlayerName);
            Debug.Log($"문자열 디코딩중 : {_player.partnerCp}");
            string result4 = result3.Replace("!CP!", _player.partnerCp);
            string result5 = result4.Replace("@", "");
            return result5;
        }
        
        else return input;
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