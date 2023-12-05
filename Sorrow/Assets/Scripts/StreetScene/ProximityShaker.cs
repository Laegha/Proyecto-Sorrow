using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;

public class ProximityShaker : MonoBehaviour
{
    [SerializeField] Transform distanceTo;
    [SerializeField] float minDistance = 60f;
    [SerializeField] float maxShake = 2f;
    [SerializeField] Volume volume;
    [SerializeField] float maxVignette = .6f;
    float shakeMultiplier, vignetteMultiplier;
    Vector3 lastSelfPos, lastToPos;
    float minVignette;
    Vignette vignette;

    void Awake()
    {
        _ = volume.profile.TryGet<Vignette>(out vignette);
        minVignette = vignette.intensity.value;
        RecalculateDistanceMultiplier();
    }

    void OnDisable()
    {
        CinematicManager.instance.StopCameraShake();
        //vignette.intensity.value = minVignette;
    }

    void Update()
    {
        if (transform.position == lastSelfPos && distanceTo.position == lastToPos)
            return;
        
        lastToPos = distanceTo.position;
        lastSelfPos = transform.position;

        var distance = Vector2.Distance(new Vector2(transform.position.x, transform.position.z), new Vector2(distanceTo.position.x, distanceTo.position.z));

        if (distance > minDistance)
        {
            CinematicManager.instance.StopCameraShake();
            vignette.intensity.value = minVignette;
            return;
        }

        CinematicManager.instance.StartCameraShake(shakeMultiplier * Mathf.Pow(minDistance - distance, 2));
        vignette.intensity.value = minVignette + vignetteMultiplier * Mathf.Pow(minDistance - distance, 2);
    }

    public void ChangeMinDistance(float newMinDistance)
    {
        minDistance = newMinDistance;
        RecalculateDistanceMultiplier();
    }

    public void ChangeMaxShake(float newMaxShake)
    {
        maxShake = newMaxShake;
        RecalculateDistanceMultiplier();
    }

    void RecalculateDistanceMultiplier()
    {
        shakeMultiplier = maxShake / Mathf.Pow(minDistance - 22.5f, 2);
        vignetteMultiplier = (maxVignette - minVignette) / Mathf.Pow(minDistance - 22.5f, 2);
    }
}
