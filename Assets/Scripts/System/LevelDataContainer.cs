using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
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
	public static LevelDataContainer instance;

	public TMP_InputField title, artist, author, clipName, songVolume, songPreviewStart, songPreviewEnd,
		noteOffset, eventOffset, songStartDelay, bgFilename, bgColor, bgDimMultiplier;
	public Slider diffSlider, volumeSlider, bgDimSlider;
	public Text diffValueText;

	public Image bgImage, bgDim;

	public string levelPath, resourcePath;
	public string[] songPath;

	public LevelData levelData;
	public FileManager fileManager;
	
	public Dictionary<uint, Dictionary<string, List<NoteEvents>>> noteEvents =
		new Dictionary<uint, Dictionary<string, List<NoteEvents>>>();

	void Awake()
	{
		instance = this;
	}

	public void GetLevelData(string file)
	{
		try
		{
			//GC.Collect();
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

			songPreviewStart.text = levelData.settings.songPreviewStart.ToString();
			songPreviewEnd.text = levelData.settings.songPreviewEnd.ToString();

			noteOffset.text = levelData.settings.noteOffset.ToString();
			eventOffset.text = levelData.settings.eventOffset.ToString();
			songStartDelay.text = levelData.settings.songStartDelay.ToString();

			bgFilename.text = levelData.settings.bgFilename;
			bgDimMultiplier.text = levelData.settings.bgDimMultiplier.ToString(CultureInfo.InvariantCulture);
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

			string[] levelLocation = levelPath.Split('\\');
			StringBuilder sb = new StringBuilder();
			for (int i = 0; i < levelLocation.Length - 1; i++) sb.Append(levelLocation[i] + "\\");
			resourcePath = sb.ToString();
			
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
		levelData.settings.title = title.text;
		levelData.settings.artist = artist.text;
		levelData.settings.author = author.text;
		levelData.settings.difficulty = (uint) diffSlider.value;
		diffValueText.text = ((int) diffSlider.value).ToString();
	}

	public void ResetLevelData()
	{
		levelPath = "";
		levelData = new LevelData();
		clipName.text = "";
		title.text = "";
		artist.text = "";
		author.text = "";
		diffSlider.value = 1;
		diffValueText.text = ((int) diffSlider.value).ToString();
	}

	public void GetClipName(string nameStr)
	{
		clipName.text = nameStr;
		levelData.settings.songFilename = nameStr;
	}
}