using UnityEngine;

namespace Managers
{
    public class Player : MonoBehaviour
    {
        private void Start()
        {
            var gameObj = GameObject.Find("@Managers");
            var manager = gameObj.GetComponent<Managers>();
            Debug.Log(manager.transform.position);
        }
    }
}