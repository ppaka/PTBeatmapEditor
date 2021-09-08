using UnityEngine;

public class LevelTimings : MonoBehaviour
{
    public static int startOffset
    {
        get
        {
            if (LevelDataContainer.Instance.levelData.timings != null && LevelDataContainer.Instance.levelData.timings.Count != 0)
            {
                return (int) LevelDataContainer.Instance.levelData.timings[0].time;
            }

            return 0;
        }
    }
}