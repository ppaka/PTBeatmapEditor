using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum ResourceType
{
	Image,
	BackgroundImage
}

public class LevelDataContainer : MonoBehaviour
{
	public static LevelDataContainer Instance;

	public LevelTimings levelTimings;

	public TMP_InputField title, artist, author, clipName, songVolume, songPreviewStart, songPreviewEnd,
		songStartDelay, bgFilename, bgColor, bgDimMultiplier;
	public Slider diffSlider, volumeSlider, bgDimSlider;
	public Text diffValueText;

	public Image bgImage, bgDim;

	public string levelPath, resourcePath;
	public string[] songPath;

	public AudioSource songSource;
	public LevelData levelData;
	public FileManager fileManager;
	public ListMaker listMaker;
	
	public Dictionary<uint, Dictionary<string, List<NoteEvents>>> noteEvents =
		new Dictionary<uint, Dictionary<string, List<NoteEvents>>>();

	void Awake()
	{
		Instance = this;
	}

	bool loadedCompletely = true;

	public void SetResourcePath()
	{
		string[] levelLocation = levelPath.Split('\\');
		StringBuilder sb = new StringBuilder();
		for (int i = 0; i < levelLocation.Length - 1; i++) sb.Append(levelLocation[i] + "\\");
		resourcePath = sb.ToString();
	}
	
	public void GetLevelData(string file)
	{
		try
		{
			//GC.Collect();
			loadedCompletely = false;
			levelData = new LevelData();
			levelData = JsonConvert.DeserializeObject<LevelData>(file);
			title.text = levelData.settings.title;
			artist.text = levelData.settings.artist;
			author.text = levelData.settings.author;
			
			diffValueText.text = levelData.settings.difficulty.ToString();
			diffSlider.value = levelData.settings.difficulty;
			
			clipName.text = levelData.settings.songFilename;
			songVolume.text = levelData.settings.volume.ToString();
			volumeSlider.value = levelData.settings.volume;
			songSource.volume = Mathf.Round(levelData.settings.volume) / 100f;

			songPreviewStart.text = levelData.settings.songPreviewStart.ToString();
			songPreviewEnd.text = levelData.settings.songPreviewEnd.ToString();

			bgFilename.text = levelData.settings.bgFilename;
			bgDimMultiplier.text = levelData.settings.bgDimMultiplier.ToString();
			bgDimSlider.value = levelData.settings.bgDimMultiplier;
			bgColor.text = levelData.settings.bgColor;
			
			levelData.events = levelData.events.OrderBy(x => x.time.ToString(), new StringAsNumericComparer()).ToList();
			levelData.notes = levelData.notes.OrderBy(x => x.noteNum.ToString(), new StringAsNumericComparer())
				.ThenBy(x => x.time.ToString(), new StringAsNumericComparer()).ToList();
			levelData.noteEvents = levelData.noteEvents
				.OrderBy(x => x.noteNum.ToString(), new StringAsNumericComparer()).ToList();

			string[] split = levelPath.Split('\\');

			string[] path = new string[split.Length];

			for (int i = 0; i < split.Length; i++) path[i] = split[i] + "\\";
			path[path.Length - 1] = levelData.settings.songFilename;
			songPath = path;

			SetResourcePath();
			
			//BG Image
			if (!levelData.settings.bgFilename.Equals(String.Empty))
			{
				try
				{
					Texture2D tex = new Texture2D(0, 0);
					tex.LoadImage(File.ReadAllBytes(resourcePath + levelData.settings.bgFilename));
					SetBg(tex);
				}
				catch (Exception e)
				{
					
					Debug.Log("배경 이미지를 가져올 수 없음" + e);
				}
			}

			fileManager.LoadSong(songPath);
			SystemEvents.noteRemoveAll?.Invoke();

			loadedCompletely = true;
		}
		catch (Exception e)
		{
			Debug.LogError(e);
		}
	}

