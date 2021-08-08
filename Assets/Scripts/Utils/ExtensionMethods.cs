using UnityEngine;

public static class ExtensionMethods
{
	public static void Log(this object value)
	{
		Debug.Log(value.ToString());
	}

	public static void Find<T>(out T obj) where T : Object
	{
		obj = Object.FindObjectOfType<T>();
	}
}