using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Random = System.Random;

public class Player : Battleable
{
    [SerializeField] private Sprite _PFP;
    [SerializeField] private Sprite _deadPFP;
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
    public int _MP;
    public int _maxMP;

    [HideInInspector] public Vector2 StartingLocation;
    private AttackSO[] _attacks;
    
    void Awake()
    {
        Random rand = new Random();
        //_HP = (int) (_maxHP * rand.NextDouble());
        _HP = _maxHP;
        _MP = (int) (_maxMP * rand.NextDouble());
        _speed = rand.Next(5000);

        _PFPSlot.sprite = _PFP;
        
        _bigHealthSlider.maxValue = _maxHP;
        _smallHealthSlider.maxValue = _maxHP;
        _bigMagicSlider.maxValue = _maxMP;
        _smallMagicSlider.maxValue = _maxMP;

        _redSliders[0].maxValue = _maxHP;
        _redSliders[0].value = _maxHP;
        _redSliders[1].maxValue = _maxHP;
        _redSliders[1].value = _maxHP;

        StartingLocation = transform.localPosition;
        transform.localPosition = new Vector2(transform.localPosition.x - 500, transform.localPosition.y);
    }

    private void Update()
    {
        _hpText.SetText($"({_HP}/{_maxHP})");
        _mpText.SetText($"({_MP}/{_maxMP})");
        
        _bigHealthSlider.value = _HP;
        _smallHealthSlider.value = _HP;
        _bigMagicSlider.value = _MP;
        _smallMagicSlider.value = _MP;

        if (_HP <= 0 && _PFPSlot.sprite != _deadPFP) _PFPSlot.sprite = _deadPFP;
    }

    public void SetNameText()
    {
        _nameText.SetText(_name);
    }
}
