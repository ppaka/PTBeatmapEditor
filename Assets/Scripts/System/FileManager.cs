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
    
    ExtensionFilter[] extensions = new[]
    {
        new ExtensionFilter("레벨 파일", "ptlevel"),
        new ExtensionFilter("Json 파일", "json")
    };

    public void LoadLevel()
    {
        var path = StandaloneFileBrowser.OpenFilePanel("레벨 불러오기", Application.persistentDataPath, extensions, false);

        if (path.Length > 0)
        {
            ldc.levelPath = path[0];

            StartCoroutine(GetLevelData(new Uri(path[0]).AbsoluteUri));
        }
    }

    public void CallLoadSong()
    {
        if (ldc.levelPath == "") return;

        var extensions = new[]
        {
            new ExtensionFilter("음악 파일", "ogg")
        };

        var path = StandaloneFileBrowser.OpenFilePanel("음악 불러오기", Application.persistentDataPath, extensions, false);

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

    public void SaveLevel()
    {
        if (ldc.levelData.settings.title == string.Empty || ldc.levelData.settings.artist == string.Empty ||
            ldc.levelData.settings.author == string.Empty)
            return;

        var path = ldc.levelPath;

        if (!File.Exists(ldc.levelPath))
            path = StandaloneFileBrowser.SaveFilePanel("레벨 저장하기",
                Application.persistentDataPath + "/", "level",
                "ptlevel");
        
        var settings = new JsonSerializerSettings()
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

        var path = StandaloneFileBrowser.SaveFilePanel("다른 이름으로 레벨 저장하기",
            Application.persistentDataPath + "/", "level",
            extensions);

        if (path.Equals(string.Empty)) return;

        var settings = new JsonSerializerSettings()
        {
            NullValueHandling = NullValueHandling.Ignore,
            Formatting = Formatting.Indented
        };

        File.WriteAllText(path, JsonConvert.SerializeObject(ldc.levelData, settings));
        ldc.levelPath = path;
    }

    private IEnumerator GetLevelData(string url)
    {
        using var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        ldc.GetLevelData(DownloadHandlerBuffer.GetContent(www));

        LoadEvents.levelLoadComplete();
    }

    private IEnumerator GetClip(string url)
    {
        var replaced = url.Replace("%20", " ").Replace("file:///", "");

        var splitPath = ldc.levelPath.Split('\\');

        var path = new string[splitPath.Length];

        for (var i = 0; i < splitPath.Length - 1; i++) path[i] = splitPath[i] + "/";

        path[path.Length - 1] = ldc.levelData.settings.songFilename;

        try
        {
            File.Copy(replaced, string.Join("", path), true);
        }
        catch
        {
            //
        }

        using var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        audioSource.clip = DownloadHandlerAudioClip.GetContent(www);

        var split = url.Split('/');
        var clipName = split[split.Length - 1].Replace("%20", " ");

        LoadEvents.audioLoadComplete();

        audioSource.clip.name = clipName;
        ldc.GetClipName(clipName);
    }
}