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

        
        /// <summary>
        ///  Записує подію отримання пошкодження в журнал.
        /// Створює новий журнал за допомогою префабу _damageLogPrefab
        /// і додає його до списку AllLogs.
        /// Відображає відповідний текст з інформацією про пошкодження.
        /// </summary>
        public void LogDamageEvent(Unit firstUnit, Unit secondUnit, int damage)
        {
            var transform1 = transform;
            var statDescription = Instantiate(_damageLogPrefab, transform1.position, Quaternion.identity, transform1);

            statDescription.LogText.text = $"{firstUnit.UnitName} наніс {damage} шкоди {secondUnit.UnitName}";
            AllLogs.Add(statDescription);
        }

        /// <summary>
        /// Записує подію смерті бойової одиниці в журнал.
        /// Створює новий журнал за допомогою префабу _damageLogPrefab
        /// і додає його до списку AllLogs.
        /// Відображає відповідний текст з інформацією про смерть одиниці.
        /// </summary>
        public void LogDeathEvent(Unit deadUnit)
        {
            var transform1 = transform;
            var statDescription = Instantiate(_damageLogPrefab, transform1.position, Quaternion.identity, transform1);

            statDescription.LogText.text = $"{deadUnit.UnitName} помер";
            AllLogs.Add(statDescription);
        }
    }
}