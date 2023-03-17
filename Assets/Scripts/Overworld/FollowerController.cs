using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FollowerController : MonoBehaviour
{
    [SerializeField] private PlayerController _player;
    [SerializeField] private GameObject _target;
    [SerializeField] private Rigidbody2D _char;
    [SerializeField] private Animator _animator;
    [SerializeField] private int _queueDelay;

    private Queue<Vector3> _posQueue = new Queue<Vector3>();
    private Queue<PlayerDir> _facingQueue = new Queue<PlayerDir>();
    private Vector2 _newMove;
    private PlayerDir _facing;

    private void FixedUpdate()
    {
        MoveChar();

        _animator.Play(!_player.IsMoving ? $"idle{_facing.ToString()}" : $"walk{_facing.ToString()}");
    }

    private void MoveChar()
    {
        _char.velocity = Vector2.zero;
        
        if (!_player.IsMoving) return;
        
        _posQueue.Enqueue(_target.transform.position);
        _facingQueue.Enqueue(_player.GetFacing());

        if (_posQueue.Count >= _queueDelay)
        {
            Vector3 pos = _posQueue.Dequeue();

            _newMove.x = pos.x - transform.position.x;
            _newMove.y = pos.y - transform.position.y;
            _facing = _facingQueue.Dequeue();
        }
        
        _char.velocity = _newMove * _player._moveSpeed;
    }
}
