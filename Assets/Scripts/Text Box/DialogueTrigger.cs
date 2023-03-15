using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private BattleLoadScene _sceneLoader;
    [SerializeField] private bool _skipTextboxCloseAnimation;
    [TextArea(3, 4)] [SerializeField] private string[] _dialogue;

    [HideInInspector] public PlayerInput _playerInput;
    private bool _triggeredDialogue;
    
    [SerializeField] private bool _talkable = false;

    private void Start()
    {
        _playerInput = GameObject.FindWithTag("Controller Manager").GetComponent<PlayerInput>();
    }

    private void Update()
    {
        if (_talkable && Globals.Player._interacting && !Globals.InBattle)
        {
            _triggeredDialogue = true;
            TriggerDialogue();
        }
        
        if (_sceneLoader == null) return;
        if (_triggeredDialogue && _playerInput.currentActionMap.name == "Overworld")
        {
            _playerInput.SwitchCurrentActionMap("Null");
            StartCoroutine(_sceneLoader.BattleTransition());
            enabled = false;
        }
    }
    
    public void TriggerDialogue()
    {
        string[] dialogue = (string[]) _dialogue.Clone();
        
        FindObjectOfType<DialogueManager>().StartText(dialogue, gameObject.transform, _spriteRenderer, _skipTextboxCloseAnimation);
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _talkable = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            _talkable = false;
        }
    }
}
