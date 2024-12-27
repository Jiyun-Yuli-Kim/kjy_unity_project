using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "NPC Data", fileName = "KindData")]
public class NPCData : ScriptableObject
{
    public string NPCName = "Kind";
    public Personalities personality;

}
