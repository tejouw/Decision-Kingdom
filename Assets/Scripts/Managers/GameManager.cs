using System;
using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;

namespace DecisionKingdom.Managers
{
    /// <summary>
    /// Ana oyun yöneticisi - Singleton
    /// </summary>
    public class GameManager : MonoBehaviour
    {
        public static GameManager Instance { get; private set; }

        [Header("Oyun Durumu")]
        [SerializeField] private GameStateData _gameState;

        [Header("Referanslar")]
        [SerializeField] private ResourceManager _resourceManager;

        // Events
        public event Action<GameState> OnGameStateChanged;
        public event Action<int> OnTurnAdvanced;
        public event Action<Era> OnEraChanged;
        public event Action<GameOverReason> OnGameOver;
        public event Action OnNewGameStarted;
        public event Action<GameEvent> OnEventProcessed;

        #region Properties
        public GameStateData CurrentGameState => _gameState;
        public GameState State => _gameState.gameState;
        public int CurrentTurn => _gameState.turn;
        public Era CurrentEra => _gameState.era;
        public bool IsPlaying => _gameState.gameState == GameState.Playing;
        public bool IsPaused => _gameState.gameState == GameState.Paused;
        public bool IsGameOver => _gameState.gameState == GameState.GameOver;
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

            InitializeGameState();
        }

        private void Start()
        {
            // ResourceManager'ı bul veya referansı al
            if (_resourceManager == null)
            {
                _resourceManager = ResourceManager.Instance;
            }

            // Game Over event'ini dinle
            if (_resourceManager != null)
            {
                _resourceManager.OnGameOver += HandleResourceGameOver;
            }
        }

        private void OnDestroy()
        {
            if (_resourceManager != null)
            {
                _resourceManager.OnGameOver -= HandleResourceGameOver;
            }
        }
        #endregion

        #region Public Methods - Game Control
        /// <summary>
        /// Yeni oyun başlat
        /// </summary>
        public void StartNewGame(Era era = Era.Medieval, Resources customResources = null)
        {
            _gameState.StartNewGame(era, customResources);

            // ResourceManager'ı güncelle
            if (_resourceManager != null)
            {
                if (customResources != null)
                {
                    _resourceManager.LoadResources(customResources);
                }
                else
                {
                    _resourceManager.ResetResources();
                }
            }

            SetGameState(GameState.Playing);
            OnNewGameStarted?.Invoke();

            Debug.Log($"[GameManager] Yeni oyun başladı - Dönem: {era}, Tur: {_gameState.turn}");
        }

        /// <summary>
        /// Oyunu duraklat
        /// </summary>
        public void PauseGame()
        {
            if (_gameState.gameState == GameState.Playing)
            {
                SetGameState(GameState.Paused);
                Debug.Log("[GameManager] Oyun duraklatıldı");
            }
        }

        /// <summary>
        /// Oyuna devam et
        /// </summary>
        public void ResumeGame()
        {
            if (_gameState.gameState == GameState.Paused)
            {
                SetGameState(GameState.Playing);
                Debug.Log("[GameManager] Oyun devam ediyor");
            }
        }

        /// <summary>
        /// Oyunu bitir
        /// </summary>
        public void EndGame(GameOverReason reason)
        {
            _gameState.gameState = GameState.GameOver;
            _gameState.gameOverReason = reason;

            OnGameStateChanged?.Invoke(GameState.GameOver);
            OnGameOver?.Invoke(reason);

            Debug.Log($"[GameManager] Oyun bitti - Sebep: {reason}, Tur: {_gameState.turn}");
        }

        /// <summary>
        /// Ana menüye dön
        /// </summary>
        public void ReturnToMainMenu()
        {
            SetGameState(GameState.MainMenu);
            Debug.Log("[GameManager] Ana menüye dönüldü");
        }
        #endregion

        #region Public Methods - Turn Management
        /// <summary>
        /// Turu ilerlet
        /// </summary>
        public void AdvanceTurn()
        {
            _gameState.AdvanceTurn();
            OnTurnAdvanced?.Invoke(_gameState.turn);

            Debug.Log($"[GameManager] Tur: {_gameState.turn}");
        }

        /// <summary>
        /// Dönemi değiştir
        /// </summary>
        public void ChangeEra(Era newEra)
        {
            Era oldEra = _gameState.era;
            _gameState.era = newEra;
            OnEraChanged?.Invoke(newEra);

            Debug.Log($"[GameManager] Dönem değişti: {oldEra} -> {newEra}");
        }
        #endregion

