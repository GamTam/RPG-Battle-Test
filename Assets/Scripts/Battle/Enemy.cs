using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Battleable
{
    [SerializeField] private EnemySO _baseEnemy;
    [SerializeField] private Slider _slider;
    
    private AttackSO[] _attacks;
    
    void Start()
    {
        _name = _baseEnemy.Name;
        _level = _baseEnemy.Level;
        _HP = _baseEnemy.HP;
        _maxHP = _baseEnemy.HP;
        _pow = _baseEnemy.Pow;
        _def = _baseEnemy.Def;
        _speed = _baseEnemy.Speed;
        _attacks = _baseEnemy.Attacks;

        _slider.maxValue = _maxHP;
    }

    private void Update()
    {
        _slider.value = _HP;
    }
}
