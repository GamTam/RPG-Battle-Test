using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class TextSelection : MonoBehaviour
{
    public TMP_Text _buttonPrefab;
    [SerializeField] private GameObject[] _columns;
    [SerializeField] private GameObject _selectionHead;
    private GameObject _currentSelectedObject;
    private BattleManager _battleManager;
    
    public List<List<TMP_Text>> _buttons = new List<List<TMP_Text>>();
    private int _activeButtons;
    
    private int _currentColum;
    private int _currentRow;

    private PlayerInput _input;
    private InputAction _cancel;
    private InputAction _confirm;
    private Vector2 _prevMoveVector;

    private void Start()
    {
        _input = GameObject.FindWithTag("Controller Manager").GetComponent<PlayerInput>();
        _battleManager = GameObject.FindWithTag("Battle Manager").GetComponent<BattleManager>();
        _cancel = _input.actions["Cancel"];
        _confirm = _input.actions["Confirm"];
    }

    public void ResetButtons()
    {
        foreach (List<TMP_Text> list in _buttons)
        {
            foreach (TMP_Text text in list)
            {
                Destroy(text.gameObject);
            }
        }
        
        _currentSelectedObject = null;
        _activeButtons = 0;
        _buttons = new List<List<TMP_Text>>();
        for (int i = 0; i < _columns.Length; i++)
        {
            _buttons.Add(new List<TMP_Text>());
        }
    }

    public void AddButton(string text)
    {
        TMP_Text button = Instantiate(_buttonPrefab, _columns[_activeButtons % 2].transform);
        button.gameObject.SetActive(true);
        button.gameObject.name = text;
        
        _buttons[_activeButtons % 2].Add(button);

        _activeButtons += 1;
        button.text = text;
    }

    public void Update()
    {
        if (_cancel.triggered)
        {
            ResetButtons();
            gameObject.SetActive(false);
            _battleManager.EnableButtons();
            _battleManager._soundManager.Play("back");
            return;
        }

        if (_confirm.triggered)
        {
            _battleManager._soundManager.Play("confirm");
            _battleManager.Click();
        }
        
        if (_currentSelectedObject == EventSystem.current.currentSelectedGameObject) return;
        _currentSelectedObject = EventSystem.current.currentSelectedGameObject;

        _selectionHead.transform.position =
            EventSystem.current.currentSelectedGameObject.transform.position;
    }
}
