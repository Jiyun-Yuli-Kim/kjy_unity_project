using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC Data")]
public class NPCData : ScriptableObject
{
    public string NPCName;
    public Personalities personality;
    public string CatchPhrase;
    public int StrollStartHour;
    public int StrollEndHour;
    public NPCScenes curScene;
    
    public Vector3 HomeSpawnPos;
    public Quaternion HomeSpawnRot;
    public Vector3 TownSpawnPos;
    public Quaternion TownSpawnRot;
}
