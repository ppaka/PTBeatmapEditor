using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using UnityEngine;
using UnityEngine.UI;

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
		Obj.Add("gameBar", objects[0]);
		Obj.Add("bgImage", objects[1]);
	}
}