using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Enemy : Battleable
{
    [SerializeField] private EnemySO _baseEnemy;
    [SerializeField] private Image _image;
    private Material _mat;
    
    private AttackSO[] _attacks;
    [HideInInspector] public string _description;
    float fade = 1;
    [HideInInspector] public bool _killable;
    
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
        _description = _baseEnemy.Description;

        _image.material = new Material(_image.material);
        _mat = _image.material;

        _slider.maxValue = _maxHP;
        _redSliders[0].maxValue = _maxHP;
        _redSliders[0].value = _maxHP;
        _slider.gameObject.transform.SetParent(gameObject.transform.parent.parent);
    }

    private void Update()
    {
        _slider.value = _HP;
        
        if (_killable) {
            fade -= Time.deltaTime;
            if (fade <= 0f) {
                fade = 0;
                gameObject.SetActive(false);
            }

            _mat.SetFloat("_Fade", fade);
        }
    }
}
