using System.Collections.Generic;
using UnityEngine;

namespace DecisionKingdom.Data
{
    /// <summary>
    /// Kart seçeneği veri yapısı
    /// </summary>
    [System.Serializable]
    public class Choice
    {
        [Tooltip("Seçenek metni")]
        public string text;

        [Tooltip("Kaynak etkileri")]
        public List<ResourceEffect> effects;

        [Tooltip("Tetiklenen event ID'leri")]
        public List<string> triggeredEventIds;

        [Tooltip("Ayarlanan flag'ler")]
        public List<string> flags;

        [Tooltip("Karakter ilişki değişimi")]
        public int relationshipChange;

        public Choice()
        {
            text = "";
            effects = new List<ResourceEffect>();
            triggeredEventIds = new List<string>();
            flags = new List<string>();
            relationshipChange = 0;
        }

        public Choice(string text)
        {
            this.text = text;
            effects = new List<ResourceEffect>();
            triggeredEventIds = new List<string>();
            flags = new List<string>();
            relationshipChange = 0;
        }

        /// <summary>
        /// Kaynak efekti ekle
        /// </summary>
        public Choice AddEffect(Core.ResourceType resource, int min, int max)
        {
            effects.Add(new ResourceEffect(resource, min, max));
            return this;
        }

        /// <summary>
        /// Sabit kaynak efekti ekle
        /// </summary>
        public Choice AddEffect(Core.ResourceType resource, int amount)
        {
            effects.Add(new ResourceEffect(resource, amount));
            return this;
        }

        /// <summary>
        /// Tetiklenen event ekle
        /// </summary>
        public Choice AddTriggeredEvent(string eventId)
        {
            triggeredEventIds.Add(eventId);
            return this;
        }

        /// <summary>
        /// Flag ekle
        /// </summary>
        public Choice AddFlag(string flag)
        {
            flags.Add(flag);
            return this;
        }

        /// <summary>
        /// Efektlerin özet metni
        /// </summary>
        public string GetEffectsSummary()
        {
            if (effects == null || effects.Count == 0)
                return "Etkisi yok";

            var summary = new System.Text.StringBuilder();
            foreach (var effect in effects)
            {
                string sign = effect.min >= 0 ? "+" : "";
                string value = effect.min == effect.max
                    ? $"{sign}{effect.min}"
                    : $"{sign}{effect.min} ile {effect.max} arası";

                summary.AppendLine($"{effect.resource}: {value}");
            }

            return summary.ToString().TrimEnd();
        }

        /// <summary>
        /// Seçeneği uygula
        /// </summary>
        public void Apply(Resources resources)
        {
            if (effects == null) return;
            resources.ApplyEffects(effects.ToArray());
        }
    }
}
