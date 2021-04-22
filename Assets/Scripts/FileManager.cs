using System;
using System.Collections;
using System.Collections.Generic;
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
            StartCoroutine(GetLevelData(new Uri(path[0]).AbsoluteUri));
        }
    }

    public void LoadSong()
    {
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

    public void SaveLevel()
    {
        StandaloneFileBrowser.SaveFilePanel("레벨 저장하기",
            Application.persistentDataPath + "/", "level",
            "ptlevel");
    }

    private IEnumerator GetLevelData(string url)
    {
        using var www = UnityWebRequest.Get(url);
        yield return www.SendWebRequest();

        ldc.SetLevel(DownloadHandlerBuffer.GetContent(www));
    }
    
    private IEnumerator GetClip(string url)
    {
        using var www = UnityWebRequestMultimedia.GetAudioClip(url, AudioType.UNKNOWN);
        yield return www.SendWebRequest();

        audioSource.clip = DownloadHandlerAudioClip.GetContent(www);
        audioSource.clip.name = DownloadHandlerAudioClip.GetContent(www).name;
    }
}