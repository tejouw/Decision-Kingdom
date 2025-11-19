using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Data
{
    /// <summary>
    /// Oyun karakteri veri yapısı
    /// </summary>
    [System.Serializable]
    public class Character
    {
        [Tooltip("Benzersiz karakter ID'si")]
        public string id;

        [Tooltip("Karakter adı")]
        public string name;

        [Tooltip("Karakter unvanı")]
        public string title;

        [Tooltip("Karakter portresi")]
        public Sprite portrait;

        [Tooltip("Karakterin aktif olduğu dönemler")]
        public List<Era> eras;

        [Tooltip("Karakter açıklaması")]
        [TextArea(2, 4)]
        public string description;

        public Character()
        {
            id = System.Guid.NewGuid().ToString();
            name = "";
            title = "";
            portrait = null;
            eras = new List<Era>();
            description = "";
        }

        public Character(string id, string name, string title)
        {
            this.id = id;
            this.name = name;
            this.title = title;
            portrait = null;
            eras = new List<Era>();
            description = "";
        }

        /// <summary>
        /// Tam isim (unvan + ad)
        /// </summary>
        public string FullName => string.IsNullOrEmpty(title) ? name : $"{title} {name}";

        /// <summary>
        /// Karakterin belirli bir dönemde aktif olup olmadığını kontrol et
        /// </summary>
        public bool IsActiveInEra(Era era)
        {
            return eras == null || eras.Count == 0 || eras.Contains(era);
        }
    }

    /// <summary>
    /// Karakter durumu (oyun içi)
    /// </summary>
    [System.Serializable]
    public class CharacterState
    {
        [Tooltip("Karakter ID'si")]
        public string characterId;

        [Tooltip("Etkileşim sayısı")]
        public int interactionCount;

        [Tooltip("İlişki durumu (-100 ile 100 arası)")]
        [Range(-100, 100)]
        public int relationship;

        [Tooltip("Karakter flag'leri")]
        public List<string> flags;

        [Tooltip("Son etkileşim turu")]
        public int lastInteractionTurn;

        public CharacterState()
        {
            characterId = "";
            interactionCount = 0;
            relationship = 0;
            flags = new List<string>();
            lastInteractionTurn = 0;
        }

        public CharacterState(string characterId)
        {
            this.characterId = characterId;
            interactionCount = 0;
            relationship = 0;
            flags = new List<string>();
            lastInteractionTurn = 0;
        }

        /// <summary>
        /// İlişkiyi değiştir
        /// </summary>
        public void ModifyRelationship(int amount)
        {
            relationship = Mathf.Clamp(relationship + amount, -100, 100);
        }

        /// <summary>
        /// Etkileşimi kaydet
        /// </summary>
        public void RecordInteraction(int currentTurn)
        {
            interactionCount++;
            lastInteractionTurn = currentTurn;
        }

        /// <summary>
        /// Flag ekle
        /// </summary>
        public void AddFlag(string flag)
        {
            if (!flags.Contains(flag))
            {
                flags.Add(flag);
            }
        }

        /// <summary>
        /// Flag'i kontrol et
        /// </summary>
        public bool HasFlag(string flag)
        {
            return flags.Contains(flag);
        }

        /// <summary>
        /// Flag'i kaldır
        /// </summary>
        public void RemoveFlag(string flag)
        {
            flags.Remove(flag);
        }

        /// <summary>
        /// İlişki durumu metni
        /// </summary>
        public string GetRelationshipStatus()
        {
            if (relationship >= 75) return "Çok Sadık";
            if (relationship >= 50) return "Sadık";
            if (relationship >= 25) return "Olumlu";
            if (relationship >= -25) return "Nötr";
            if (relationship >= -50) return "Olumsuz";
            if (relationship >= -75) return "Düşman";
            return "Kan Düşmanı";
        }
    }
}
