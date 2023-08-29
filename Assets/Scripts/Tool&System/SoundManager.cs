using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.PlayerSettings;

public enum SoundEnum
{
	BGM,
	Effect
}

public class SoundManager : MonoSingleton<SoundManager>
{
	[SerializeField]
	private AudioSource _bgmSource;

	[SerializeField]
	private AudioSource _effectSourceObject;

	private List<AudioSource> _effectSources;
	public void BGMClipChange(AudioClip source)
	{
		_bgmSource.Stop();
		_bgmSource.clip = source;
		_bgmSource.Play();
	}

	public void EffectPlay(AudioClip clip, Vector3 pos)
	{
		GameObject obj = Instantiate(_effectSourceObject.gameObject, this.transform);
		obj.transform.position = pos;
	}

	public void EffectStop(AudioClip clip, Vector3 vec)
	{
		foreach(AudioSource source in _effectSources) 
		{
			 if(source.transform.position == vec)
			{
				source.clip = clip;
				return;
			}
		}
	}
}