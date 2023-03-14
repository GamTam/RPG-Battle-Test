using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float _moveSpeed = 2.5f;
    [SerializeField] private Rigidbody2D _char;
    [SerializeField] private Animator _animator;
    
    [HideInInspector] public PlayerInput _playerInput;
    [HideInInspector] public InputAction _moveVector;
    [HideInInspector] public InputAction _interact;
    [HideInInspector] public bool _interacting;

    private Vector2 _prevMoveVector;
    private string _facing = "_d";
    
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

        #region SetAnimation

        if (moveVector == Vector2.zero)
        {
            _animator.Play($"idle{_facing}");
        }
        else
        {
            if (moveVector == Vector2.up)
            {
                _facing = "_u";
            } 
            else if (moveVector == Vector2.down)
            {
                _facing = "_d";
            } 
            else if (moveVector == Vector2.left)
            {
                _facing = "_l";
            } 
            else if (moveVector == Vector2.right)
            {
                _facing = "_r";
            }
            
            _animator.Play($"walk{_facing}");
        }

        #endregion

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