	public void SetBg(Texture2D texture2D)
	{
		bgImage.type = levelData.settings.bgType switch
		{
			"Simple" => Image.Type.Simple,
			"Filled" => Image.Type.Filled,
			"Tiled" => Image.Type.Tiled,
			_ => bgImage.type
		};
		bgImage.sprite = Sprite.Create(texture2D, new Rect(0, 0, texture2D.width, texture2D.height), new Vector2(0.5f, 0.5f));
		bgImage.sprite.texture.filterMode = FilterMode.Point;

		ColorUtility.TryParseHtmlString("#" + levelData.settings.bgColor, out Color color);
		bgImage.color = color;
		bgImage.pixelsPerUnitMultiplier = levelData.settings.bgPixelPerUnitMultiplier;
		bgDim.color = new Color(0, 0, 0, levelData.settings.bgDimMultiplier);
	}

	public void SetLevelData()
	{
		if (!loadedCompletely) return;

		levelData.settings.title = title.text;
		levelData.settings.artist = artist.text;
		levelData.settings.author = author.text;
		levelData.settings.difficulty = (uint) diffSlider.value;
		diffValueText.text = ((int) diffSlider.value).ToString();

		levelData.settings.songFilename = clipName.text;
		levelData.settings.volume = (uint)volumeSlider.value;
		songVolume.text = ((uint)volumeSlider.value).ToString();
		songSource.volume = Mathf.Round(levelData.settings.volume) / 100f;

		if (songPreviewStart.text != String.Empty)
			levelData.settings.songPreviewStart = Convert.ToUInt32(songPreviewStart.text);
		if (songPreviewEnd.text != String.Empty)
			levelData.settings.songPreviewEnd = Convert.ToUInt32(songPreviewEnd.text);

		levelData.settings.bgFilename = bgFilename.text;
		levelData.settings.bgDimMultiplier = bgDimSlider.value;
		bgDimMultiplier.text = bgDimSlider.value.ToString();

		if (!songStartDelay.text.Equals(string.Empty))
		{
			if (songStartDelay.text.Length == 8)
			{
				levelData.settings.bgColor = bgColor.text;
			}
			else if (songStartDelay.text.Length == 6)
			{
				levelData.settings.bgColor = bgColor.text;
			}
			else
			{
				levelData.settings.bgColor = bgColor.text = "FFFFFF";
			}
		}
	}

	public void ResetLevelData()
	{
		loadedCompletely = false;
		levelData = new LevelData
		{
			settings = new Settings(),
			objects = new List<Objects>(),
			curves = new List<Curves>(),
			timings = new List<Timings>(),
			events = new List<Events>(),
			noteEvents = new List<NoteEvents>(),
			notes = new List<Notes>()
		};
		
		levelPath = string.Empty;
		resourcePath = string.Empty;
		
		title.text = levelData.settings.title = "";
		artist.text = levelData.settings.artist = "";
		author.text = levelData.settings.author = "";
		
		levelData.settings.difficulty = (uint)(diffSlider.value = 1);
		diffValueText.text = levelData.settings.difficulty.ToString();

		clipName.text = levelData.settings.songFilename;
		songVolume.text = levelData.settings.volume.ToString();
		volumeSlider.value = levelData.settings.volume;

		songPreviewStart.text = levelData.settings.songPreviewStart.ToString();
		songPreviewEnd.text = levelData.settings.songPreviewEnd.ToString();

		bgFilename.text = levelData.settings.bgFilename;
		bgDimMultiplier.text = levelData.settings.bgDimMultiplier.ToString();
		bgDimSlider.value = levelData.settings.bgDimMultiplier;
		bgColor.text = levelData.settings.bgColor;

		listMaker.ClearItems();
		SystemEvents.noteRemoveAll?.Invoke();

		loadedCompletely = true;
	}

	public void GetClipName(string nameStr)
	{
		clipName.text = nameStr;
		levelData.settings.songFilename = nameStr;
	}
}