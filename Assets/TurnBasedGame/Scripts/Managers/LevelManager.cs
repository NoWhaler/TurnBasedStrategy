using TurnBasedGame.Scripts.GameConfiguration;
using TurnBasedGame.Scripts.UI;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace TurnBasedGame.Scripts.Managers
{
    public class LevelManager : SingletonBase<LevelManager>
    {
        [field: SerializeField] public int CurrentLevel { get; set; } = 1;

        private void Awake()
        {
            DontDestroyOnLoad(this);
            CurrentLevel = PlayerPrefs.GetInt(PrefsList.CurrentLevelPref, 1);
        }

        private void Start()
        {
            LoadCurrentLevel(CurrentLevel);
        }

        public void RestartLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void LoadNextLevel()
        {
            SceneManager.LoadScene(CurrentLevel);
        }

        private void LoadCurrentLevel(int level)
        {
            SceneManager.LoadScene(level);
        }
    }
}