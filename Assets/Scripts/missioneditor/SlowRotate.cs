using UnityEngine;

public class SlowRotate : MonoBehaviour
{
    // Rotation speed in degrees per second
    public Vector3 rotationSpeed = new Vector3(0, 30, 0);

    void Update()
    {
        transform.Rotate(rotationSpeed * Time.unscaledDeltaTime);
    }
}
