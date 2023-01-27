using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Battle.State_Machine;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class BattleManager : MonoBehaviour
{
    [SerializeField] private string _song;
    [SerializeField] private GameObject _playerList;
    [SerializeField] private GameObject _enemyList;
    
    public TMP_Text _textBoxText;
    
    [Header("Player Box")]
    public TMP_Text _nameTagText;
    public Image _nameTagImage;
    public Button[] _buttons;
    public TextSelection[] _selectionBoxes;
    [HideInInspector] public List<Image> _buttonImages = new List<Image>();
    
    public State _state;
    [HideInInspector] public List<Battleable> _fighters = new List<Battleable>();
    [HideInInspector] public List<Animator> _players = new List<Animator>();
    [HideInInspector] public List<Enemy> _enemies = new List<Enemy>();
    [HideInInspector] public List<Vector3> _profilePoints = new List<Vector3>();
    [HideInInspector] public MusicManager _musicManager;
    private int _currentFighterIndex;
    [HideInInspector] public List<Animator> _deadPlayers = new List<Animator>();
    [HideInInspector] public List<Enemy> _deadEnemies = new List<Enemy>();
    [HideInInspector] public int _currentPlayerIndex;
    [HideInInspector] public int _turnIndex;
    [HideInInspector] public int _currentButton = 0;
    
    [HideInInspector] public bool _cancelMovement;
    
    [HideInInspector] public PlayerInput _playerInput;
    [HideInInspector] public InputAction _confirm;

    private void Start()
    {
        _playerInput = GameObject.FindWithTag("Controller Manager").GetComponent<PlayerInput>();
        _confirm = _playerInput.actions["Confirm"];
        
        foreach (Battleable chara in _playerList.GetComponentsInChildren<Battleable>())
        {
            _players.Add(chara.gameObject.GetComponent<Animator>());
            _profilePoints.Add(chara.gameObject.transform.localPosition);
            _fighters.Add(chara);
        }

        foreach (Battleable chara in _enemyList.GetComponentsInChildren<Battleable>())
        {
            _enemies.Add((Enemy) chara);
            _fighters.Add(chara);
        }
        
        _currentPlayerIndex = 0;
        _currentFighterIndex = 0;
        _turnIndex = 0;
        
        SortFighters();

        _currentFighterIndex = -1;
        _turnIndex = -1;

        foreach (Button button in _buttons)
        {
            _buttonImages.Add(button.GetComponent<Image>());
        }
        
        _musicManager = GameObject.FindWithTag("Audio").GetComponent<MusicManager>();
        _musicManager.Play(_song);
        SwitchStates(new Opening(this));
    }

    public void PickTurn()
    {
        _currentFighterIndex = (_currentFighterIndex + 1) % _fighters.Count;
        if (_currentFighterIndex == 0) _turnIndex += 1;
        
        bool playersDead = true;
        foreach (Animator player in _players)
        {
            if (!_deadPlayers.Contains(player))
            {
                playersDead = false;
                break;
            }
        }

        if (playersDead)
        {
            SwitchStates(new LoseState(this));
            return;
        }
        
        bool enemysDead = true;
        foreach (Enemy enemy in _enemies)
        {
            if (!_deadEnemies.Contains(enemy))
            {
                enemysDead = false;
                break;
            }
        }

        if (enemysDead)
        {
            SwitchStates(new WinState(this));
            return;
        }
        
        SortFighters(false);
        
        if (_fighters[_currentFighterIndex]._HP > 0)
        {
            if (_fighters[_currentFighterIndex].GetType() == typeof(Player))
            {
                SwitchStates(new PlayerTurn(this, (Player) _fighters[_currentFighterIndex]));
                _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
            }
            else
            {
                SwitchStates(new EnemyTurn(this, (Enemy) _fighters[_currentFighterIndex]));
            }

            return;
        }

        if (_fighters[_currentFighterIndex].GetType() == typeof(Player))
        {
            _currentPlayerIndex = (_currentPlayerIndex + 1) % _players.Count;
        }

        PickTurn();
    }

    public void Click()
    {
        StartCoroutine(_state.OnClick());
    }

    public void SwitchStates(State state)
    {
        if (_state != null) StartCoroutine(_state.ExitState());
        _state = state;
        StartCoroutine(state.EnterState());
    }

    public void SortFighters(bool position = true)
    {
        _profilePoints.Sort((x, y) => x.y.CompareTo(y.y));
        _fighters.Sort((x, y) => y._speed.CompareTo(x._speed));

        if (!position) return;
        int i = 0;
        foreach (Battleable bat in _fighters)
        {
            if (bat.GetType() != typeof(Player)) continue;
            bat.transform.localPosition = _profilePoints[(i + _currentPlayerIndex) % _players.Count];
            Vector2 vec = ((Player) bat).StartingLocation;
            ((Player) bat).StartingLocation = new Vector2(vec.x, bat.transform.localPosition.y);
            i++;
        }
    }

    public void InitFinalSlide(Player obj)
    {
        StartCoroutine(FinalSlide(obj));
    }
    
    private IEnumerator FinalSlide(Player obj)
    {
        _cancelMovement = false;
        float movementDuration = 2;
        float timeElapsed = 0;
            
        while (timeElapsed < movementDuration && !_cancelMovement)
        {
            timeElapsed += Time.deltaTime;
            obj.gameObject.transform.localPosition = Vector3.Lerp(obj.gameObject.transform.localPosition, obj.StartingLocation, Time.deltaTime * 5);
                    
            yield return null;
        }
        
        if (!_cancelMovement) obj.gameObject.transform.localPosition = obj.StartingLocation;
    }
    
    public void DisableButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            if (_buttons[i].gameObject == EventSystem.current.currentSelectedGameObject)
                _currentButton = i;
            _buttons[i].interactable = false;
        }
    }
    
    public void EnableButtons()
    {
        for (int i = 0; i < _buttons.Length; i++)
        {
            _buttons[i].interactable = true;
        }

        EventSystem.current.SetSelectedGameObject(_buttons[_currentButton].gameObject);
    }
}

public enum DamageTypes
{
    HP,
    MP,
    Money
}