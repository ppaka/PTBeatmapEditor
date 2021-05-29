using System;
using System.Collections;
using System.IO;
using JetBrains.Annotations;
using SFB;
using UnityEngine;
using UnityEngine.Networking;

public class FileManager : MonoBehaviour
{
    public AudioSource audioSource;
    public LevelDataContainer ldc;

    public void LoadLevel()
    {
        var extensions = new[]
        {
            new ExtensionFilter("레벨 파일", "ptlevel")
        };

        var path = StandaloneFileBrowser.OpenFilePanel("레벨 불러오기", Application.persistentDataPath, extensions, false);

        if (path.Length > 0)
        {
            ldc.level.levelPath = path[0];

            StartCoroutine(GetLevelData(new Uri(path[0]).AbsoluteUri));
        }
    }

    public void CallLoadSong()
    {
        if (ldc.level.levelPath == "") return;

        var extensions = new[]
        {
            new ExtensionFilter("음악 파일", "mp3", "wav", "ogg")
        };

        var path = StandaloneFileBrowser.OpenFilePanel("음악 불러오기", Application.persistentDataPath, extensions, false);

        if (path.Length > 0)
        {
            StartCoroutine(GetClip(new Uri(path[0]).AbsoluteUri));
        }
    }

    public void LoadSong([CanBeNull] string[] path)
    {
        if (path == null)
        {
            var extensions = new[]
            {
                new ExtensionFilter("음악 파일", "mp3", "wav", "ogg")
            };

            path = StandaloneFileBrowser.OpenFilePanel("음악 불러오기", Application.persistentDataPath, extensions, false);

            if (path.Length > 0)
            {
                StartCoroutine(GetClip(new Uri(path[0]).AbsoluteUri));
            }
        }
        else
        {
            if (path.Length > 0)
            {
                StartCoroutine(GetClip(new Uri(string.Join("", path)).AbsoluteUri));
            }
        }
    }

    public void SaveLevel()
    {
        if (ldc.level.title == string.Empty || ldc.level.artist == string.Empty || ldc.level.creater == string.Empty)
            return;

        var path = ldc.level.levelPath;

        if (!File.Exists(ldc.level.levelPath))
        {
            path = StandaloneFileBrowser.SaveFilePanel("레벨 저장하기",
                Application.persistentDataPath + "/", "level",
                "ptlevel");
        }

        var writer = File.CreateText(path);
        writer.WriteLine("[Info]");
        writer.WriteLine(ldc.level.title);
        writer.WriteLine(ldc.level.artist);
        writer.WriteLine(ldc.level.creater);
        writer.WriteLine(ldc.level.difficulty);
        writer.WriteLine(ldc.level.clipName);
        writer.WriteLine("");

        writer.WriteLine("[Timings]");
        foreach (var eachTiming in ldc.level.timings)
        {
            writer.WriteLine(eachTiming);
        }

        writer.WriteLine("");

        writer.WriteLine("[Events]");
        foreach (var eachEvent in ldc.level.events)
        {
            writer.WriteLine(eachEvent);
        }

        writer.WriteLine("");

        writer.WriteLine("[Note]");
        foreach (var eachNote in ldc.level.note)
        {
            writer.WriteLine(eachNote);
        }

        writer.Dispose();

        ldc.level.levelPath = path;
    }

    public void SaveLevelAs()
    {
        if (ldc.level.title == string.Empty || ldc.level.artist == string.Empty || ldc.level.creater == string.Empty)
            return;

        var path = StandaloneFileBrowser.SaveFilePanel("다른 이름으로 레벨 저장하기",
            Application.persistentDataPath + "/", "level",
            "ptlevel");

        if (path.Equals(string.Empty))
            return;

        var writer = File.CreateText(path);
        writer.WriteLine("[Info]");
        writer.WriteLine(ldc.level.title);
        writer.WriteLine(ldc.level.artist);
        writer.WriteLine(ldc.level.creater);
        writer.WriteLine(ldc.level.difficulty);
        writer.WriteLine(ldc.level.clipName);
        writer.WriteLine("");

        writer.WriteLine("[Timings]");
        foreach (var eachTiming in ldc.level.timings)
        {
            writer.WriteLine(eachTiming);
        }

        writer.WriteLine("");

        writer.WriteLine("[Events]");
        foreach (var eachEvent in ldc.level.events)
        {
            writer.WriteLine(eachEvent);
        }

        writer.WriteLine("");

        writer.WriteLine("[Note]");
        foreach (var eachNote in ldc.level.note)
        {
            writer.WriteLine(eachNote);
        }

        writer.Dispose();

        ldc.level.levelPath = path;
    }

    private IEnumerator GetLevelData(string url)
    {
        using var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        ldc.GetLevelData(DownloadHandlerBuffer.GetContent(www));
    }

    private IEnumerator GetClip(string url)
    {
        var furl = url.Replace("%20", " ");
        var surl = furl.Replace("file:///", "");

        var splitee = ldc.level.levelPath.Split('\\');

        string[] path = new string[splitee.Length];

        for (var i = 0; i < splitee.Length - 1; i++)
        {
            path[i] = splitee[i] + "/";
        }

        path[path.Length - 1] = ldc.level.clipName;

        try
        {
            File.Copy(surl, string.Join("", path), true);
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

        LoadEvents.AudioLoadComplete();

        audioSource.clip.name = clipName;
        ldc.GetClipName(clipName);
    }
}