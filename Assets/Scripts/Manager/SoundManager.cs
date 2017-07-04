#pragma warning disable CS0649

using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public enum SeKind
{
	Start, Win, PlayerShot, EnemyShot, Hit,
    PlayerDamaged, EnemyDamaged, EnemyDefeated
}

public enum BgmKind
{
	Game,
}

public class SoundManager : Singleton<SoundManager>
{
    [SerializeField]
	private AudioClip startSound;
	[SerializeField]
	private AudioClip winSound;
	[SerializeField]
	private AudioClip playerShotSound;
	[SerializeField]
	private AudioClip enemyShotSound;
    [SerializeField]
    private AudioClip hitSound;
	[SerializeField]
    private AudioClip playerDamagedSound;
	[SerializeField]
	private AudioClip enemyDamagedSound;
	[SerializeField]
	private AudioClip enemyDefeatedSound;

    [SerializeField]
	private AudioSource soundEffectSource;
	[SerializeField]
    private AudioSource gameBgmSource;

	private Dictionary<SeKind, AudioClip> seAudioClips { get; set; }
	private Dictionary<BgmKind, AudioSource> bgmAudioSources { get; set; }

    protected override void Init()
    {
        seAudioClips = new Dictionary<SeKind, AudioClip>();
        seAudioClips[SeKind.Start] = startSound;
        seAudioClips[SeKind.Win] = winSound;
        seAudioClips[SeKind.PlayerShot] = playerShotSound;
        seAudioClips[SeKind.EnemyShot] = enemyShotSound;
        seAudioClips[SeKind.PlayerDamaged] = playerDamagedSound;
        seAudioClips[SeKind.EnemyDamaged] = enemyDamagedSound;
        seAudioClips[SeKind.EnemyDefeated] = enemyDefeatedSound;
        seAudioClips[SeKind.Hit] = hitSound;

		bgmAudioSources = new Dictionary<BgmKind, AudioSource>();
		bgmAudioSources[BgmKind.Game] = Instantiate(gameBgmSource);
    }

    /// <summary>
    /// 指定した効果音やBGMを再生します。
    /// </summary>
    /// <returns>The play.</returns>
    /// <param name="kind">Kind.</param>
    public void PlaySe(SeKind kind, float volumeScale = 1)
    {
        soundEffectSource.PlayOneShot(seAudioClips[kind], volumeScale);
    }

    public void PlayBgm(BgmKind kind)
    {
        bgmAudioSources[kind].Play();
    }

    public void StopBgm(BgmKind kind)
    {
        bgmAudioSources[kind].Stop();
    }
}
