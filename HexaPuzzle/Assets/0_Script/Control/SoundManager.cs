using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class Sound
{
    public string SoundName;
    public AudioClip Clip;
}

public class SoundManager : SingletonBehaviour<SoundManager>
{
    [Header("사운드 리소스")]
    [Tooltip("게임에 사용될 BGM 파일을 적용시켜주세요.")]
    [SerializeField] Sound[] bgmSoundList;
    [Tooltip("게임에 사용될 SE 파일을 적용시켜주세요.")]
    [SerializeField] Sound[] seSoundList;

    [Tooltip("게임 시작시 자동으로 할당됩니다.")]
    [Header("오디오 소스")]
    [SerializeField] AudioSource bgmPlayer;
    public float BGMValue { get { return bgmPlayer.volume; } }

    [Tooltip("게임 시작시 자동으로 할당됩니다.")]
    [SerializeField] AudioSource[] sePlayer;
    public float SEValue { get { return sePlayer[0].volume; } }

    void Awake()
    {
        AudioSource[] bgmAudioPlayer = transform.GetChild(0).GetComponents<AudioSource>();
        bgmPlayer = bgmAudioPlayer[0]; // BGM은 어차피 하나

        AudioSource[] seAudioPlayer = transform.GetChild(1).GetComponents<AudioSource>();
        sePlayer = new AudioSource[seAudioPlayer.Length];
        for (int i = 0; i < sePlayer.Length; i++) // 나머지는 SE
        {
            sePlayer[i] = seAudioPlayer[i];
        }
    }

    public void PlayBGM(string _soundName)
    {
        for (int i = 0; i < bgmSoundList.Length; i++)
        {
            if (bgmSoundList[i].SoundName.Equals(_soundName))
            {
                if (bgmPlayer.clip == bgmSoundList[i].Clip) return;
                bgmPlayer.clip = bgmSoundList[i].Clip;
                bgmPlayer.Play();
                return;
            }
        }
    }

    public void PlaySE(string _soundName)
    {
        for (int i = 0; i < seSoundList.Length; i++)
        {
            if (_soundName == seSoundList[i].SoundName)
            {
                for (int x = 0; x < sePlayer.Length; x++)
                {
                    if (sePlayer[x] == null) return;
                    if (sePlayer[x].isPlaying == false)
                    {
                        sePlayer[x].clip = seSoundList[i].Clip;
                        sePlayer[x].Play();
                        return;
                    }
                }
                return;
            }
        }
    }

    public void SetBGMVolume(float _volume)
    {
        bgmPlayer.volume = _volume;
    }

    public void SetSEVolume(float _volume)
    {
        for (int i = 0; i < sePlayer.Length; i++)
        {
            sePlayer[i].volume = _volume;
        }
    }
}
