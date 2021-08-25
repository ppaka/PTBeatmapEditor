using System.Collections;
using UnityEngine;

public class Metronome : MonoBehaviour
{
	[Header("Objects")] [SerializeField] AudioSource audioSource;
	[SerializeField] AudioManager manager;
	[SerializeField] AudioSource ticSource;

	public double musicBpm = 120, stdBpm = 60.0;
	public double musicBeat = 4, stdBeat = 4;
	public bool isMute = true, isSongPositionMove;

	double _offset;

	double oneBeatTime, barPerSec, beatPerSample, bitPerSample, bitPerSec, nextSample, offsetForSample;

	Vector3Int rotateAngle;

	void Update()
	{
		if (isSongPositionMove || isMute || audioSource.clip == null || !audioSource.isPlaying) return;

		// Debug.Log(audioSource.timeSamples);
		// ((float) audioSource.timeSamples / (float) audioSource.clip.frequency).Log();
		if (audioSource.timeSamples >= nextSample)
			StartCoroutine(TicSfx());
	}

	public void StartMet()
	{
		isMute = true;

		if (audioSource.clip == null)
		{
			Debug.Log("클립이 비어있습니다.");
			return;
		}

		rotateAngle = Vector3Int.zero;
		rotateAngle.z = 90;

		_offset = LevelTimings.startOffset * 0.001f;

		if (musicBpm <= 0) musicBpm = manager.AnalyzeBpm();
		if (stdBpm <= 0) stdBpm = 60.0;
		if (musicBeat <= 0) musicBeat = 4;
		if (stdBeat <= 0) stdBeat = 4;

		if (oneBeatTime != stdBpm / musicBpm * (musicBeat / stdBeat)) isSongPositionMove = true;

		offsetForSample = _offset * audioSource.clip.frequency;
		oneBeatTime = stdBpm / musicBpm * (musicBeat / stdBeat);

		//audioSource.clip.frequency.Log();
		//audioSource.clip.samples.Log();

		beatPerSample = oneBeatTime * audioSource.clip.frequency;
		nextSample = offsetForSample;

		MovePosition();

		isMute = false;

		//Debug.Log(nextSample);
		//Debug.Log(oneBeatTime * audioSource.clip.frequency);

		// (audioSource.clip.samples / audioSource.clip.frequency).Log();
	}

	public void MovePosition()
	{
		if (!isSongPositionMove)
			isSongPositionMove = true;

		// double newSample = oneBeatTime * audioSource.clip.frequency + offsetForSample;

		double newSample = offsetForSample;

		while (newSample <= audioSource.timeSamples)
		{
			if (newSample > audioSource.timeSamples) break;
			newSample += beatPerSample;
		}

		isSongPositionMove = false;

		nextSample = newSample;
	}

	IEnumerator TicSfx()
	{
		ticSource.Play();
		beatPerSample = oneBeatTime * audioSource.clip.frequency;
		nextSample += beatPerSample;
		// gameObject.transform.Rotate(rotateAngle);
		yield return null;
	}
}