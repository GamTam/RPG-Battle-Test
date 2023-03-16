using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    [SerializeField] private SaveDataFormatter _save;
    public void SaveIntoJson()
    {
        _save.CurrentScene = SceneManager.GetActiveScene().name;
        _save.PlayerPos = Globals.Player.transform.position;
        _save.Items = Globals.Items;
        _save.PlayerStats = Globals.PlayerStatsList;
        
        string jason = JsonUtility.ToJson(_save, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/File {Globals.SaveFile}.json", jason);
    }
}

[Serializable]
public class SaveDataFormatter
{
    public string CurrentScene;
    public Vector3 PlayerPos;
    public List<sItem> Items;
    public List<PlayerStats> PlayerStats;
}