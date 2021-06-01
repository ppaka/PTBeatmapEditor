using System;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

public class LevelDataContainer : MonoBehaviour
{
    public FileManager fileManager;

    public InputField title, artist, creater, clipName;
    public Slider diffSlider;
    public Text diffValueText;

    public Level level;

    public void GetLevelData(string file)
    {
        try
        {
            var info = FileReader.ReadLevelData(file, DataType.Info);
            level.info = info;
            title.text = level.title = info[0];
            artist.text = level.artist = info[1];
            creater.text = level.creater = info[2];
            diffValueText.text = (diffSlider.value = level.difficulty = int.Parse(info[3])).ToString();
            clipName.text = level.clipName = info[4];

            var note = FileReader.ReadLevelData(file, DataType.Note);

            var lastNot = note[note.Count - 1].Split(',');
            var lnLength = lastNot[0].Length;

            foreach (var data in note)
            {
                var sData = data.Split(',');
                var str = "";

                var stringBuilder = new StringBuilder();

                for (var i = 1; i < sData.Length; i++)
                {
                    stringBuilder.Append($",{sData[i]}");
                }

                str = stringBuilder.ToString();

                level.note.Add(int.Parse(sData[0]).ToString($"D{lnLength}") + str);
            }

            var events = FileReader.ReadLevelData(file, DataType.Events);
            var lastEvt = events[events.Count - 1].Split(',');
            var evLength = lastEvt[0].Length;

            foreach (var data in events)
            {
                var sData = data.Split(',');
                var str = "";

                var stringBuilder = new StringBuilder();

                for (var i = 1; i < sData.Length; i++)
                {
                    stringBuilder.Append($",{sData[i]}");
                }

                str = stringBuilder.ToString();

                level.events.Add(int.Parse(sData[0]).ToString($"D{evLength}") + str);
            }
            
            level.timings = FileReader.ReadLevelData(file, DataType.Timings);

            level.note.Sort();
            level.events.Sort();
            level.timings.Sort();

            var split = level.levelPath.Split('\\');

            string[] path = new string[split.Length];

            for (var i = 0; i < split.Length - 1; i++)
            {
                path[i] = split[i] + "/";
            }

            path[path.Length - 1] = level.clipName;

            fileManager.LoadSong(path);
        }
        catch (Exception e)
        {
            Debug.LogError(e);
        }
    }

    public void SetLevelData()
    {
        level.title = title.text;
        level.artist = artist.text;
        level.creater = creater.text;
        level.difficulty = (int) diffSlider.value;
        diffValueText.text = ((int) diffSlider.value).ToString();
    }

    public void ResetLevelData()
    {
        level.levelPath = "";
        level.clipName = clipName.text = "";
        level.info.Clear();
        level.note.Clear();
        level.events.Clear();
        level.timings.Clear();
        level.title = title.text = "";
        level.artist = artist.text = "";
        level.creater = creater.text = "";
        level.difficulty = (int) (diffSlider.value = 1);
        diffValueText.text = ((int) diffSlider.value).ToString();
    }

    public void GetClipName(string nameStr)
    {
        clipName.text = nameStr;
        level.clipName = nameStr;
    }
}