using UnityEngine;
using NnUnityEasings;
using UnityEngine.Audio;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Objects/Sound")]
public class Sound : ScriptableObject
{
    public string Name;
    public AudioClip Clip;
    public AudioMixerGroup MixerGroup;
    public Vector2 VolumeRange = new(1, 1);
    public bool GetVolumeOnPlay;
    public Vector2 PitchRange = new(1, 1);
    public bool GetPitchOnPlay;
    public bool Unscaled;
    public bool Loop;
    public float SpatialBlend;
    public float FadeInTime;
    public Easing FadeInEasing;
    public float FadeOutTime;
    public Easing FadeOutEasing;
}
