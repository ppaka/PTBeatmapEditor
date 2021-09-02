using UnityEngine;

public class ParticlesManager : MonoBehaviour
{
    public ParticleSystem particle;

    public void PlayParticle()
    {
        particle.Play();
    }
}