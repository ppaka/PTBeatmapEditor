using System.Collections.Generic;

[System.Serializable]
public class Level
{
    public string levelPath;

    public string title, artist, creater, clipName;
    public int difficulty = 1;

    public List<string> info, note, events, timings;
}