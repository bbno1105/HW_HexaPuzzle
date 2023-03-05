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
    [Header("���� ���ҽ�")]
    [Tooltip("���ӿ� ���� BGM ������ ��������ּ���.")]
    [SerializeField] Sound[] bgmSoundList;
    [Tooltip("���ӿ� ���� SE ������ ��������ּ���.")]
    [SerializeField] Sound[] seSoundList;

    [Tooltip("���� ���۽� �ڵ����� �Ҵ�˴ϴ�.")]
    [Header("����� �ҽ�")]
    [SerializeField] AudioSource bgmPlayer;
    public float BGMValue { get { return bgmPlayer.volume; } }

    [Tooltip("���� ���۽� �ڵ����� �Ҵ�˴ϴ�.")]
    [SerializeField] AudioSource[] sePlayer;
    public float SEValue { get { return sePlayer[0].volume; } }

    void Awake()
    {
        AudioSource[] bgmAudioPlayer = transform.GetChild(0).GetComponents<AudioSource>();
        bgmPlayer = bgmAudioPlayer[0]; // BGM�� ������ �ϳ�

        AudioSource[] seAudioPlayer = transform.GetChild(1).GetComponents<AudioSource>();
        sePlayer = new AudioSource[seAudioPlayer.Length];
        for (int i = 0; i < sePlayer.Length; i++) // �������� SE
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
