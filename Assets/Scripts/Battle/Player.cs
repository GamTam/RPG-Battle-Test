using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class Player : Battleable
{
    [SerializeField] private Sprite _PFP;
    [SerializeField] private Image _PFPSlot;
    [Header("Sliders")]
    [SerializeField] private Slider _bigHealthSlider;
    [SerializeField] private Slider _smallHealthSlider;
    [SerializeField] private Slider _bigMagicSlider;
    [SerializeField] private Slider _smallMagicSlider;
    [Header("Text")]
    [SerializeField] private TMP_Text _nameText;
    [SerializeField] private TMP_Text _hpText;
    [SerializeField] private TMP_Text _mpText;

    [Header("Temp")]
    [SerializeField] private string _name;
    [SerializeField] private int _level;
    [SerializeField] private int _HP;
    [SerializeField] private int _maxHP;
    [SerializeField] private int _MP;
    [SerializeField] private int _maxMP;
    [SerializeField] private int _pow;
    [SerializeField] private int _def;
    [SerializeField] private int _speed;
    private AttackSO[] _attacks;
    
    void Start()
    {
        _HP = _maxHP;
        _MP = _maxMP;

        _PFPSlot.sprite = _PFP;
        
        _bigHealthSlider.maxValue = _maxHP;
        _smallHealthSlider.maxValue = _maxHP;
        _bigMagicSlider.maxValue = _maxMP;
        _smallMagicSlider.maxValue = _maxMP;
    }

    private void Update()
    {
        _hpText.SetText($"({_HP}/{_maxHP})");
        _mpText.SetText($"({_MP}/{_maxMP})");
        
        _bigHealthSlider.value = _HP;
        _smallHealthSlider.value = _HP;
        _bigMagicSlider.value = _MP;
        _smallMagicSlider.value = _MP;
    }
}
