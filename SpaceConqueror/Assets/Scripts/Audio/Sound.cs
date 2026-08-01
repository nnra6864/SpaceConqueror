using UnityEngine;
using NnUnityEasings;

[CreateAssetMenu(fileName = "Sound", menuName = "Scriptable Objects/Sound")]
public class Sound : ScriptableObject
{
    public string Name;
    public AudioClip Clip;
    public float Volume = 1;
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
