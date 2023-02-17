using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
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
    [SerializeField] public List<AttackSO> _attacks;
    
    private bool _hpSliding;
    private bool _mpSliding;
    
    void Awake()
    {
        Random rand = new Random();
        _HP = _maxHP;
        _MP = _maxMP;
        _speed = rand.Next(5000);

        _PFPSlot.sprite = _PFP;
        _PFPSlot.material = new Material(_PFPSlot.material);
        _mat = _PFPSlot.material;
        
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

    private void LateUpdate()
    {
        _hpText.SetText($"({_HP}/{_maxHP})");
        _mpText.SetText($"({_MP}/{_maxMP})");
        
        if (EventSystem.current.currentSelectedGameObject == gameObject) FlashWhite();
        else
        {
            _timer = 0;
            _mat.SetColor("_Color_2", Color.white);
        }

        if (!_hpSliding)
        {
            _bigHealthSlider.value = _HP;
            _smallHealthSlider.value = _HP;
        }

        if (!_mpSliding)
        {
            _bigMagicSlider.value = _MP;
            _smallMagicSlider.value = _MP;
        }

        if (_HP <= 0 && _PFPSlot.sprite != _deadPFP) _PFPSlot.sprite = _deadPFP;
    }

    public void SetNameText()
    {
        _nameText.SetText(_name);
    }
    
    public IEnumerator UpdateHpSlider(int targetValue)
    {
        _hpSliding = true;

        _HP = targetValue;
        float movementDuration = 3;
        float timeElapsed = 0;
            
        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            _bigHealthSlider.value = Mathf.Lerp(_bigHealthSlider.value, targetValue, Time.deltaTime * 5);
            _smallHealthSlider.value = Mathf.Lerp(_smallHealthSlider.value, targetValue, Time.deltaTime * 5);
                    
            yield return null;
        }

        _bigHealthSlider.value = targetValue;
        _smallHealthSlider.value = targetValue;

        foreach (Slider slider in _redSliders)
        {
            slider.value = targetValue;
        }
        _hpSliding = false;
    }

    public IEnumerator UpdateMpSlider(int targetValue)
    {
        _mpSliding = true;

        _MP = targetValue;
        float movementDuration = 3;
        float timeElapsed = 0;
            
        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            _bigMagicSlider.value = Mathf.Lerp(_bigMagicSlider.value, targetValue, Time.deltaTime * 5);
            _smallMagicSlider.value = Mathf.Lerp(_smallMagicSlider.value, targetValue, Time.deltaTime * 5);
                    
            yield return null;
        }

        _bigMagicSlider.value = targetValue;
        _smallMagicSlider.value = targetValue;
        
        _mpSliding = false;
    }
}
