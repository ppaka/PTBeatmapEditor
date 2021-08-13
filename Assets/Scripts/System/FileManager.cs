using System;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using Newtonsoft.Json;
using SFB;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
	public AudioSource audioSource;
	public LevelDataContainer ldc;

	readonly ExtensionFilter[] extensions =
	{
		new ExtensionFilter("레벨 파일", "ptlevel"),
		new ExtensionFilter("Json 파일", "json")
	};

	public void LoadLevel()
	{
		string[] path =
			StandaloneFileBrowser.OpenFilePanel("레벨 불러오기", Application.persistentDataPath, extensions, false);

		if (path.Length > 0)
		{
			ldc.levelPath = path[0];

			//StartCoroutine(GetLevelData(new Uri(path[0]).AbsoluteUri));
			StartCoroutine(GetLevelData(path[0]));
		}
	}

	public void CallLoadSong()
	{
		if (ldc.levelPath == "") return;

		var extensions = new[]
		{
			new ExtensionFilter("음악 파일", "ogg")
		};

		string[] path =
			StandaloneFileBrowser.OpenFilePanel("음악 불러오기", Application.persistentDataPath, extensions, false);

		if (path.Length > 0) StartCoroutine(GetClip(new Uri(path[0]).AbsoluteUri));
	}

	public void LoadSong([CanBeNull] string[] path)
	{
		if (path == null)
		{
			var extensions = new[]
			{
				new ExtensionFilter("음악 파일", "ogg")
			};

			path = StandaloneFileBrowser.OpenFilePanel("음악 불러오기", Application.persistentDataPath, extensions, false);

			if (path.Length > 0) StartCoroutine(GetClip(new Uri(path[0]).AbsoluteUri));
		}
		else
		{
			if (path.Length > 0) StartCoroutine(GetClip(new Uri(string.Join("", path)).AbsoluteUri));
		}
	}
	
	public void CallLoadBackgroundImage()
	{
		if (ldc.levelPath == "") return;

		var extensions = new[]
		{
			new ExtensionFilter("이미지 파일", "png")
		};

		string[] path =
			StandaloneFileBrowser.OpenFilePanel("배경 이미지 불러오기", Application.persistentDataPath, extensions, false);

		if (path.Length > 0) StartCoroutine(GetResourceImage(path[0], ResourceType.BackgroundImage));
	}

	public void SaveLevel()
	{
		if (ldc.levelData.settings.title == string.Empty || ldc.levelData.settings.artist == string.Empty ||
		    ldc.levelData.settings.author == string.Empty)
			return;

		string path = ldc.levelPath;

		if (!File.Exists(ldc.levelPath))
			path = StandaloneFileBrowser.SaveFilePanel("레벨 저장하기", Application.persistentDataPath + "/", "level",
				extensions);

		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.Indented
		};

		File.WriteAllText(path, JsonConvert.SerializeObject(ldc.levelData, settings));
		ldc.levelPath = path;
	}

	public void SaveLevelAs()
	{
		if (ldc.levelData.settings.title.Equals("") || ldc.levelData.settings.artist.Equals("") ||
		    ldc.levelData.settings.author.Equals(""))
			return;

		string path = StandaloneFileBrowser.SaveFilePanel("다른 이름으로 레벨 저장하기", Application.persistentDataPath + "/",
			"level", extensions);

		if (path.Equals(string.Empty)) return;

		JsonSerializerSettings settings = new JsonSerializerSettings
		{
			NullValueHandling = NullValueHandling.Ignore,
			Formatting = Formatting.Indented
		};

		File.WriteAllText(path, JsonConvert.SerializeObject(ldc.levelData, settings));
		ldc.levelPath = path;
	}

	IEnumerator GetLevelData(string url)
	{
		if (File.Exists(url))
		{
			StreamReader reader = new StreamReader(url);

			string textValue = reader.ReadToEnd();
			reader.Close();
			ldc.GetLevelData(textValue);
			SystemEvents.levelLoadComplete();
			yield break;
		}
	}

	IEnumerator GetResourceImage(string url, ResourceType type)
	{
		if (type == ResourceType.BackgroundImage)
		{
			if (File.Exists(url))
			{
				Texture2D tex = new Texture2D(0, 0);
				tex.LoadImage(File.ReadAllBytes(url));
				
				ldc.SetBg(tex);
				yield break;
			}
		}
		yield return null;
	}

	IEnumerator GetClip(string url)
	{
		string replaced = url.Replace("%20", " ").Replace("file:///", "");

		string[] splitPath = ldc.levelPath.Split('\\');

		string[] path = new string[splitPath.Length];

		for (int i = 0; i < splitPath.Length - 1; i++) path[i] = splitPath[i] + "/";

		path[path.Length - 1] = ldc.levelData.settings.songFilename;

		try
		{
			File.Copy(replaced, string.Join("", path), true);
		}
		catch
		{
			//
		}

		using UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.OGGVORBIS);
		yield return www.SendWebRequest();

		audioSource.clip = DownloadHandlerAudioClip.GetContent(www);

		string[] split = url.Split('/');
		string clipName = split[split.Length - 1].Replace("%20", " ");

		SystemEvents.audioLoadComplete();

		audioSource.clip.name = clipName;
		ldc.GetClipName(clipName);
	}
}