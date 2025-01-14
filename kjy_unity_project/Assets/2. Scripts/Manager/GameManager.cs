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
        InvokeRepeating("CheckSchedule", 0, 600);
    }

    void CheckSchedule()
    {
        foreach (GameObject NPC in NPCs)
        {
            var controller = NPC.GetComponent<NPCController>();
            var statemachine = NPC.GetComponent<NPCStateMachine>();
            
            // 마을 배회 시작
            if (DateTime.Now.Hour >= controller.npcData.StrollStartHour && controller.isStrolling==false)
            {
                // 이미 마을에 있다면 아래 로직 시행하지 않음
                if (controller.npcData.curScene == NPCScenes.Town)
                {
                    continue;
                }
                
                // 현재 NPC가 집에 있다면 NPC 오브젝트 제거
                if (controller.npcData.curScene == NPCScenes.Home)
                {
                    Destroy(NPC);
                }

                Instantiate(NPC, controller.npcData.TownSpawnPos.position, controller.npcData.TownSpawnPos.rotation);
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
                Instantiate(NPC, controller.npcData.HomeSpawnPos.position, controller.npcData.HomeSpawnPos.rotation);
                controller.npcData.curScene = NPCScenes.Home;
                statemachine.OnChangeState(NPCStateMachine.StateType.NHome);
            }        
        }
    }
}