using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace TurnBasedGame.Scripts.UI.StatsDescription
{
    public class StatsPanel : MonoBehaviour
    {
        [Header("Stats")]
        [Space]
        
        [SerializeField] private TMP_Text _attackText;

        [SerializeField] private TMP_Text _defenseText;

        [SerializeField] private TMP_Text _damageText;

        [SerializeField] private TMP_Text _moveRangeText;

        [SerializeField] private TMP_Text _initiativeText;

        [SerializeField] private TMP_Text _attackRangeText;

        [SerializeField] private Image _unitPortrait;

        [SerializeField] private TMP_Text _unitName;
        
        [field: Space]
        [field: Header("Scrollbar")]
        [field: Space]
        
        [field: SerializeField] public TMP_Text MaxUnits { get; set; }

        [field: SerializeField] public TMP_Text MinUnits { get; set; }

        [field: SerializeField] public TMP_Text CurrentUnits { get; set; }

        [field: SerializeField] public Scrollbar Scrollbar { get; set; }

        [field: SerializeField] public int CurrentUnitsToBuy { get; set; }
        
        [field: SerializeField] public int MaxUnitsToBuy { get; set; }
        
        public Unit CurrentUnit { get; set; }

        private bool IsOpen { get; set; }

        public void OpenPanel(Unit unit)
        {
            if (!IsOpen)
            {
                IsOpen = true;
                gameObject.SetActive(true);
            }
            
            SetStats(unit);
        }

        private void Update()
        {
            CurrentUnitsToBuy = Mathf.FloorToInt(Scrollbar.value * MaxUnitsToBuy);
            CurrentUnits.text = CurrentUnitsToBuy.ToString();
        }

        private void SetStats(Unit unit)
        {
            CurrentUnit = unit;
            
            _unitPortrait.sprite = unit.UnitPortrait;

            _unitName.text = unit.UnitName;
            
            _attackText.text = $"Атака: {unit.Attack}";
            
            _defenseText.text = $"Захист: {unit.Defense}";
            
            _damageText.text = $"Шкода: {unit.MinDamage}-{unit.MaxDamage}";
            
            _moveRangeText.text = $"Дальність руху: {unit.MoveRange}";
            
            _initiativeText.text = $"Ініціатива: {unit.Initiative}";
            
            _attackRangeText.text = $"Дальність атаки: {unit.AttackRange}";
        }
    }
}