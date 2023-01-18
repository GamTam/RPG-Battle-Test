using System;
using System.Collections;
using System.Collections.Generic;
using Battle.State_Machine;
using TMPro;
using UnityEngine;
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
    public Image[] _buttonImages;
    
    private State _state;
    [HideInInspector] public List<Battleable> _fighters = new List<Battleable>();
    [HideInInspector] public List<Animator> _players = new List<Animator>();
    [HideInInspector] public List<GameObject> _enemies = new List<GameObject>();
    [HideInInspector] public List<Vector3> _profilePoints = new List<Vector3>();
    private MusicManager _musicManager;
    private int _currentIndex = 0;

    private void Start()
    {
        foreach (Battleable chara in _playerList.GetComponentsInChildren<Battleable>())
        {
            _players.Add(chara.gameObject.GetComponent<Animator>());
            _profilePoints.Add(chara.gameObject.transform.localPosition);
            _fighters.Add(chara);
        }

        foreach (Battleable chara in _enemyList.GetComponentsInChildren<Battleable>())
        {
            _enemies.Add(chara.gameObject);
            _fighters.Add(chara);
        }
        SortFighters();
        
        _musicManager = GameObject.FindWithTag("Audio").GetComponent<MusicManager>();
        _musicManager.Play(_song);
        SwitchStates(new Opening(this));
    }

    public void PickTurn()
    {
        if (_fighters[_currentIndex].GetType() == typeof(Player))
        {
            SwitchStates(new PlayerTurn(this, (Player) _fighters[_currentIndex]));
        }
        else
        {
            SwitchStates(new EnemyTurn(this, (Enemy) _fighters[_currentIndex]));
        }

        _currentIndex = _currentIndex + 1 >= _fighters.Count ? 0 : _currentIndex + 1;
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

    public void SortFighters()
    {
        _profilePoints.Sort((x, y) => x.y.CompareTo(y.y));
        _fighters.Sort((x, y) => y._speed.CompareTo(x._speed));

        int i = 0;
        foreach (Battleable bat in _fighters)
        {
            if (bat.GetType() != typeof(Player)) continue;
            bat.transform.localPosition = _profilePoints[i];
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
        float movementDuration = 2;
        float timeElapsed = 0;
            
        while (timeElapsed < movementDuration)
        {
            timeElapsed += Time.deltaTime;
            obj.gameObject.transform.localPosition = Vector3.Lerp(obj.gameObject.transform.localPosition, obj.StartingLocation, Time.deltaTime * 5);
                    
            yield return null;
        }

        obj.gameObject.transform.localPosition = obj.StartingLocation;
    }
}

public enum DamageTypes
{
    HP,
    MP,
    Money
}