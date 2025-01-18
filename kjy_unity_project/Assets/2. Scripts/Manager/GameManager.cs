using System;
using System.Collections;
using System.Collections.Generic;
using AOT;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }
    public SceneChanger SceneChange { get; private set; }
    [SerializeField] public List<GameObject> NPCs = new List<GameObject>();

    private void Awake()
    {
        Init();
    }

    private void Init()
    {
        SetSingleton();
        SceneChange = GetComponent<SceneChanger>();
    }

    private void SetSingleton()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
            Instance = this;
        }
        
        else if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(this);
        }
    }

    void Start()
    {
        InvokeRepeating("CheckSchedule", 0, 60);
        // 시간단위니까 1분마다 체크?
    }

    void CheckSchedule()
    {
        Debug.Log("CheckSchedule Invoked");
        for (int i=0; i < NPCs.Count; i++)
        {
            GameObject NPC = NPCs[i];
            var controller = NPCs[i].GetComponent<NPCController>();
            var statemachine = NPCs[i].GetComponent<NPCStateMachine>();
            
            // 마을 배회 시작
            if (DateTime.Now.Hour >= controller.npcData.StrollStartHour && controller.isStrolling==false)
            {
                Debug.Log($"{NPC.name} starts strolling");
                // 이미 마을에 있다면 state만 변경
                if (controller.npcData.curScene == NPCScenes.Town)
                {
                    statemachine.OnChangeState(NPCStateMachine.StateType.NStroll);
                    Debug.Log($"{NPC.name} state changed to {statemachine.CurrentState}");
                    continue;
                }
                
                // 현재 NPC가 집에 있다면 NPC 오브젝트 제거
                if (controller.npcData.curScene == NPCScenes.Home)
                {
                    Destroy(NPC);
                }

                var newNPC = Instantiate(NPC, controller.npcData.TownSpawnPos, controller.npcData.TownSpawnRot);
                // 기존 데이터 연동
                newNPC.GetComponent<NPCController>().npcData = controller.npcData;
                controller = newNPC.GetComponent<NPCController>();
                statemachine = newNPC.GetComponent<NPCStateMachine>();
                
                NPCs[i]= newNPC;
                controller.npcData.curScene = NPCScenes.Town;
                
                statemachine.OnChangeState(NPCStateMachine.StateType.NStroll);
            }
            
            // 집으로 복귀
            if (DateTime.Now.Hour >= controller.npcData.StrollEndHour && controller.isStrolling)
            {
                // 이미 집에 있다면 아래 로직 시행하지 않음
                if (controller.npcData.curScene == NPCScenes.Home)
                {
                    continue;
                }
                
                // 현재 NPC가 마을에 있다면 NPC 오브젝트 제거
                if (controller.npcData.curScene == NPCScenes.Town)
                {
                    Destroy(NPC);
                }
                
                var newNPC = Instantiate(NPC, controller.npcData.HomeSpawnPos, controller.npcData.HomeSpawnRot);
                newNPC.GetComponent<NPCController>().npcData = controller.npcData;
                NPCs[i]= newNPC;
                controller.npcData.curScene = NPCScenes.Home;
                statemachine.OnChangeState(NPCStateMachine.StateType.NHome);
            }        
        }
    }
}