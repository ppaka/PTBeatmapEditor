using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Level
{
    public string title, artist, creater;
    public int difficulty;
    
    public List<string> info, note, events, timings;
}

public class LevelDataContainer : MonoBehaviour
{
    public Level level;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public void GetLevelData(string file)
    {
        var info = FileReader.ReadLevelData(file, DataType.Info);
        level.title = info[0];
        level.artist = info[1];
        level.creater = info[2];
        level.difficulty = int.Parse(info[3]);
    }

    public InputField title, artist, creater;
    public Slider diffSlider;
    
    public void SetLevelData()
    {
        level.title = title.text;
        level.artist = artist.text;
        level.creater = creater.text;
        level.difficulty = (int) diffSlider.value;
    }
}