        #region Public Methods - Event Processing
        /// <summary>
        /// Event'i işle (seçim yapıldıktan sonra)
        /// </summary>
        public void ProcessEventChoice(GameEvent gameEvent, bool choseRight)
        {
            if (gameEvent == null) return;

            Choice choice = choseRight ? gameEvent.rightChoice : gameEvent.leftChoice;

            // Kaynak efektlerini uygula
            if (_resourceManager != null && choice.effects != null)
            {
                _resourceManager.ApplyChoice(choice);
            }

            // Event'i geçmişe ekle
            _gameState.RecordEvent(gameEvent.id);

            // Tetiklenen event'leri kuyruğa ekle
            if (choice.triggeredEventIds != null)
            {
                foreach (string eventId in choice.triggeredEventIds)
                {
                    _gameState.QueueTriggeredEvent(eventId);
                }
            }

            // Flag'leri ayarla
            if (choice.flags != null)
            {
                foreach (string flag in choice.flags)
                {
                    _gameState.AddFlag(flag);
                }
            }

            // Karakter etkileşimini kaydet
            if (!string.IsNullOrEmpty(gameEvent.CharacterId))
            {
                CharacterState charState = _gameState.GetOrCreateCharacterState(gameEvent.CharacterId);
                charState.RecordInteraction(_gameState.turn);
                charState.ModifyRelationship(choice.relationshipChange);
            }

            OnEventProcessed?.Invoke(gameEvent);

            // Turu ilerlet
            AdvanceTurn();
        }

        /// <summary>
        /// Sıradaki tetiklenmiş event ID'sini al
        /// </summary>
        public string GetNextTriggeredEventId()
        {
            return _gameState.DequeueTriggeredEvent();
        }

        /// <summary>
        /// Tetiklenmiş event var mı?
        /// </summary>
        public bool HasTriggeredEvents()
        {
            return _gameState.HasTriggeredEvents;
        }
        #endregion

        #region Public Methods - State Queries
        /// <summary>
        /// Flag'i kontrol et
        /// </summary>
        public bool HasFlag(string flag)
        {
            return _gameState.HasFlag(flag);
        }

        /// <summary>
        /// Flag ekle
        /// </summary>
        public void AddFlag(string flag)
        {
            _gameState.AddFlag(flag);
        }

        /// <summary>
        /// Flag kaldır
        /// </summary>
        public void RemoveFlag(string flag)
        {
            _gameState.RemoveFlag(flag);
        }

        /// <summary>
        /// Karakter durumunu al
        /// </summary>
        public CharacterState GetCharacterState(string characterId)
        {
            return _gameState.GetOrCreateCharacterState(characterId);
        }

        /// <summary>
        /// Event daha önce oynandı mı?
        /// </summary>
        public bool HasPlayedEvent(string eventId)
        {
            return _gameState.HasPlayedEvent(eventId);
        }

        /// <summary>
        /// Zorluk çarpanını al
        /// </summary>
        public float GetDifficultyMultiplier()
        {
            return _gameState.GetDifficultyMultiplier();
        }

        /// <summary>
        /// Prestige puanını hesapla
        /// </summary>
        public int CalculatePrestigePoints()
        {
            return _gameState.CalculatePrestigePoints();
        }

        /// <summary>
        /// Oyun süresini al
        /// </summary>
        public TimeSpan GetSessionDuration()
        {
            return _gameState.GetSessionDuration();
        }
        #endregion

        #region Private Methods
        private void InitializeGameState()
        {
            _gameState = new GameStateData();
        }

        private void SetGameState(GameState newState)
        {
            if (_gameState.gameState != newState)
            {
                _gameState.gameState = newState;
                OnGameStateChanged?.Invoke(newState);
            }
        }

        private void HandleResourceGameOver(GameOverReason reason)
        {
            EndGame(reason);
        }
        #endregion

        #region Save/Load Support
        /// <summary>
        /// Oyun durumunu al (save için)
        /// </summary>
        public GameStateData GetGameStateForSave()
        {
            // Kaynakları senkronize et
            if (_resourceManager != null)
            {
                _gameState.resources = new Resources(_resourceManager.CurrentResources);
            }

            return _gameState;
        }

        /// <summary>
        /// Oyun durumunu yükle (load için)
        /// </summary>
        public void LoadGameState(GameStateData loadedState)
        {
            _gameState = loadedState;

            // ResourceManager'ı güncelle
            if (_resourceManager != null)
            {
                _resourceManager.LoadResources(_gameState.resources);
            }

            OnGameStateChanged?.Invoke(_gameState.gameState);

            Debug.Log($"[GameManager] Oyun yüklendi - Tur: {_gameState.turn}, Dönem: {_gameState.era}");
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Start New Game (Medieval)")]
        private void DebugStartNewGame()
        {
            StartNewGame(Era.Medieval);
        }

        [ContextMenu("Advance 10 Turns")]
        private void DebugAdvance10Turns()
        {
            for (int i = 0; i < 10; i++)
            {
                AdvanceTurn();
            }
        }

        [ContextMenu("Trigger Game Over (Revolution)")]
        private void DebugTriggerGameOver()
        {
            EndGame(GameOverReason.Revolution);
        }

        [ContextMenu("Print Game State")]
        private void DebugPrintGameState()
        {
            Debug.Log($"Turn: {_gameState.turn}");
            Debug.Log($"Era: {_gameState.era}");
            Debug.Log($"State: {_gameState.gameState}");
            Debug.Log($"Resources: {_gameState.resources}");
            Debug.Log($"Flags: {string.Join(", ", _gameState.flags)}");
            Debug.Log($"Event History Count: {_gameState.eventHistory.Count}");
        }
#endif
        #endregion
    }
}
