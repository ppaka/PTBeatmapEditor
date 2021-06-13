using System;
using System.Linq;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataContainer : MonoBehaviour
{
    public InputField title, artist, creater, clipName;
    public Slider diffSlider;
    public Text diffValueText;

    public string levelPath;

    public LevelData level;
    public FileManager fileManager;

    public void GetLevelData(string file)
    {
        try
        {
            level = JsonConvert.DeserializeObject<LevelData>(file);
            title.text = level.settings.title;
            artist.text = level.settings.artist;
            creater.text = level.settings.author;
            diffValueText.text = (diffSlider.value = level.settings.difficulty).ToString();
            clipName.text = level.settings.songFilename;

            level.events = level.events.OrderBy(x => x.time.ToString(), new StringAsNumericComparer()).ToList();
            level.notes = level.notes.OrderBy(x => x.time.ToString(), new StringAsNumericComparer())
                // ReSharper disable once SpecifyACultureInStringConversionExplicitly
                .ThenBy(x => x.duration.ToString(), new StringAsNumericComparer()).ToList();
            level.noteEvents = level.noteEvents.OrderBy(x => x.noteNum.ToString(), new StringAsNumericComparer())
                .ToList();

            var split = levelPath.Split('\\');

            var path = new string[split.Length];

            for (var i = 0; i < split.Length - 1; i++) path[i] = split[i] + "/";

            path[path.Length - 1] = level.settings.songFilename;

            fileManager.LoadSong(path);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void SetLevelData()
    {
        level.settings.title = title.text;
        level.settings.artist = artist.text;
        level.settings.author = creater.text;
        level.settings.difficulty = (uint) diffSlider.value;
        diffValueText.text = ((int) diffSlider.value).ToString();
    }

    public void ResetLevelData()
    {
        levelPath = "";
        level.settings.songFilename = clipName.text = "";
        level.settings = new Settings();
        level.notes.Clear();
        level.events.Clear();
        level.noteEvents.Clear();
        level.objects.Clear();
        level.settings.title = title.text = "";
        level.settings.artist = artist.text = "";
        level.settings.author = creater.text = "";
        level.settings.difficulty = (uint) (diffSlider.value = 1);
        diffValueText.text = ((int) diffSlider.value).ToString();
    }

    public void GetClipName(string nameStr)
    {
        clipName.text = nameStr;
        level.settings.songFilename = nameStr;
    }
}