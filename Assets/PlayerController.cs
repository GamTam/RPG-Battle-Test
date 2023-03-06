using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private Rigidbody2D _char;
    
    [HideInInspector] public PlayerInput _playerInput;
    [HideInInspector] public InputAction _moveVector;
    [HideInInspector] public InputAction _interact;
    [HideInInspector] public bool _interacting;
    
    void Start()
    {
        if (Globals.Player != null) Destroy(Globals.Player);

        Globals.Player = this;
        
        _playerInput = GameObject.FindWithTag("Controller Manager").GetComponent<PlayerInput>();
        _moveVector = _playerInput.actions["Overworld/Move"];
        _interact = _playerInput.actions["Interact"];

        Globals.MusicManager.Play("SubconForest");
    }
    
    void Update()
    {
        _interacting = _interact.triggered;
        
        Vector2 moveVector = _moveVector.ReadValue<Vector2>();

        _char.velocity = moveVector * _moveSpeed;
    }
    
    public void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("NPC"))
        {
            other.gameObject.GetComponent<DialogueTrigger>().Talkable = true;
        }
    }

    public void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag.Equals("NPC"))
        {
            other.gameObject.GetComponent<DialogueTrigger>().Talkable = false;
        }
    }
}
