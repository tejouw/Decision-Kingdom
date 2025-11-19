using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DecisionKingdom.Core;
using DecisionKingdom.Data;
using DecisionKingdom.Managers;

namespace DecisionKingdom.Events
{
    /// <summary>
    /// Event seçim algoritması
    /// </summary>
    public class EventSelector : MonoBehaviour
    {
        public static EventSelector Instance { get; private set; }

        [Header("Event Database")]
        [SerializeField] private EventDatabase _eventDatabase;

        [Header("Seçim Ayarları")]
        [SerializeField] private float _chainEventPriority = 100f;
        [SerializeField] private float _characterEventBonus = 1.5f;
        [SerializeField] private float _rareEventChance = Constants.RARE_EVENT_CHANCE;

        private Queue<string> _priorityEventQueue = new Queue<string>();

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
        }
        #endregion

        #region Public Methods
        /// <summary>
        /// Sonraki event'i seç
        /// </summary>
        public GameEvent SelectNextEvent(GameStateData gameState)
        {
            // 1. Öncelikli event kuyruğunu kontrol et
            if (_priorityEventQueue.Count > 0)
            {
                string eventId = _priorityEventQueue.Dequeue();
                var priorityEvent = _eventDatabase.GetEventById(eventId);
                if (priorityEvent != null)
                {
                    return priorityEvent;
                }
            }

            // 2. Tetiklenmiş event var mı?
            if (gameState.HasTriggeredEvents)
            {
                string triggeredId = gameState.DequeueTriggeredEvent();
                var triggeredEvent = _eventDatabase.GetEventById(triggeredId);
                if (triggeredEvent != null && triggeredEvent.CheckConditions(gameState))
                {
                    return triggeredEvent;
                }
            }

            // 3. Zincir event kontrolü
            if (gameState.eventHistory.Count > 0)
            {
                string lastEventId = gameState.eventHistory[gameState.eventHistory.Count - 1];
                var chainEvent = _eventDatabase.GetChainEvent(lastEventId);
                if (chainEvent != null && chainEvent.CheckConditions(gameState))
                {
                    return chainEvent;
                }
            }

            // 4. Nadir event şansı
            if (Random.value < _rareEventChance)
            {
                var rareEvent = SelectRareEvent(gameState);
                if (rareEvent != null)
                {
                    return rareEvent;
                }
            }

            // 5. Normal event seçimi
            return SelectWeightedEvent(gameState);
        }

        /// <summary>
        /// Öncelikli event kuyruğuna ekle
        /// </summary>
        public void QueuePriorityEvent(string eventId)
        {
            _priorityEventQueue.Enqueue(eventId);
        }

        /// <summary>
        /// Öncelikli event kuyruğunu temizle
        /// </summary>
        public void ClearPriorityQueue()
        {
            _priorityEventQueue.Clear();
        }
        #endregion

        #region Private Methods
        private GameEvent SelectWeightedEvent(GameStateData gameState)
        {
            // Uygun eventleri al
            var availableEvents = GetAvailableEvents(gameState);

            if (availableEvents.Count == 0)
            {
                Debug.LogWarning("[EventSelector] No available events!");
                return null;
            }

            // Ağırlıkları hesapla
            var weightedEvents = CalculateWeights(availableEvents, gameState);

            // Ağırlıklı rastgele seçim
            float totalWeight = weightedEvents.Sum(w => w.weight);
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var (evt, weight) in weightedEvents)
            {
                currentWeight += weight;
                if (randomValue <= currentWeight)
                {
                    return evt;
                }
            }

            return availableEvents[0];
        }

        private GameEvent SelectRareEvent(GameStateData gameState)
        {
            var rareEvents = _eventDatabase.Events
                .Where(e => e.isRare)
                .Where(e => e.era == gameState.era)
                .Where(e => e.CheckConditions(gameState))
                .Where(e => !gameState.HasPlayedEvent(e.id))
                .ToList();

            if (rareEvents.Count == 0)
                return null;

            return rareEvents[Random.Range(0, rareEvents.Count)];
        }

        private List<GameEvent> GetAvailableEvents(GameStateData gameState)
        {
            return _eventDatabase.Events
                .Where(e => e.era == gameState.era)
                .Where(e => e.CheckConditions(gameState))
                .Where(e => !gameState.HasPlayedEvent(e.id) || e.category == EventCategory.Random)
                .Where(e => !e.isRare) // Nadir eventler ayrı seçilir
                .ToList();
        }

        private List<(GameEvent evt, float weight)> CalculateWeights(List<GameEvent> events, GameStateData gameState)
        {
            var result = new List<(GameEvent, float)>();

            foreach (var evt in events)
            {
                float weight = evt.weight;

                // Öncelik bonusu
                weight += evt.priority;

                // Karakter event bonusu
                if (evt.character != null)
                {
                    var charState = gameState.GetOrCreateCharacterState(evt.CharacterId);

                    // Daha önce etkileşim varsa bonus
                    if (charState.interactionCount > 0)
                    {
                        weight *= _characterEventBonus;
                    }

                    // Uzun süredir görülmemişse bonus
                    int turnsSinceLastSeen = gameState.turn - charState.lastInteractionTurn;
                    if (turnsSinceLastSeen > 10)
                    {
                        weight *= 1.2f;
                    }
                }

                // Zorluk çarpanı (geç oyunda daha zorlu eventler)
                if (evt.category == EventCategory.Story)
                {
                    weight *= gameState.GetDifficultyMultiplier();
                }

                result.Add((evt, Mathf.Max(weight, 0.1f)));
            }

            // Önceliğe göre sırala
            return result.OrderByDescending(x => x.Item2).ToList();
        }
        #endregion

        #region Debug Methods
#if UNITY_EDITOR
        [ContextMenu("Print Available Events")]
        private void DebugPrintAvailableEvents()
        {
            if (GameManager.Instance == null)
            {
                Debug.Log("GameManager not found");
                return;
            }

            var events = GetAvailableEvents(GameManager.Instance.CurrentGameState);
            Debug.Log($"Available events: {events.Count}");

            foreach (var evt in events.Take(10))
            {
                Debug.Log($"- {evt.id}: {evt.text.Truncate(50)}");
            }
        }
#endif
        #endregion
    }

    // Extension method for string truncation
    public static class StringExtensions
    {
        public static string Truncate(this string str, int maxLength)
        {
            if (string.IsNullOrEmpty(str) || str.Length <= maxLength)
                return str;
            return str.Substring(0, maxLength) + "...";
        }
    }
}
