using System.Collections;
using UnityEngine;

public class Metronome : MonoBehaviour
{
	[Header("Objects")] 
	[SerializeField] private AudioSource audioSource;

	private AudioSource ticSource;
	
	private Vector3Int rotateAngle;

	private double offset;
	private double bpm;

	private int split;

	private double oneBeatTime;
	private double nextSample;
	private double offsetForSample;

	public void StartMet()
	{
		rotateAngle = Vector3Int.zero;
		rotateAngle.z = 90;

		ticSource = gameObject.GetComponent<AudioSource>();

		offset = LevelDataContainer.Instance.levelData.settings.noteOffset * 0.001f;
		bpm = 120;

		offsetForSample = offset * audioSource.clip.frequency;
		oneBeatTime = (60.0 / bpm);

		nextSample = offsetForSample;

		audioSource.clip.frequency.Log();
		audioSource.clip.samples.Log();

		// (audioSource.clip.samples / audioSource.clip.frequency).Log();
	}

	private bool isMute;
	
	public void MovePosition()
	{
		double newSample = offsetForSample;

		if (newSample <= audioSource.timeSamples)
		{
			isMute = true;
		}
		
		while (newSample <= audioSource.timeSamples)
		{
			newSample += oneBeatTime * audioSource.clip.frequency;
		}
		
		isMute = false;

		nextSample = newSample;
	}

	private void Update()
	{
		if (!audioSource.isPlaying) return;
		
		// ((float) audioSource.timeSamples / (float) audioSource.clip.frequency).Log();
		if (audioSource.timeSamples >= nextSample)
		{
			if (!isMute)
				StartCoroutine(TicSFX());
		}
	}

	private IEnumerator TicSFX()
	{
		ticSource.Play();
		nextSample += oneBeatTime * audioSource.clip.frequency;
		gameObject.transform.Rotate(rotateAngle);
		yield return null;
	}
}

public static class ExtensionMethods
{
	public static void Log(this object value)
	{
		Debug.Log(value.ToString());
	}
}