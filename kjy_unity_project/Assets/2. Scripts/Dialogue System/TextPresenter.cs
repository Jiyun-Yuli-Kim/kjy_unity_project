using System.Collections;
using System.Collections.Generic;
using System.Net.Mime;
using Cinemachine;
using TMPro;
using Unity.VisualScripting;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

public class TextPresenter : MonoBehaviour
{
    public PlayerController player;

    public GameObject uICanvas;
    public GameObject twoOpsPopup;
    public GameObject threeOpsPopup;

    public TextMeshProUGUI npcName;
    public TextMeshProUGUI dialogueText;
    public TextMeshProUGUI choice1;
    public TextMeshProUGUI choice2;
    public TextMeshProUGUI choiceA;
    public TextMeshProUGUI choiceB;
    public TextMeshProUGUI choiceC;

    public GameObject highlighter1;
    public GameObject highlighter2;
    public GameObject highlighterA;
    public GameObject highlighterB;
    public GameObject highlighterC;

    public GameObject blinker;
    
    void Start()
    {
        uICanvas.SetActive(false);
        twoOpsPopup.SetActive(false);
        threeOpsPopup.SetActive(false);
        highlighter1.SetActive(false);
        highlighter2.SetActive(false);
        highlighterA.SetActive(false);
        highlighterB.SetActive(false);
        highlighterC.SetActive(false);
        blinker.SetActive(false);
    }

    public void TwoOpsPopupOn()
    {
        highlighter1.SetActive(true);
        twoOpsPopup.SetActive(true);
    }
    
    public void TwoOpsPopupOff()
    {
        highlighter1.SetActive(false);
        highlighter2.SetActive(false);
        twoOpsPopup.SetActive(false);
    }

    public void ThreeOpsPopupOn()
    {
        highlighterA.SetActive(true);
        threeOpsPopup.SetActive(true);
    }
    
    public void ThreeOpsPopupOff()
    {
        highlighterA.SetActive(false);
        highlighterB.SetActive(false);
        highlighterC.SetActive(false);
        threeOpsPopup.SetActive(false);
    }

    public void Select1()
    {
        highlighter2.SetActive(false);
        highlighter1.SetActive(true);
    }

    public void Select2()
    {
        highlighter1.SetActive(false);
        highlighter2.SetActive(true);
    }

    public void SelectA()
    {
        highlighterA.SetActive(true);
        highlighterB.SetActive(false);
        highlighterC.SetActive(false);
    }
    
    public void SelectB()
    {
        highlighterA.SetActive(false);
        highlighterB.SetActive(true);
        highlighterC.SetActive(false);
    }
    
    public void SelectC()
    {
        highlighterA.SetActive(false);
        highlighterB.SetActive(false);
        highlighterC.SetActive(true);
    }

    public void SetNPCName(string s)
    {
        Debug.Log(npcName);
        npcName.text = s;
    }

    public void SetDialogueText(string s)
    {
        dialogueText.text = s.Replace("!CP!", player.partnerCp);
    }

    public void SetChoice1(string s)
    {
        choice1.text = s;
    }
    
    public void SetChoice2(string s)
    {
        choice2.text = s;
    }
    
    public void SetChoiceA(string s)
    {
        choiceA.text = s;
    }
    
    public void SetChoiceB(string s)
    {
        choiceB.text = s;
    }
    
    public void SetChoiceC(string s)
    {
        choiceC.text = s;
    }

    public void DialogueCanvasOn()
    {
        uICanvas.SetActive(true);
    }
    
    public void DialogueCanvasOff()
    {
        uICanvas.SetActive(false);
    }



    
}
