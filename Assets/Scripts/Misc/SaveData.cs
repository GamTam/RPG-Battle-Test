using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SaveData : MonoBehaviour
{
    [SerializeField] private SaveDataFormatter _save;

    private void Awake()
    {
        // LoadFromJson(false);
        // if (SceneManager.GetActiveScene().name != _save.CurrentScene) SceneManager.LoadScene(_save.CurrentScene);
    }

    public void SaveIntoJson()
    {
        _save = new SaveDataFormatter();
        
        _save.CurrentScene = SceneManager.GetActiveScene().name;
        _save.PlayerPos = Globals.Player.transform.position;
        _save.Items = Globals.Items;
        _save.PlayerStats = Globals.PlayerStatsList;
        _save.PlayedCutscenes = Globals.PlayedCutscenes;
        
        string jason = JsonUtility.ToJson(_save, true);
        System.IO.File.WriteAllText(Application.persistentDataPath + $"/File {Globals.SaveFile}.oddsmaker", jason);
    }

    public void LoadFromJson(bool LoadScene)
    {
        if (!File.Exists(Application.persistentDataPath + $"/File {Globals.SaveFile}.oddsmaker")) return;
        
        string jason = System.IO.File.ReadAllText(Application.persistentDataPath + $"/File {Globals.SaveFile}.oddsmaker");
        _save = JsonUtility.FromJson<SaveDataFormatter>(jason);
        
        Globals.PlayerPos = _save.PlayerPos;
        Globals.Items = _save.Items;
        Globals.PlayerStatsList = _save.PlayerStats;
        Globals.PlayedCutscenes = _save.PlayedCutscenes;
        
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
    public List<string> PlayedCutscenes;
}