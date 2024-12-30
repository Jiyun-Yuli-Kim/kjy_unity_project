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
    // [SerializeField] private TextMeshProUGUI _choice3;

    [SerializeField] private GameObject _highlighter1;
    [SerializeField] private GameObject _highlighter2;

    [SerializeField] private GameObject _blinker;
    
    public int choice=0;

    // Start is called before the first frame update
    void Awake()
    {
        _input = _player.GetComponent<PlayerInput>();
    }

    void Start()
    {
        _uICanvas.SetActive(false);
        _2opsPopup.SetActive(false);
        _highlighter1.SetActive(false);
        _highlighter2.SetActive(false);
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
        _choice1.text = choices[0];
        _choice2.text = choices[1];
        _highlighter1.SetActive(true);
        _2opsPopup.SetActive(true);
    }
    
    public IEnumerator GetChoice()
    {
        Debug.Log("선택 코루틴 정상시행");
        while (!_input.actions["Trigger"].WasPressedThisFrame())
        {
            if (choice == 0 && _input.actions["South"].WasPressedThisFrame())
            { 
                _highlighter1.SetActive(false);
                _highlighter2.SetActive(true);
                choice=1;
            }

            if (choice == 1 && _input.actions["North"].WasPressedThisFrame())
            {
                _highlighter2.SetActive(false);
                _highlighter1.SetActive(true);
                choice=0;
            }
            
            yield return null;
        }
        _highlighter2.SetActive(false);
        _2opsPopup.SetActive(false);
    }
}
