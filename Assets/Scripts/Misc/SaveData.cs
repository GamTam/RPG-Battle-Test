using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    [SerializeField] private SaveDataFormatter _save;

    private void Awake()
    {
        LoadFromJson(false);
    }

    public void SaveIntoJson()
    {
        _save = new SaveDataFormatter();
        
        _save.CurrentScene = SceneManager.GetActiveScene().name;
        _save.PlayerPos = Globals.Player.transform.position;
        _save.Items = Globals.Items;
        _save.PlayerStats = Globals.PlayerStatsList;
        
        string jason = JsonUtility.ToJson(_save, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/File {Globals.SaveFile}.json", jason);
    }

    public void LoadFromJson(bool LoadScene)
    {
        string jason = System.IO.File.ReadAllText(Application.persistentDataPath + $"/File {Globals.SaveFile}.json");
        _save = JsonUtility.FromJson<SaveDataFormatter>(jason);
        
        Globals.PlayerPos = _save.PlayerPos;
        Globals.Items = _save.Items;
        Globals.PlayerStatsList = _save.PlayerStats;
        
        if (LoadScene) SceneManager.LoadScene(_save.CurrentScene);
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