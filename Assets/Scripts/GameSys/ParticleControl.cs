using UnityEngine;

public class ParticleControl : MonoBehaviour
{
    public ParticleSystem particle;
    public float startTime;

    public void SetTime(float time)
    {
        if (particle.isPlaying && startTime > time)
            particle.Stop();
        else if (!particle.isPlaying) return;
        Debug.Log("재생");
        particle.time = time - startTime;
    }
}