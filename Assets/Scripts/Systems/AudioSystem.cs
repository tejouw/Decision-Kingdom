using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Systems
{
    /// <summary>
    /// Ses ve muzik yonetim sistemi - Faz 8.2
    /// </summary>
    public class AudioSystem : MonoBehaviour
    {
        public static AudioSystem Instance { get; private set; }

        [Header("Audio Sources")]
        [SerializeField] private AudioSource _musicSource;
        [SerializeField] private AudioSource _sfxSource;
        [SerializeField] private AudioSource _ambientSource;

        [Header("Volume Settings")]
        [SerializeField] [Range(0f, 1f)] private float _masterVolume = 1f;
        [SerializeField] [Range(0f, 1f)] private float _musicVolume = 0.7f;
        [SerializeField] [Range(0f, 1f)] private float _sfxVolume = 1f;
        [SerializeField] [Range(0f, 1f)] private float _ambientVolume = 0.5f;

        [Header("Music Clips")]
        [SerializeField] private AudioClip _menuMusic;
        [SerializeField] private AudioClip _medievalMusic;
        [SerializeField] private AudioClip _renaissanceMusic;
        [SerializeField] private AudioClip _industrialMusic;
        [SerializeField] private AudioClip _modernMusic;
        [SerializeField] private AudioClip _futureMusic;
        [SerializeField] private AudioClip _gameOverMusic;
        [SerializeField] private AudioClip _victoryMusic;

        [Header("SFX Clips")]
        [SerializeField] private AudioClip _cardSwipeLeft;
        [SerializeField] private AudioClip _cardSwipeRight;
        [SerializeField] private AudioClip _cardAppear;
        [SerializeField] private AudioClip _buttonClick;
        [SerializeField] private AudioClip _resourceUp;
        [SerializeField] private AudioClip _resourceDown;
        [SerializeField] private AudioClip _resourceCritical;
        [SerializeField] private AudioClip _achievementUnlock;
        [SerializeField] private AudioClip _gameOver;
        [SerializeField] private AudioClip _victory;
        [SerializeField] private AudioClip _turnAdvance;
        [SerializeField] private AudioClip _eraChange;

        [Header("Ambient Clips")]
        [SerializeField] private AudioClip _medievalAmbient;
        [SerializeField] private AudioClip _renaissanceAmbient;
        [SerializeField] private AudioClip _industrialAmbient;
        [SerializeField] private AudioClip _modernAmbient;
        [SerializeField] private AudioClip _futureAmbient;

        [Header("Fade Settings")]
        [SerializeField] private float _musicFadeDuration = 1.5f;
        [SerializeField] private float _ambientFadeDuration = 2f;

        // Events
        public event Action<float> OnMasterVolumeChanged;
        public event Action<float> OnMusicVolumeChanged;
        public event Action<float> OnSfxVolumeChanged;

        private Era _currentEra;
        private bool _isMuted;
        private Dictionary<SoundEffect, AudioClip> _sfxClips;

        private const string MASTER_VOLUME_KEY = "MasterVolume";
        private const string MUSIC_VOLUME_KEY = "MusicVolume";
        private const string SFX_VOLUME_KEY = "SfxVolume";
        private const string MUTED_KEY = "IsMuted";

        #region Properties
        public float MasterVolume => _masterVolume;
        public float MusicVolume => _musicVolume;
        public float SfxVolume => _sfxVolume;
        public float AmbientVolume => _ambientVolume;
        public bool IsMuted => _isMuted;
        public bool IsMusicPlaying => _musicSource != null && _musicSource.isPlaying;
        #endregion

        #region Unity Lifecycle
        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            InitializeAudioSources();
            InitializeSfxDictionary();
            LoadVolumeSettings();
        }

        private void Start()
        {
            ApplyVolumeSettings();
        }
        #endregion

        #region Public Methods - Volume Control
        /// <summary>
        /// Master volume ayarla
        /// </summary>
        public void SetMasterVolume(float volume)
        {
            _masterVolume = Mathf.Clamp01(volume);
            ApplyVolumeSettings();
            SaveVolumeSettings();
            OnMasterVolumeChanged?.Invoke(_masterVolume);
        }

        /// <summary>
        /// Muzik volume ayarla
        /// </summary>
        public void SetMusicVolume(float volume)
        {
            _musicVolume = Mathf.Clamp01(volume);
            ApplyVolumeSettings();
            SaveVolumeSettings();
            OnMusicVolumeChanged?.Invoke(_musicVolume);
        }

        /// <summary>
        /// SFX volume ayarla
        /// </summary>
        public void SetSfxVolume(float volume)
        {
            _sfxVolume = Mathf.Clamp01(volume);
            ApplyVolumeSettings();
            SaveVolumeSettings();
            OnSfxVolumeChanged?.Invoke(_sfxVolume);
        }

        /// <summary>
        /// Ambient volume ayarla
        /// </summary>
        public void SetAmbientVolume(float volume)
        {
            _ambientVolume = Mathf.Clamp01(volume);
            ApplyVolumeSettings();
            SaveVolumeSettings();
        }

        /// <summary>
        /// Tum sesleri kapat/ac
        /// </summary>
        public void ToggleMute()
        {
            _isMuted = !_isMuted;
            ApplyVolumeSettings();
            SaveVolumeSettings();
        }

        /// <summary>
        /// Mute durumunu ayarla
        /// </summary>
        public void SetMuted(bool muted)
        {
            _isMuted = muted;
            ApplyVolumeSettings();
            SaveVolumeSettings();
        }
        #endregion

        #region Public Methods - Music
        /// <summary>
        /// Menu muzigini cal
        /// </summary>
        public void PlayMenuMusic()
        {
            PlayMusic(_menuMusic);
        }

        /// <summary>
        /// Era muzigini cal
        /// </summary>
        public void PlayEraMusic(Era era)
        {
            _currentEra = era;
            AudioClip clip = GetEraMusicClip(era);
            AudioClip ambient = GetEraAmbientClip(era);

            StartCoroutine(FadeToMusic(clip));
            StartCoroutine(FadeToAmbient(ambient));
        }

        /// <summary>
        /// Game over muzigini cal
        /// </summary>
        public void PlayGameOverMusic()
        {
            StartCoroutine(FadeToMusic(_gameOverMusic));
            StopAmbient();
        }

        /// <summary>
        /// Zafer muzigini cal
        /// </summary>
        public void PlayVictoryMusic()
        {
            StartCoroutine(FadeToMusic(_victoryMusic));
            StopAmbient();
        }

        /// <summary>
        /// Muzigi durdur
        /// </summary>
        public void StopMusic()
        {
            StartCoroutine(FadeOutMusic());
        }

        /// <summary>
        /// Muzigi duraklat
        /// </summary>
        public void PauseMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.Pause();
            }
        }

        /// <summary>
        /// Muzige devam et
        /// </summary>
        public void ResumeMusic()
        {
            if (_musicSource != null)
            {
                _musicSource.UnPause();
            }
        }
        #endregion

        #region Public Methods - SFX
        /// <summary>
        /// Ses efekti cal
        /// </summary>
        public void PlaySfx(SoundEffect effect)
        {
            if (_sfxSource == null || _isMuted)
                return;

            if (_sfxClips.TryGetValue(effect, out AudioClip clip) && clip != null)
            {
                _sfxSource.PlayOneShot(clip);
            }
        }

        /// <summary>
        /// Belirli bir AudioClip cal
        /// </summary>
        public void PlaySfx(AudioClip clip)
        {
            if (_sfxSource == null || _isMuted || clip == null)
                return;

            _sfxSource.PlayOneShot(clip);
        }

        /// <summary>
        /// Kart kaydirma sesi
        /// </summary>
        public void PlayCardSwipe(bool isRight)
        {
            PlaySfx(isRight ? SoundEffect.CardSwipeRight : SoundEffect.CardSwipeLeft);
        }

        /// <summary>
        /// Kaynak degisim sesi
        /// </summary>
        public void PlayResourceChange(int delta)
        {
            if (delta > 0)
                PlaySfx(SoundEffect.ResourceUp);
            else if (delta < 0)
                PlaySfx(SoundEffect.ResourceDown);
        }

        /// <summary>
        /// Kritik kaynak uyari sesi
        /// </summary>
        public void PlayResourceCritical()
        {
            PlaySfx(SoundEffect.ResourceCritical);
        }
        #endregion

        #region Private Methods
        private void InitializeAudioSources()
        {
            if (_musicSource == null)
            {
                var musicObj = new GameObject("MusicSource");
                musicObj.transform.SetParent(transform);
                _musicSource = musicObj.AddComponent<AudioSource>();
                _musicSource.loop = true;
                _musicSource.playOnAwake = false;
            }

            if (_sfxSource == null)
            {
                var sfxObj = new GameObject("SfxSource");
                sfxObj.transform.SetParent(transform);
                _sfxSource = sfxObj.AddComponent<AudioSource>();
                _sfxSource.loop = false;
                _sfxSource.playOnAwake = false;
            }

            if (_ambientSource == null)
            {
                var ambientObj = new GameObject("AmbientSource");
                ambientObj.transform.SetParent(transform);
                _ambientSource = ambientObj.AddComponent<AudioSource>();
                _ambientSource.loop = true;
                _ambientSource.playOnAwake = false;
            }
        }

        private void InitializeSfxDictionary()
        {
            _sfxClips = new Dictionary<SoundEffect, AudioClip>
            {
                { SoundEffect.CardSwipeLeft, _cardSwipeLeft },
                { SoundEffect.CardSwipeRight, _cardSwipeRight },
                { SoundEffect.CardAppear, _cardAppear },
                { SoundEffect.ButtonClick, _buttonClick },
                { SoundEffect.ResourceUp, _resourceUp },
                { SoundEffect.ResourceDown, _resourceDown },
                { SoundEffect.ResourceCritical, _resourceCritical },
                { SoundEffect.AchievementUnlock, _achievementUnlock },
                { SoundEffect.GameOver, _gameOver },
                { SoundEffect.Victory, _victory },
                { SoundEffect.TurnAdvance, _turnAdvance },
                { SoundEffect.EraChange, _eraChange }
            };
        }

        private AudioClip GetEraMusicClip(Era era)
        {
            return era switch
            {
                Era.Medieval => _medievalMusic,
                Era.Renaissance => _renaissanceMusic,
                Era.Industrial => _industrialMusic,
                Era.Modern => _modernMusic,
                Era.Future => _futureMusic,
                _ => _medievalMusic
            };
        }

        private AudioClip GetEraAmbientClip(Era era)
        {
            return era switch
            {
                Era.Medieval => _medievalAmbient,
                Era.Renaissance => _renaissanceAmbient,
                Era.Industrial => _industrialAmbient,
                Era.Modern => _modernAmbient,
                Era.Future => _futureAmbient,
                _ => _medievalAmbient
            };
        }

        private void PlayMusic(AudioClip clip)
        {
            if (_musicSource == null || clip == null)
                return;

            _musicSource.clip = clip;
            _musicSource.Play();
        }

        private void StopAmbient()
        {
            StartCoroutine(FadeOutAmbient());
        }

        private void ApplyVolumeSettings()
        {
            float effectiveMaster = _isMuted ? 0f : _masterVolume;

            if (_musicSource != null)
                _musicSource.volume = effectiveMaster * _musicVolume;

            if (_sfxSource != null)
                _sfxSource.volume = effectiveMaster * _sfxVolume;

            if (_ambientSource != null)
                _ambientSource.volume = effectiveMaster * _ambientVolume;
        }

        private void SaveVolumeSettings()
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, _masterVolume);
            PlayerPrefs.SetFloat(MUSIC_VOLUME_KEY, _musicVolume);
            PlayerPrefs.SetFloat(SFX_VOLUME_KEY, _sfxVolume);
            PlayerPrefs.SetInt(MUTED_KEY, _isMuted ? 1 : 0);
            PlayerPrefs.Save();
        }

        private void LoadVolumeSettings()
        {
            _masterVolume = PlayerPrefs.GetFloat(MASTER_VOLUME_KEY, 1f);
            _musicVolume = PlayerPrefs.GetFloat(MUSIC_VOLUME_KEY, 0.7f);
            _sfxVolume = PlayerPrefs.GetFloat(SFX_VOLUME_KEY, 1f);
            _isMuted = PlayerPrefs.GetInt(MUTED_KEY, 0) == 1;
        }
        #endregion

        #region Coroutines
        private IEnumerator FadeToMusic(AudioClip newClip)
        {
            if (_musicSource == null)
                yield break;

            // Fade out current
            float startVolume = _musicSource.volume;
            float timer = 0f;

            while (timer < _musicFadeDuration / 2)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / (_musicFadeDuration / 2));
                yield return null;
            }

            // Switch clip
            _musicSource.clip = newClip;
            if (newClip != null)
            {
                _musicSource.Play();
            }

            // Fade in
            float targetVolume = (_isMuted ? 0f : _masterVolume) * _musicVolume;
            timer = 0f;

            while (timer < _musicFadeDuration / 2)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(0f, targetVolume, timer / (_musicFadeDuration / 2));
                yield return null;
            }

            _musicSource.volume = targetVolume;
        }

        private IEnumerator FadeOutMusic()
        {
            if (_musicSource == null)
                yield break;

            float startVolume = _musicSource.volume;
            float timer = 0f;

            while (timer < _musicFadeDuration)
            {
                timer += Time.deltaTime;
                _musicSource.volume = Mathf.Lerp(startVolume, 0f, timer / _musicFadeDuration);
                yield return null;
            }

            _musicSource.Stop();
        }

        private IEnumerator FadeToAmbient(AudioClip newClip)
        {
            if (_ambientSource == null)
                yield break;

            // Fade out
            float startVolume = _ambientSource.volume;
            float timer = 0f;

            while (timer < _ambientFadeDuration / 2)
            {
                timer += Time.deltaTime;
                _ambientSource.volume = Mathf.Lerp(startVolume, 0f, timer / (_ambientFadeDuration / 2));
                yield return null;
            }

            // Switch
            _ambientSource.clip = newClip;
            if (newClip != null)
            {
                _ambientSource.Play();
            }

            // Fade in
            float targetVolume = (_isMuted ? 0f : _masterVolume) * _ambientVolume;
            timer = 0f;

            while (timer < _ambientFadeDuration / 2)
            {
                timer += Time.deltaTime;
                _ambientSource.volume = Mathf.Lerp(0f, targetVolume, timer / (_ambientFadeDuration / 2));
                yield return null;
            }

            _ambientSource.volume = targetVolume;
        }

        private IEnumerator FadeOutAmbient()
        {
            if (_ambientSource == null)
                yield break;

            float startVolume = _ambientSource.volume;
            float timer = 0f;

            while (timer < _ambientFadeDuration)
            {
                timer += Time.deltaTime;
                _ambientSource.volume = Mathf.Lerp(startVolume, 0f, timer / _ambientFadeDuration);
                yield return null;
            }

            _ambientSource.Stop();
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Play Medieval Music")]
        private void DebugPlayMedieval()
        {
            PlayEraMusic(Era.Medieval);
        }

        [ContextMenu("Play Button Click SFX")]
        private void DebugPlayClick()
        {
            PlaySfx(SoundEffect.ButtonClick);
        }
#endif
        #endregion
    }

    /// <summary>
    /// Ses efekti turleri
    /// </summary>
    public enum SoundEffect
    {
        CardSwipeLeft,
        CardSwipeRight,
        CardAppear,
        ButtonClick,
        ResourceUp,
        ResourceDown,
        ResourceCritical,
        AchievementUnlock,
        GameOver,
        Victory,
        TurnAdvance,
        EraChange
    }
}
