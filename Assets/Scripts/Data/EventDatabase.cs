using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Data
{
    /// <summary>
    /// Event veritabanı - tüm eventleri yönetir
    /// </summary>
    [CreateAssetMenu(fileName = "EventDatabase", menuName = "Decision Kingdom/Event Database")]
    public class EventDatabase : ScriptableObject
    {
        [Header("Event Listesi")]
        [SerializeField] private List<GameEvent> _events = new List<GameEvent>();

        [Header("Karakter Listesi")]
        [SerializeField] private List<Character> _characters = new List<Character>();

        #region Properties
        public IReadOnlyList<GameEvent> Events => _events;
        public IReadOnlyList<Character> Characters => _characters;
        public int EventCount => _events.Count;
        public int CharacterCount => _characters.Count;
        #endregion

        #region Event Queries
        /// <summary>
        /// ID ile event bul
        /// </summary>
        public GameEvent GetEventById(string id)
        {
            return _events.FirstOrDefault(e => e.id == id);
        }

        /// <summary>
        /// Döneme göre eventleri filtrele
        /// </summary>
        public List<GameEvent> GetEventsByEra(Era era)
        {
            return _events.Where(e => e.era == era).ToList();
        }

        /// <summary>
        /// Kategoriye göre eventleri filtrele
        /// </summary>
        public List<GameEvent> GetEventsByCategory(EventCategory category)
        {
            return _events.Where(e => e.category == category).ToList();
        }

        /// <summary>
        /// Karaktere göre eventleri filtrele
        /// </summary>
        public List<GameEvent> GetEventsByCharacter(string characterId)
        {
            return _events.Where(e => e.CharacterId == characterId).ToList();
        }

        /// <summary>
        /// Uygun eventleri getir (koşulları sağlayan)
        /// </summary>
        public List<GameEvent> GetAvailableEvents(GameStateData gameState)
        {
            return _events
                .Where(e => e.era == gameState.era)
                .Where(e => e.CheckConditions(gameState))
                .Where(e => !gameState.HasPlayedEvent(e.id) || e.category == EventCategory.Random)
                .ToList();
        }

        /// <summary>
        /// Ağırlıklı rastgele event seç
        /// </summary>
        public GameEvent SelectRandomEvent(GameStateData gameState)
        {
            var availableEvents = GetAvailableEvents(gameState);

            if (availableEvents.Count == 0)
                return null;

            // Nadir event şansı
            if (Random.value < Constants.RARE_EVENT_CHANCE)
            {
                var rareEvents = availableEvents.Where(e => e.isRare).ToList();
                if (rareEvents.Count > 0)
                {
                    return rareEvents[Random.Range(0, rareEvents.Count)];
                }
            }

            // Önceliğe göre sırala
            availableEvents = availableEvents.OrderByDescending(e => e.priority).ToList();

            // Ağırlıklı seçim
            float totalWeight = availableEvents.Sum(e => e.weight);
            float randomValue = Random.Range(0f, totalWeight);
            float currentWeight = 0f;

            foreach (var evt in availableEvents)
            {
                currentWeight += evt.weight;
                if (randomValue <= currentWeight)
                    return evt;
            }

            return availableEvents[0];
        }

        /// <summary>
        /// Zincir eventini bul
        /// </summary>
        public GameEvent GetChainEvent(string previousEventId)
        {
            return _events.FirstOrDefault(e => e.previousEventId == previousEventId);
        }
        #endregion

        #region Character Queries
        /// <summary>
        /// ID ile karakter bul
        /// </summary>
        public Character GetCharacterById(string id)
        {
            return _characters.FirstOrDefault(c => c.id == id);
        }

        /// <summary>
        /// Döneme göre karakterleri filtrele
        /// </summary>
        public List<Character> GetCharactersByEra(Era era)
        {
            return _characters.Where(c => c.IsActiveInEra(era)).ToList();
        }
        #endregion

        #region Editor Methods
#if UNITY_EDITOR
        /// <summary>
        /// Event ekle
        /// </summary>
        public void AddEvent(GameEvent evt)
        {
            _events.Add(evt);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Karakter ekle
        /// </summary>
        public void AddCharacter(Character character)
        {
            _characters.Add(character);
            UnityEditor.EditorUtility.SetDirty(this);
        }

        /// <summary>
        /// Veritabanını doğrula
        /// </summary>
        [ContextMenu("Validate Database")]
        public void ValidateDatabase()
        {
            int errors = 0;

            // Duplicate ID kontrolü
            var duplicateEventIds = _events.GroupBy(e => e.id)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key);

            foreach (var id in duplicateEventIds)
            {
                Debug.LogError($"[EventDatabase] Duplicate event ID: {id}");
                errors++;
            }

            // Boş text kontrolü
            foreach (var evt in _events)
            {
                if (string.IsNullOrEmpty(evt.text))
                {
                    Debug.LogWarning($"[EventDatabase] Event {evt.id} has empty text");
                    errors++;
                }

                if (string.IsNullOrEmpty(evt.leftChoice?.text))
                {
                    Debug.LogWarning($"[EventDatabase] Event {evt.id} has empty left choice");
                    errors++;
                }

                if (string.IsNullOrEmpty(evt.rightChoice?.text))
                {
                    Debug.LogWarning($"[EventDatabase] Event {evt.id} has empty right choice");
                    errors++;
                }
            }

            // Karakter referans kontrolü
            foreach (var evt in _events)
            {
                if (evt.character != null && !_characters.Contains(evt.character))
                {
                    Debug.LogWarning($"[EventDatabase] Event {evt.id} references unknown character");
                    errors++;
                }
            }

            Debug.Log($"[EventDatabase] Validation complete. {errors} issues found.");
        }
#endif
        #endregion
    }
}
