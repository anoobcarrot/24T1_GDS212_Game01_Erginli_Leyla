using UnityEngine;

public class HallwayTrigger : MonoBehaviour
{
    public ParticleSystem particleSystem; 
    private bool isParticleActive;

    void Start()
    {
        isParticleActive = false;
        particleSystem.Stop();
    }

    // when player enters trigger
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !isParticleActive)
        {
            // Activate the particle system
            particleSystem.Play();
            isParticleActive = true;
        }
    }

    // when player exits trigger
    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player") && isParticleActive)
        {
            // Deactivate the particle system
            particleSystem.Stop();
            isParticleActive = false;
        }
    }
}

