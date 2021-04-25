using System;
using System.Collections.Generic;
using System.Globalization;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class Level
{
    public string levelPath;
    
    public string title, artist, creater, clipName;
    public int difficulty = 1;
    
    public List<string> info, note, events, timings;
}

public class LevelDataContainer : MonoBehaviour
{
    public Level level;
    public FileManager fileManager;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }

    public InputField title, artist, creater, clipName;
    public Slider diffSlider;
    public Text diffValueText;
    
    public void GetLevelData(string file)
    {
        var info = FileReader.ReadLevelData(file, DataType.Info);
        title.text = level.title = info[0];
        artist.text = level.artist = info[1];
        creater.text = level.creater = info[2];
        diffValueText.text = (diffSlider.value = level.difficulty = int.Parse(info[3])).ToString();
        clipName.text = level.clipName = info[4];
        
            var split = level.levelPath.Split('\\');
            
            string[] path = new string[split.Length];
            
            for (var i = 0; i < split.Length - 1; i++)
            {
                path[i] = split[i] + "/";
            }

            path[path.Length - 1] = level.clipName;

            fileManager.LoadSong(path);
    }

    public void SetLevelData()
    {
        level.title = title.text;
        level.artist = artist.text;
        level.creater = creater.text;
        level.difficulty = (int) diffSlider.value;
        diffValueText.text = ((int)diffSlider.value).ToString();
    }

    public void GetClipName(string nameStr)
    {
        clipName.text = nameStr;
        level.clipName = nameStr;
    }
}