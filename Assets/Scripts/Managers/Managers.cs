using UnityEngine;

namespace Managers
{
    public class Managers : MonoBehaviour
    {
        private static Managers _instance;
        public static Managers Instance => _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
    }
}