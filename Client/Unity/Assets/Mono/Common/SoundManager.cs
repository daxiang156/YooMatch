using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    public class SoundManager: MonoBehaviour
    {
        public static SoundManager Instance;

        // Use this for initialization
        public AudioSource musicBGM; //背景音乐播放
        public AudioSource musicUI; //音效播放
        public float volumeMusic = 0.6f;
        public float volumeUI = 1;

        private Dictionary<String, AudioClip> audioDic = new Dictionary<string, AudioClip>();

        private void Awake()
        {
            Instance = this;
            PlayBgMusic("Music/bgm");
            this.volumeMusic = PlayerPrefs.GetFloat(EventName.VolumeMusic, this.volumeMusic);
            this.volumeUI = PlayerPrefs.GetFloat(EventName.VolumeUI, this.volumeUI);
            this.musicBGM.volume = this.volumeMusic;
            this.musicUI.volume = this.volumeUI;
        }

        public float GetMusicVolume()
        {
            return this.volumeMusic;
        }
        
        public float GetUIVolume()
        {
            return this.volumeUI;
        }

        public void PlayBgMusic(string name, bool isLoop = true)
        {       
            AudioClip audioClip = Resources.Load<AudioClip>(name);
            musicBGM.clip = audioClip;
            musicBGM.Play();
            musicBGM.loop = isLoop;
        }

        public void StopBgMusic()
        {
            if (musicBGM.isPlaying)
            {
                musicBGM.Stop();
            }
        }

        //播放音效
        public void PlayActionSound(string name)
        {
            if (audioDic.ContainsKey(name))
            {
                musicUI.PlayOneShot(audioDic[name]);
            }
            else
            {
                AudioClip audioClip = Resources.Load<AudioClip>(name);
                musicUI.PlayOneShot(audioClip);
                this.audioDic.Add(name, audioClip);
            }
        }

        public void PlayAudio(AudioClip audioClip)
        {
            musicUI.PlayOneShot(audioClip);
        }

        public void StopActionSound(string name)
        {
            musicUI.Stop();
        }

        public void SetMusicVolume(float volume)
        {
            this.volumeMusic = volume;
            PlayerPrefs.SetFloat(EventName.VolumeMusic, this.volumeMusic);
            this.musicBGM.volume = this.volumeMusic;
        }
        
        public void SetUIVolume(float volume)
        {
            this.volumeUI = volume;
            PlayerPrefs.SetFloat(EventName.VolumeUI, this.volumeUI);
            this.musicUI.volume = this.volumeUI;
        }
    }
}