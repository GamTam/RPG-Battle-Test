using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DialogueTrigger : MonoBehaviour
{
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [TextArea(15, 20)] [SerializeField] private string[] _dialogue;
    
    private bool _talkable = false;
    
    public bool Talkable {get {return _talkable;} set {_talkable = value;}}

    private void Update()
    {
        if (_talkable && Globals.Player._interacting)
        {
            TriggerDialogue();
        }
    }
    
    public void TriggerDialogue()
    {
        string[] dialogue = (string[]) _dialogue.Clone();
        
        FindObjectOfType<DialogueManager>().StartText(dialogue, gameObject.transform, _spriteRenderer);
    }
}
