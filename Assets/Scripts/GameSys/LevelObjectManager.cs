using System.Collections.Generic;
using UnityEngine;

public class LevelObjectManager : MonoBehaviour
{
	public GameObject[] objects;

	List<Objects> _objectData;
	public Dictionary<string, GameObject> Obj = new Dictionary<string, GameObject>();

	void OnEnable()
	{
		SystemEvents.levelLoadComplete += Load;
	}

	void OnDisable()
	{
		SystemEvents.levelLoadComplete -= Load;
	}

	public void Load()
	{
		if (!Obj.ContainsKey("gameBar"))
			Obj.Add("gameBar", objects[0]);
		if (!Obj.ContainsKey("bgImage"))
			Obj.Add("bgImage", objects[1]);
	}
}