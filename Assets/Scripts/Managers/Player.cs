using UnityEngine;

namespace Managers
{
    public class Player : MonoBehaviour
    {
        private void Start()
        {
            var managers = Managers.Instance;
            Debug.Log(managers.gameObject.transform.position);
        }
    }
}