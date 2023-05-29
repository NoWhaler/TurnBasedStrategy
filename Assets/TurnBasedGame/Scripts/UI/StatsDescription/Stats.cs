using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace TurnBasedGame.Scripts.UI.StatsDescription
{
    public class Stats : MonoBehaviour
    {
        [field: SerializeField] private List<TMP_Text> AllDescriptions { get; } = new ();

        [SerializeField] private Description _description;

        public void ShowStats(Unit unit)
        {
            if (unit.CurrentHealthPoints != 0 && unit.MaxHealthPoints != 0)
            {
                var transform1 = transform;
                var statDescription = Instantiate(_description, transform1.position, Quaternion.identity, transform1);

                statDescription.DescriptionText.text = $"Health: {unit.CurrentHealthPoints} / {unit.MaxHealthPoints}";
                AllDescriptions.Add(statDescription.DescriptionText);
            }
            
            if (unit.Attack != 0)
            {
                var transform1 = transform;
                var statDescription = Instantiate(_description, transform1.position, Quaternion.identity, transform1);
               
                statDescription.DescriptionText.text = $"Attack: {unit.Attack}";
                AllDescriptions.Add(statDescription.DescriptionText);
            }
            
            if (unit.Defense != 0)
            {
                var transform1 = transform;
                var statDescription = Instantiate(_description, transform1.position, Quaternion.identity, transform1);

                statDescription.DescriptionText.text = $"Defense: {unit.Defense}";
                AllDescriptions.Add(statDescription.DescriptionText);
            }
            
            if (unit.MinDamage != 0 && unit.MaxDamage != 0)
            {
                var transform1 = transform;
                var statDescription = Instantiate(_description, transform1.position, Quaternion.identity, transform1);
                
                statDescription.DescriptionText.text = $"Damage: {unit.MinDamage} / {unit.MaxDamage}";
                AllDescriptions.Add(statDescription.DescriptionText);
            }
            
            if (unit.AttackRange != 0)
            {
                var transform1 = transform;
                var statDescription = Instantiate(_description, transform1.position, Quaternion.identity, transform1);
                
                statDescription.DescriptionText.text = $"Attack Range: {unit.AttackRange}";
                AllDescriptions.Add(statDescription.DescriptionText);
            }
            
            if (unit.MoveRange != 0)
            {
                var transform1 = transform;
                var statDescription = Instantiate(_description, transform1.position, Quaternion.identity, transform1);
               
                statDescription.DescriptionText.text = $"Move range: {unit.MoveRange}";
                AllDescriptions.Add(statDescription.DescriptionText);
            }
            
            if (unit.Initiative != 0)
            {
                var transform1 = transform;
                var statDescription = Instantiate(_description, transform1.position, Quaternion.identity, transform1);
                
                statDescription.DescriptionText.text = $"Initiative: {unit.Initiative}";
                AllDescriptions.Add(statDescription.DescriptionText);
            }
        }

        public void ClearStats()
        {
            foreach (var description in AllDescriptions)
            {
                Destroy(description.gameObject);
            }
            
            AllDescriptions.Clear();
        }
    }
}