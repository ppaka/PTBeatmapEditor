using System;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataContainer : MonoBehaviour
{
    public static LevelDataContainer Instance;
    
    public InputField title, artist, creater, clipName;
    public Slider diffSlider;
    public Text diffValueText;

    public Image bgImage, bgDim;

    public string levelPath, resourcePath;
    public string[] songPath;

    public LevelData levelData;
    public FileManager fileManager;

    private void Awake()
    {
        Instance = this;
    }

    public void GetLevelData(string file)
    {
        try
        {
            levelData = JsonConvert.DeserializeObject<LevelData>(file);
            title.text = levelData.settings.title;
            artist.text = levelData.settings.artist;
            creater.text = levelData.settings.author;
            diffValueText.text = (diffSlider.value = levelData.settings.difficulty).ToString();
            clipName.text = levelData.settings.songFilename;

            levelData.events = levelData.events.OrderBy(x => x.time.ToString(), new StringAsNumericComparer()).ToList();
            levelData.notes = levelData.notes.OrderBy(x => x.noteNum.ToString(), new StringAsNumericComparer())
                .ThenBy(x => x.time.ToString(), new StringAsNumericComparer()).ToList();
            levelData.noteEvents = levelData.noteEvents.OrderBy(x => x.noteNum.ToString(), new StringAsNumericComparer())
                .ToList();

            var split = levelPath.Split('\\');

            var path = new string[split.Length];

            for (var i = 0; i < split.Length; i++) path[i] = split[i] + "\\";
            path[path.Length - 1] = levelData.settings.songFilename;
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
        bgImage.type = levelData.settings.bgType switch
        {
            "Simple" => Image.Type.Simple,
            "Filled" => Image.Type.Filled,
            "Tiled" => Image.Type.Tiled,
            _ => bgImage.type
        };
        var tex = new Texture2D(0, 0);
        tex.LoadImage(File.ReadAllBytes(resourcePath+levelData.settings.bgFilename));
        bgImage.sprite = Sprite.Create(tex, new Rect(0,0,tex.width,tex.height), new Vector2(0.5f,0.5f));
        bgImage.sprite.texture.filterMode = FilterMode.Point;
        
        ColorUtility.TryParseHtmlString("#" + levelData.settings.bgColor, out var color);
        bgImage.color = color;
        bgImage.pixelsPerUnitMultiplier = levelData.settings.bgPixelPerUnitMultiplier;
        bgDim.color = new Color(0, 0, 0, levelData.settings.bgDimMultiplier);
    }

    public void SetLevelData()
    {
        levelData.settings.title = title.text;
        levelData.settings.artist = artist.text;
        levelData.settings.author = creater.text;
        levelData.settings.difficulty = (uint) diffSlider.value;
        diffValueText.text = ((int) diffSlider.value).ToString();
    }

    public void ResetLevelData()
    {
        levelPath = "";
        levelData.settings.songFilename = clipName.text = "";
        levelData.settings = new Settings();
        levelData.notes.Clear();
        levelData.events.Clear();
        levelData.noteEvents.Clear();
        levelData.objects.Clear();
        levelData.settings.title = title.text = "";
        levelData.settings.artist = artist.text = "";
        levelData.settings.author = creater.text = "";
        levelData.settings.difficulty = (uint) (diffSlider.value = 1);
        diffValueText.text = ((int) diffSlider.value).ToString();
    }

    public void GetClipName(string nameStr)
    {
        clipName.text = nameStr;
        levelData.settings.songFilename = nameStr;
    }
}