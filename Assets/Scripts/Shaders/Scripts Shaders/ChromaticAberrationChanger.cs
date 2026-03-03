using System.Collections;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.HighDefinition;

public class ChromaticAberrationChanger : MonoBehaviour
{
    [Header("Volume Shaders")]
    public Volume globalVolume;
    public float transitionDuration = 3f; 

    [Header("Data")]
    [Range(0f, 1f)] public float maxChromatic = 1f;
    [Range(-1f, 1f)] public float maxLensDistortion = 0f;
    [Range(0f, 1f)] public float maxVignette = 0f;
    [Range(0f, 1f)] public float maxFilmGrain = 0.8f;
    [Range(-100f, 100f)] public float targetSaturation = -80f;

    private ChromaticAberration _chromatic;
    private LensDistortion _distortion;
    private Vignette _vignette;
    private FilmGrain _grain;
    private ColorAdjustments _color;

    private bool _hasTriggered = false;

    void Start()
    {
        if (globalVolume.profile.TryGet(out _chromatic)) _chromatic.intensity.value = 0f;
        if (globalVolume.profile.TryGet(out _distortion)) _distortion.intensity.value = 0f;
        if (globalVolume.profile.TryGet(out _vignette)) _vignette.intensity.value = 0f;
        if (globalVolume.profile.TryGet(out _grain)) _grain.intensity.value = 0f;

        if (globalVolume.profile.TryGet(out _color)) _color.saturation.value = 0f;

        var renderer = GetComponent<MeshRenderer>();
        if (renderer != null) renderer.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !_hasTriggered)
        {
            _hasTriggered = true;
            StopAllCoroutines();
            StartCoroutine(FadeMadnessEffects(true));
        }
    }

    IEnumerator FadeMadnessEffects(bool increasing)
    {
        float time = 0;
        float startChrom = _chromatic != null ? _chromatic.intensity.value : 0;
        float startDist = _distortion != null ? _distortion.intensity.value : 0;
        float startVig = _vignette != null ? _vignette.intensity.value : 0;
        float startGrain = _grain != null ? _grain.intensity.value : 0;
        float startSat = _color != null ? _color.saturation.value : 0;

        float targetChrom = increasing ? maxChromatic : 0;
        float targetDist = increasing ? maxLensDistortion : 0;
        float targetVig = increasing ? maxVignette : 0;
        float targetGrain = increasing ? maxFilmGrain : 0;
        float targetSatVal = increasing ? targetSaturation : 0;

        while (time < transitionDuration)
        {
            float t = time / transitionDuration;
            t = t * t * (3f - 2f * t);

            if (_chromatic != null) _chromatic.intensity.value = Mathf.Lerp(startChrom, targetChrom, t);
            if (_distortion != null) _distortion.intensity.value = Mathf.Lerp(startDist, targetDist, t);
            if (_vignette != null) _vignette.intensity.value = Mathf.Lerp(startVig, targetVig, t);
            if (_grain != null) _grain.intensity.value = Mathf.Lerp(startGrain, targetGrain, t);
            if (_color != null) _color.saturation.value = Mathf.Lerp(startSat, targetSatVal, t);

            time += Time.deltaTime;
            yield return null;
        }

        SetFinalValues(increasing);
    }

    void SetFinalValues(bool increased)
    {
        if (_chromatic != null) _chromatic.intensity.value = increased ? maxChromatic : 0;
        if (_distortion != null) _distortion.intensity.value = increased ? maxLensDistortion : 0;
        if (_vignette != null) _vignette.intensity.value = increased ? maxVignette : 0;
        if (_grain != null) _grain.intensity.value = increased ? maxFilmGrain : 0;
        if (_color != null) _color.saturation.value = increased ? targetSaturation : 0;
    }
}

