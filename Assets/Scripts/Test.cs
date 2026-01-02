using UnityEngine;

public class Test : MonoBehaviour
{
    private void Start()
    {
        Debug.Log("Test script started");
    }

    private void Update()
    {
        transform.Rotate(new Vector3(1f, 1f, 1f) * (100f * Time.deltaTime));
    }
}