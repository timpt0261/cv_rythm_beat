using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerBurst : MonoBehaviour
{
    [SerializeField] private ParticleSystem impactRingEffect;
    [SerializeField] private HandCollisionDetector detector;

    private void Update()
    {
        if (detector.active)
        {
            BurstImpactRing();
        }
    }

    private void BurstImpactRing()
    {
        impactRingEffect.Emit(1); // Emit a burst of particles
    }
}
