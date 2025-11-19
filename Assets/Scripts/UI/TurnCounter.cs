using UnityEngine;
using UnityEngine.UI;
using DecisionKingdom.Managers;

namespace DecisionKingdom.UI
{
    /// <summary>
    /// Tur sayacı UI komponenti
    /// </summary>
    public class TurnCounter : MonoBehaviour
    {
        [Header("UI Referansları")]
        [SerializeField] private Text _turnText;
        [SerializeField] private Text _eraText;

        [Header("Format")]
        [SerializeField] private string _turnFormat = "Tur: {0}";
        [SerializeField] private string[] _eraNames = {
            "Ortaçağ",
            "Rönesans",
            "Sanayi Devrimi",
            "Modern Dönem",
            "Gelecek"
        };

        #region Unity Lifecycle
        private void Start()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnTurnAdvanced += HandleTurnAdvanced;
                GameManager.Instance.OnEraChanged += HandleEraChanged;
                GameManager.Instance.OnNewGameStarted += HandleNewGameStarted;

                // Başlangıç değerlerini göster
                UpdateTurnText(GameManager.Instance.CurrentTurn);
                UpdateEraText(GameManager.Instance.CurrentEra);
            }
        }

        private void OnDestroy()
        {
            if (GameManager.Instance != null)
            {
                GameManager.Instance.OnTurnAdvanced -= HandleTurnAdvanced;
                GameManager.Instance.OnEraChanged -= HandleEraChanged;
                GameManager.Instance.OnNewGameStarted -= HandleNewGameStarted;
            }
        }
        #endregion

        #region Event Handlers
        private void HandleTurnAdvanced(int turn)
        {
            UpdateTurnText(turn);
        }

        private void HandleEraChanged(Core.Era era)
        {
            UpdateEraText(era);
        }

        private void HandleNewGameStarted()
        {
            if (GameManager.Instance != null)
            {
                UpdateTurnText(GameManager.Instance.CurrentTurn);
                UpdateEraText(GameManager.Instance.CurrentEra);
            }
        }
        #endregion

        #region Private Methods
        private void UpdateTurnText(int turn)
        {
            if (_turnText != null)
            {
                _turnText.text = string.Format(_turnFormat, turn);
            }
        }

        private void UpdateEraText(Core.Era era)
        {
            if (_eraText != null)
            {
                int index = (int)era;
                if (index >= 0 && index < _eraNames.Length)
                {
                    _eraText.text = _eraNames[index];
                }
            }
        }
        #endregion
    }
}
