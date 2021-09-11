using System.Collections;
using NAudio.Wave;
using UnityEngine;

public class Metronome : MonoBehaviour
{
	[Header("Objects")]
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
		if (isSongPositionMove || isMute || manager.audioFile == null || manager.outputDevice.PlaybackState != PlaybackState.Playing) return;

		if (manager.audioFile.Position >= nextSample)
		{
			print(manager.audioFile.Position);
			StartCoroutine(TicSfx());
		}
	}

	public void StartMet()
	{
		isMute = true;

		if (manager.audioFile == null)
		{
			Debug.Log("클립이 비어있습니다.");
			return;
		}

		rotateAngle = Vector3Int.zero;
		rotateAngle.z = 90;

		_offset = LevelTimings.startOffset * 0.001f;

		//if (musicBpm <= 0) musicBpm = manager.AnalyzeBpm();
		try
		{
			musicBpm = LevelDataContainer.Instance.levelData.timings[0].bpm;
		}
		catch
		{
			musicBpm = 120;
		}

		musicBpm = 170;
		if (stdBpm <= 0) stdBpm = 60.0;
		if (musicBeat <= 0) musicBeat = 4;
		if (stdBeat <= 0) stdBeat = 4;

		if (oneBeatTime != stdBpm / musicBpm * (musicBeat / stdBeat)) isSongPositionMove = true;

		offsetForSample = _offset * manager.audioFile.WaveFormat.AverageBytesPerSecond;
		oneBeatTime = stdBpm / musicBpm * (musicBeat / stdBeat);

		beatPerSample = oneBeatTime;
		nextSample = offsetForSample;

		print("샘플 당 비트 " + beatPerSample);
		
		MovePosition();

		isMute = false;
	}

	public void MovePosition()
	{
		if (!isSongPositionMove)
			isSongPositionMove = true;

		double newSample = offsetForSample;

		while (newSample <= manager.audioFile.Position)
		{
			if (newSample > manager.audioFile.Position) break;
			newSample += beatPerSample;
		}

		isSongPositionMove = false;

		nextSample = newSample;
		
		Debug.Log("새로운 샘플 : " + newSample);
	}

	IEnumerator TicSfx()
	{
		ticSource.Play();
		beatPerSample = oneBeatTime * manager.audioFile.WaveFormat.AverageBytesPerSecond;
		nextSample += beatPerSample;
		Debug.Log(nextSample + " : " + beatPerSample);
		// gameObject.transform.Rotate(rotateAngle);
		yield return null;
	}
}