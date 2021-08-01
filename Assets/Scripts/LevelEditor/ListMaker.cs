using UnityEngine;
using UnityEngine.UI;

public class ListMaker : MonoBehaviour
{
    public GameObject itemPrefab, noteListParent, eventListParent;
    public LevelDataContainer ldc;
    public InputField time, duration;
    public Dropdown noteType, ease;

    private void Awake()
    {
        LoadEvents.levelLoadComplete += MakeLists;
    }

    private void OnDisable()
    {
        LoadEvents.levelLoadComplete -= MakeLists;
    }

    private void MakeLists()
    {
        foreach (var data in ldc.levelData.notes)
        {
            var cache = Instantiate(itemPrefab, noteListParent.transform);
            //cache.GetComponentInChildren<Text>().text = data.ToString();
        }

        foreach (var data in ldc.levelData.events)
        {
            var cache = Instantiate(itemPrefab, eventListParent.transform);
            //cache.GetComponentInChildren<Text>().text = data.ToString();
        }
    }

    public void SelectData()
    {
    }
}