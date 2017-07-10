#pragma warning disable CS0649

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum SeKind
{
	Start, Win, PlayerShot, EnemyShot, Hit,
    PlayerDefeated, EnemyDamaged,
    PlayerDamaged
}

public enum BgmKind
{
	Game, Title
}

public class SoundManager : Singleton<SoundManager>
{
    private AudioSource seAudioSource { get; set; }
    private AudioSource bgmAudioSource { get; set; }

    protected override void Init()
    {
        var audioSources = GetComponents<AudioSource>();
        seAudioSource = audioSources[0];
        bgmAudioSource = audioSources[1];
    }

    protected override void OnDestroy()
    {
        seAudioSource = null;
        bgmAudioSource = null;
    }

    /// <summary>
    /// 指定した効果音やBGMを再生します。
    /// </summary>
    /// <returns>The play.</returns>
    /// <param name="kind">Kind.</param>
    public void PlaySe(SeKind kind, float volumeScale = 1)
    {
        var clip = Resources.Load<AudioClip>("Sounds/" + kind.ToString());
        seAudioSource.PlayOneShot(clip, volumeScale);
    }

    public void PlayBgm(BgmKind kind)
    {
        if (bgmAudioSource.isPlaying)
        {
            bgmAudioSource.Stop();
        }

        var clip = Resources.Load<AudioClip>("Sounds/Bgm" + kind.ToString());
        bgmAudioSource.clip = clip;
        bgmAudioSource.Play();
    }

    public void StopBgm(BgmKind kind)
    {
        bgmAudioSource.Stop();
    }
}
