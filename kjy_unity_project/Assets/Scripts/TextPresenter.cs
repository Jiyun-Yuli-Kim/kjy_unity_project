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
    [SerializeField] private PlayerController _player;
    [SerializeField] private DialogueSystem _dialogueSystem;
    private PlayerInput _input;

    [SerializeField] private GameObject _uICanvas;
    [SerializeField] private GameObject _2opsPopup;
    [SerializeField] private GameObject _3opsPopup;

    [SerializeField] private TextMeshProUGUI _npcName;
    [SerializeField] private TextMeshProUGUI _dialogueText;
    [SerializeField] private TextMeshProUGUI _choice1;
    [SerializeField] private TextMeshProUGUI _choice2;
    [SerializeField] private TextMeshProUGUI _choiceA;
    [SerializeField] private TextMeshProUGUI _choiceB;
    [SerializeField] private TextMeshProUGUI _choiceC;
    
    [SerializeField] private GameObject _highlighter1;
    [SerializeField] private GameObject _highlighter2;
    [SerializeField] private GameObject _highlighterA;
    [SerializeField] private GameObject _highlighterB;
    [SerializeField] private GameObject _highlighterC;
    
    [SerializeField] private GameObject _blinker;
    
    public int choice = 0;

    // Start is called before the first frame update
    void Awake()
    {
        _input = _player.GetComponent<PlayerInput>();
    }

    void Start()
    {
        _uICanvas.SetActive(false);
        _2opsPopup.SetActive(false);
        _3opsPopup.SetActive(false);
        _highlighter1.SetActive(false);
        _highlighter2.SetActive(false);
        _highlighterA.SetActive(false);
        _highlighterB.SetActive(false);
        _highlighterC.SetActive(false);
        _blinker.SetActive(false);
    }

    public IEnumerator StartDialogue()
    {
        _npcName.text = _player.partnerName;
        _dialogueText.text = _dialogueSystem.textToPrint;
        _uICanvas.SetActive(true);
        
        yield return new WaitForSeconds(0.7f);
        _blinker.SetActive(true);
        
        yield return new WaitUntil(() => _input.actions["Trigger"].WasPressedThisFrame());
        _blinker.SetActive(false);
    }

    public void EndDialogue()
    {
        _uICanvas.SetActive(false);
    }

    public IEnumerator LoadNextLine()
    {
        _dialogueText.text = _dialogueSystem.textToPrint;
        yield return new WaitForSeconds(0.7f);
        _blinker.SetActive(true);
        
        yield return new WaitUntil(() => _input.actions["Trigger"].WasPressedThisFrame());
        _blinker.SetActive(false);
    }

    public void SetChoices(string[] choices)
    {
        if (choices.Length == 2)
        {
            _choice1.text = choices[0];
            _choice2.text = choices[1];
            _highlighter1.SetActive(true);
            _2opsPopup.SetActive(true);
        }
        if (choices.Length == 3)
        {
            _choiceA.text = choices[0];
            _choiceB.text = choices[1];
            _choiceC.text = choices[2];
            _highlighterA.SetActive(true);
            _3opsPopup.SetActive(true);
        }
    }
    
    public IEnumerator GetChoice(string[] choices)
    {
        SetChoices(choices);
        Debug.Log("선택 코루틴 정상시행");
        yield return null;
        
        if (choices.Length == 2)
        {
            while (!_input.actions["Trigger"].WasPressedThisFrame())
            {
                Debug.Log("선택 루프 정상시행");

                if (choice == 0 && _input.actions["South"].WasPressedThisFrame())
                {
                    _highlighter1.SetActive(false);
                    _highlighter2.SetActive(true);
                    choice = 1;
                }

                if (choice == 1 && _input.actions["North"].WasPressedThisFrame())
                {
                    _highlighter2.SetActive(false);
                    _highlighter1.SetActive(true);
                    choice = 0;
                }

                yield return null;
            }
            _highlighter1.SetActive(false);
            _highlighter2.SetActive(false);
            _2opsPopup.SetActive(false);
        }
        
        if (choices.Length == 3)
        {
            while (!_input.actions["Trigger"].WasPressedThisFrame())
            {
                Debug.Log($"선택 루프 정상시행, {choice}");
                    
                if (choice == 0 && _input.actions["South"].WasPressedThisFrame())
                {
                    _highlighterA.SetActive(false);
                    _highlighterB.SetActive(true);
                    _highlighterC.SetActive(false);
                    choice = 1;
                }

                else if (choice == 1 && _input.actions["North"].WasPressedThisFrame())
                {
                    _highlighterA.SetActive(true);
                    _highlighterB.SetActive(false);
                    _highlighterC.SetActive(false);
                    choice = 0;
                }
                
                else if (choice == 1 && _input.actions["South"].WasPressedThisFrame())
                {
                    _highlighterA.SetActive(false);
                    _highlighterB.SetActive(false);
                    _highlighterC.SetActive(true);
                    choice = 2;
                }
                
                else if (choice == 2 && _input.actions["North"].WasPressedThisFrame())
                {
                    _highlighterA.SetActive(false);
                    _highlighterB.SetActive(true);
                    _highlighterC.SetActive(false);
                    choice = 1;
                }

                yield return null;
            }
            _highlighterA.SetActive(false);
            _highlighterB.SetActive(false);
            _highlighterC.SetActive(false);
            _3opsPopup.SetActive(false);
        }
    }
}
