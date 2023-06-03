using TMPro;
using TurnBasedGame.Scripts.UI.StatsDescription;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI
{
    public class UnitsCountView : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField] public Image _backGroundImage;
        
        private TMP_Text _countText;
        
        private Stats _unitStats;
        
        private int Value { get; set; }

        private void OnEnable()
        {
            _unitStats = FindObjectOfType<Stats>();
            _countText = GetComponentInChildren<TMP_Text>();
        }

        public void UpdateCountValue(int value)
        {
            Value += value;
            if (Value <= 0)
            {
                gameObject.SetActive(false);
            }
            _countText.text = Value.ToString();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            Debug.Log("Enter");
            _unitStats.GetComponent<Image>().enabled = true;
            var unit = GetComponentInParent<Unit>();
            _unitStats.ShowStats(unit);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            Debug.Log("Exit");
            _unitStats.GetComponent<Image>().enabled = false;
            _unitStats.ClearStats();
        }
    }
}