using UnityEngine;

public class LevelTimings : MonoBehaviour
{
    public static int startOffset
    {
        get
        {
            if (LevelDataContainer.instance.levelData.timings != null && LevelDataContainer.instance.levelData.timings.Count != 0)
            {
                return (int) LevelDataContainer.instance.levelData.timings[0].time;
            }

            return 0;
        }
    }
}