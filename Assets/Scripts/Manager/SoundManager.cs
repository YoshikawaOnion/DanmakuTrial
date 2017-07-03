using UnityEngine;
using System.Collections;
using System;

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
	private AudioClip playerDefeatedSound;
	[SerializeField]
	private AudioClip enemyDefeatedSound;

    private AudioSource audioSource { get; set; }

    protected override void Init()
    {
        audioSource = GetComponent<AudioSource>();
    }
}
