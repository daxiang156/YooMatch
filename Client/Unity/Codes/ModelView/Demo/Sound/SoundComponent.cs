using System;
using System.Collections.Generic;
using UnityEngine;

namespace ET
{
    [ObjectSystem]
    public class SoundComponentAwakeSystem: AwakeSystem<SoundComponent>
    {
        public override void Awake(SoundComponent self)
        {
            self.Awake();
        }
    }
    public class SoundComponent : Entity, IAwake
    {
        public static SoundComponent Instance;
        public AudioSource musicBGM; //背景音乐播放
        public AudioSource musicUI; //音效播放
        public float volumeMusic = 0.42f;
        public float volumeUI = 0.8f;
        private Dictionary<String, AudioClip> audioDic = new Dictionary<string, AudioClip>();
        public void Awake()
        {
            Instance = this;
            OnInit();
            OnAddListner();
            PlayBgMusic_Res("Music/bgm");
        }

        public void OnInit()
        {
            // if(Init.Global == null)
            //     return;
            this.musicBGM = GlobalComponent.Instance.Global.GetComponent<AudioSource>();
            GameObject uiCamera = GlobalComponent.Instance.UICamera.gameObject;
            this.musicUI = uiCamera.GetComponent<AudioSource>();
            // PlayerPrefs.SetFloat(EventName.VolumeMusic, 0.42f);
            // PlayerPrefs.SetFloat(EventName.VolumeUI, 0.8f);
            this.volumeMusic = PlayerPrefs.GetFloat(EventName.VolumeMusic, this.volumeMusic);
            this.volumeUI = PlayerPrefs.GetFloat(EventName.VolumeUI, this.volumeUI);
            this.musicBGM.volume = this.volumeMusic;
            this.musicUI.volume = this.volumeUI;
        }

        private void OnAddListner()
        {
            EventDispatcher.AddObserver(this, ETEventName.StopBgMusic, (object[] info) =>
            {
                this.StopBgMusic();
                return false;
            }, null);
            EventDispatcher.AddObserver(this, ETEventName.PlayActionSound, (object[] info) =>
            {
                string abName = info[0].ToString();
                string musicName = info[1].ToString();
                this.PlayActionSound(abName,musicName);
                return false;
            }, null);
            EventDispatcher.AddObserver(this,EventName.ADAlreadyLoad, (object[] info) =>
            {
                if(GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                    this.StopBgMusic();
                return false;
            },null);
            EventDispatcher.AddObserver(this,EventName.SendGoogleAdsReward, (object[] info) =>
            {
                if(GameDataMgr.Instance.Platflam() == PlatForm.IOS)
                    ReplayBgMusic();
                return false;
            },null);
        }

        public void RemoveListner()
        {
            EventDispatcher.RemoveObserver(this, ETEventName.StopBgMusic,null);
            EventDispatcher.RemoveObserver(this, ETEventName.PlayActionSound,null);
            EventDispatcher.RemoveObserver(this, EventName.ADAlreadyLoad,null);
            EventDispatcher.RemoveObserver(this, EventName.SendGoogleAdsReward,null);
        }

        public float GetMusicVolume()
        {
            return this.volumeMusic;
        }
        
        public float GetUIVolume()
        {
            return this.volumeUI;
        }
        public void StopBgMusic()
        {
            if (musicBGM.isPlaying)
            {
                musicBGM.Stop();
            }
        }

        public void ReplayBgMusic()
        {
            if (musicBGM != null && !musicBGM.isPlaying)
            {
                this.musicBGM.Play();
            }
        }

        private void PlayBgSound(AudioClip audioClip,bool isLoop = true)
        {
            if(this.musicBGM == null)
                return;
            musicBGM.clip = audioClip;
            musicBGM.Play();
            musicBGM.loop = isLoop;
        }

        private void PlayUISound(AudioClip audioClip,string musicName)
        {
            if(this.audioDic == null)
                return;
            if (audioDic.ContainsKey(musicName))
            {
                musicUI.PlayOneShot(audioDic[musicName]);
            }
            else
            {
                musicUI.PlayOneShot(audioClip);
                this.audioDic.Add(musicName, audioClip);
            }
        }
        /// <summary>
        /// 从Resource开始加载播放
        /// </summary>
        /// <param name="musicName"></param>
        /// <param name="isLoop"></param>
        public async void PlayBgMusic_Res(string musicName = "",bool isLoop = true)
        {
            if(musicBGM == null)
                return;
            AudioClip audioClip = Resources.Load<AudioClip>(musicName);
            musicBGM.clip = audioClip;
            musicBGM.Play();
            musicBGM.loop = isLoop;
            await ETTask.CompletedTask;
        }

        /// <summary>
        /// 从DB里播放
        /// </summary>
        /// <param name="abName"></param>
        /// <param name="musicName"></param>
        /// <param name="isLoop"></param>
        public void PlayBgMusic(string abName = "",string musicName = "",bool isLoop = true)
        {
            // abName = abName.ToLower() + ".unity3d";
            // ResourcesLoaderComponent loadComponent =  GlobalComponent.Instance.scene.GetComponent<ResourcesLoaderComponent>();
            // await loadComponent.LoadAsync(abName);
            // AudioClip audioClip = (AudioClip)ResourcesComponent.Instance.GetAsset(abName,musicName);
            // if (audioClip != null)
            // {
            //     PlayBgSound(audioClip,isLoop);
            // }
        }
        
        //播放音效
        public void PlayActionSound(string abName = "",string musicName = "")
        {
            //Log.Console("musicName:" + musicName);
            // abName = abName.ToLower() + ".unity3d";
            // ResourcesLoaderComponent loadComponent =  GlobalComponent.Instance.scene.GetComponent<ResourcesLoaderComponent>();
            // await loadComponent.LoadAsync(abName);
            // AudioClip audioClip = (AudioClip)ResourcesComponent.Instance.GetAsset(abName,musicName);
            // if (audioClip != null)
            // {
            //     PlayUISound(audioClip,musicName);
            // }
        }
    }
}