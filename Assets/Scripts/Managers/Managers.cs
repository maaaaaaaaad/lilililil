using UnityEngine;

namespace Managers
{
    public class Managers : MonoBehaviour
    {
        private static Managers _instance;

        public static Managers Instance
        {
            get
            {
                if (_instance != null)
                    return _instance;

                _instance = FindAnyObjectByType<Managers>();
                if (_instance != null)
                    return _instance;

                var gameObj = new GameObject("@Managers");
                _instance = gameObj.AddComponent<Managers>();
                return _instance;
            }
        }

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }
}