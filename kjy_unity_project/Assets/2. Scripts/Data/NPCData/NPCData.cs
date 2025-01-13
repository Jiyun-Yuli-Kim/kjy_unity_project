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
}
