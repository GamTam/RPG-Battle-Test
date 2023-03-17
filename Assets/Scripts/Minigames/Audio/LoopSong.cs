using UnityEngine;
using System.IO;

public class LoopSong : MonoBehaviour
{
    public AudioSource audioSource;
    private float startTime = 0.0f;
    private float endTime = 1000.0f;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        LoadStartEndTime(audioSource.clip.name);
    }

    void Update()
    {
        if (audioSource.time >= endTime)
        {
            audioSource.time = startTime;
        }
    }

    public void LoadStartEndTime(string songName)
    {
        string filePath = Application.dataPath + "/songInfo.txt";
        if (File.Exists(filePath))
        {
            string[] lines = File.ReadAllLines(filePath);
            for(int i = 0; i < lines.Length; i++)
            {
                string[]songData = lines[i].Split(',');
                if(songData[0] == songName)
                {
                    startTime = float.Parse(songData[1]);
                    endTime = float.Parse(songData[2]);
                    break;
                }
            }
        }
        else
        {
            Debug.LogError("File not found at " + filePath);
        }
    }
}