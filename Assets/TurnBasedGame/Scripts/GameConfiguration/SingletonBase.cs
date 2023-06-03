using UnityEngine;

namespace TurnBasedGame.Scripts.GameConfiguration
{
    public class SingletonBase<T> : MonoBehaviour where T : MonoBehaviour
    {
        private static T _instance;

        public static T Instance
        {
            get
            {
                if (_instance == null)
                {
                    _instance = FindObjectOfType<T>();

                    if (_instance == null)
                    {
                        Debug.LogError($"Singleton {typeof(T)} was not instance!");

                    }
                }

                return _instance;
            }
        }

        private void OnEnable()
        {
            T[] currentObjects = FindObjectsOfType<T>();

            if (currentObjects.Length > 1)
            {
                Debug.LogWarning($"Singleton {typeof(T)} have copy!");

                Destroy(gameObject);
                return;
            }
        }
    }
}