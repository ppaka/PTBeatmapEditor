using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataContainer : MonoBehaviour
{
    public InputField title, artist, creater, clipName;
    public Slider diffSlider;
    public Text diffValueText;

    public Image bgImage, bgDim;

    public string levelPath, resourcePath;
    public string[] songPath;

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

            for (var i = 0; i < split.Length; i++) path[i] = split[i] + "\\";
            path[path.Length - 1] = level.settings.songFilename;
            songPath = path;

            var levelLocation = levelPath.Split('\\');
            var sb = new StringBuilder();
            for (var i = 0; i < levelLocation.Length - 1; i++) sb.Append(levelLocation[i] + "\\");
            resourcePath = sb.ToString();
            SetBg();
            
            fileManager.LoadSong(songPath);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void SetBg()
    {
        bgImage.type = level.settings.bgType switch
        {
            "Simple" => Image.Type.Simple,
            "Filled" => Image.Type.Filled,
            "Tiled" => Image.Type.Tiled,
            _ => bgImage.type
        };
        var tex = new Texture2D(0, 0);
        tex.LoadImage(File.ReadAllBytes(resourcePath+level.settings.bgFilename));
        bgImage.sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f,0.5f));
        bgImage.sprite.texture.filterMode = FilterMode.Point;
        
        ColorUtility.TryParseHtmlString("#" + level.settings.bgColor, out var color);
        bgImage.color = color;
        bgImage.pixelsPerUnitMultiplier = level.settings.bgPixelPerUnitMultiplier;
        bgDim.color = new Color(0, 0, 0, level.settings.bgDimMultiplier);
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