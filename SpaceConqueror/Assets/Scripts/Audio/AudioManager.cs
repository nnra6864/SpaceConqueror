using UnityEngine;
using UnityCommunity.UnitySingleton;
using LitMotion;
using System.Collections.Generic;

public class AudioManager : MonoSingleton<AudioManager>
{
    // TODO: Implement pooling for positional sources
    private readonly Dictionary<Sound, AudioSource> _sources = new();

    public void Init(Sound[] sounds)
    {
        foreach(var sound in sounds)
            _sources.Add(sound, InitSource(gameObject.AddComponent<AudioSource>(), sound));
    }

    public void Play(Sound sound)
    {
        AudioSource source = _sources[sound];
        source.Play();
    }

    public void PlayAt(Sound sound, Vector3 pos)
    {
        AudioSource source = _sources[sound];
        source.Play();
    }

    private AudioSource InitSource(AudioSource source, Sound sound)
    {
        source.clip = sound.Clip;
        source.outputAudioMixerGroup = sound.MixerGroup;
        if (!sound.GetVolumeOnPlay)
            source.volume = Random.Range(sound.VolumeRange.x, sound.VolumeRange.y);
        if (!sound.GetPitchOnPlay)
            source.pitch = Random.Range(sound.PitchRange.x, sound.PitchRange.y);
        return ConfigureSource(source, sound);
    }

    private AudioSource ConfigureSource(AudioSource source, Sound sound)
    {
        if(sound.GetVolumeOnPlay)
            source.volume = Random.Range(sound.VolumeRange.x, sound.VolumeRange.y);
        if(sound.GetPitchOnPlay)
            source.pitch = Random.Range(sound.PitchRange.x, sound.PitchRange.y);
        source.loop = sound.Loop;
        source.spatialBlend = sound.SpatialBlend;

        return source;
    }
}
