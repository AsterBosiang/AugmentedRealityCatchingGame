using UnityEngine;

public class AppleRotation : MonoBehaviour
{
    [SerializeField]
    private float rotationSpeed = 50f;

    void Update()
    {
        transform.Rotate(0f, rotationSpeed * Time.deltaTime, 0f);
    }
}
