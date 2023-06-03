using System;
using System.Collections.Generic;
using TurnBasedGame.Scripts.GameConfiguration;
using UnityEngine;


namespace TurnBasedGame.Scripts.UI.Controller
{
    public class DamageLogsController : SingletonBase<DamageLogsController>
    {
        private List<DamageLog> AllLogs = new List<DamageLog>();

        [SerializeField] private DamageLog _damageLogPrefab;

        [field: SerializeField] public GameObject LoggerPanel { get; set; }

        public void LogDamageEvent(Unit firstUnit, Unit secondUnit)
        {
            var transform1 = transform;
            var statDescription = Instantiate(_damageLogPrefab, transform1.position, Quaternion.identity, transform1);

            statDescription.LogText.text = $"{firstUnit.UnitName} deal {firstUnit.MaxDamage * firstUnit.UnitNumber} damage to {secondUnit.UnitName}";
            AllLogs.Add(statDescription);
        }


        public void LogDeathEvent(Unit deadUnit)
        {
            var transform1 = transform;
            var statDescription = Instantiate(_damageLogPrefab, transform1.position, Quaternion.identity, transform1);

            statDescription.LogText.text = $"{deadUnit.UnitName} died";
            AllLogs.Add(statDescription);
        }
    }
}