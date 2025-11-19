using System.Collections.Generic;
using UnityEngine;
using DecisionKingdom.Core;

namespace DecisionKingdom.Data
{
    /// <summary>
    /// Oyun event/kart veri yapısı
    /// </summary>
    [System.Serializable]
    public class GameEvent
    {
        [Tooltip("Benzersiz event ID'si")]
        public string id;

        [Tooltip("Event'in ait olduğu dönem")]
        public Era era;

        [Tooltip("Event kategorisi")]
        public EventCategory category;

        [Tooltip("Event ile ilişkili karakter")]
        public Character character;

        [Tooltip("Event metni")]
        [TextArea(3, 6)]
        public string text;

        [Tooltip("Sol seçenek (Hayır/Reddet)")]
        public Choice leftChoice;

        [Tooltip("Sağ seçenek (Evet/Kabul)")]
        public Choice rightChoice;

        [Tooltip("Event'in görünmesi için gerekli koşullar")]
        public List<Condition> conditions;

        [Tooltip("Event önceliği (yüksek = önce)")]
        public int priority;

        [Tooltip("Event ağırlığı (seçim olasılığı)")]
        [Range(0f, 100f)]
        public float weight = 1f;

        [Tooltip("Nadir event mi?")]
        public bool isRare;

        [Tooltip("Zincir event - bir önceki event ID'si")]
        public string previousEventId;

        [Tooltip("Event açıklaması (debug için)")]
        [TextArea(1, 2)]
        public string description;

        public GameEvent()
        {
            id = System.Guid.NewGuid().ToString();
            era = Era.Medieval;
            category = EventCategory.Random;
            character = null;
            text = "";
            leftChoice = new Choice();
            rightChoice = new Choice();
            conditions = new List<Condition>();
            priority = 0;
            weight = 1f;
            isRare = false;
            previousEventId = "";
            description = "";
        }

        public GameEvent(string id, Era era, string text)
        {
            this.id = id;
            this.era = era;
            category = EventCategory.Random;
            character = null;
            this.text = text;
            leftChoice = new Choice();
            rightChoice = new Choice();
            conditions = new List<Condition>();
            priority = 0;
            weight = 1f;
            isRare = false;
            previousEventId = "";
            description = "";
        }

        /// <summary>
        /// Event'in koşullarını kontrol et
        /// </summary>
        public bool CheckConditions(GameStateData gameState)
        {
            if (conditions == null || conditions.Count == 0)
                return true;

            foreach (var condition in conditions)
            {
                if (!condition.Evaluate(gameState))
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Karakter ID'si
        /// </summary>
        public string CharacterId => character?.id ?? "";

        /// <summary>
        /// Karakter adı
        /// </summary>
        public string CharacterName => character?.FullName ?? "Bilinmeyen";
    }

    /// <summary>
    /// Event koşulu
    /// </summary>
    [System.Serializable]
    public class Condition
    {
        [Tooltip("Koşul türü")]
        public ConditionType type;

        [Tooltip("Kaynak türü (ResourceThreshold için)")]
        public ResourceType resource;

        [Tooltip("Operatör")]
        public ConditionOperator conditionOperator;

        [Tooltip("Karşılaştırma değeri")]
        public int value;

        [Tooltip("Karakter ID (CharacterInteraction için)")]
        public string characterId;

        [Tooltip("Flag adı (Flag için)")]
        public string flag;

        [Tooltip("Dönem (Era için)")]
        public Era era;

        public Condition()
        {
            type = ConditionType.ResourceThreshold;
            resource = ResourceType.Gold;
            conditionOperator = ConditionOperator.GreaterThan;
            value = 0;
            characterId = "";
            flag = "";
            era = Era.Medieval;
        }

        /// <summary>
        /// Koşulu değerlendir
        /// </summary>
        public bool Evaluate(GameStateData gameState)
        {
            switch (type)
            {
                case ConditionType.ResourceThreshold:
                    return EvaluateResourceThreshold(gameState);

                case ConditionType.CharacterInteraction:
                    return EvaluateCharacterInteraction(gameState);

                case ConditionType.TurnCount:
                    return Compare(gameState.turn, value);

                case ConditionType.Flag:
                    return EvaluateFlag(gameState);

                case ConditionType.Era:
                    return gameState.era == era;

                default:
                    return true;
            }
        }

        private bool EvaluateResourceThreshold(GameStateData gameState)
        {
            int resourceValue = gameState.resources.GetResource(resource);
            return Compare(resourceValue, value);
        }

        private bool EvaluateCharacterInteraction(GameStateData gameState)
        {
            if (gameState.characterStates.TryGetValue(characterId, out CharacterState state))
            {
                return Compare(state.interactionCount, value);
            }
            return Compare(0, value);
        }

        private bool EvaluateFlag(GameStateData gameState)
        {
            bool hasFlag = gameState.flags.Contains(flag);
            return conditionOperator == ConditionOperator.Equal ? hasFlag : !hasFlag;
        }

        private bool Compare(int a, int b)
        {
            return conditionOperator switch
            {
                ConditionOperator.Equal => a == b,
                ConditionOperator.NotEqual => a != b,
                ConditionOperator.LessThan => a < b,
                ConditionOperator.LessThanOrEqual => a <= b,
                ConditionOperator.GreaterThan => a > b,
                ConditionOperator.GreaterThanOrEqual => a >= b,
                _ => false
            };
        }

        public override string ToString()
        {
            return type switch
            {
                ConditionType.ResourceThreshold => $"{resource} {conditionOperator} {value}",
                ConditionType.CharacterInteraction => $"Character {characterId} interaction {conditionOperator} {value}",
                ConditionType.TurnCount => $"Turn {conditionOperator} {value}",
                ConditionType.Flag => $"Flag '{flag}' {conditionOperator}",
                ConditionType.Era => $"Era == {era}",
                _ => "Unknown condition"
            };
        }
    }
}
