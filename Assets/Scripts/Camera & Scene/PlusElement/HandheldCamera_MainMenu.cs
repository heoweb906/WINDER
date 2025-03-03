using UnityEngine;

public class HandheldCamera_MainMenu : MonoBehaviour
{
    public float rotationAmount; // Amount of rotation
    public float rotationSpeed; // Rotation speed

    public Vector3 originalRotation;

    private void Start()
    {
        originalRotation = transform.localEulerAngles;
    }

    void FixedUpdate()
    {
        float xRotation = Mathf.PerlinNoise(Time.time * rotationSpeed, 0) * rotationAmount;
        float yRotation = Mathf.PerlinNoise(0, Time.time * rotationSpeed) * rotationAmount;

        transform.localEulerAngles = new Vector3(
            originalRotation.x + xRotation,
            originalRotation.y + yRotation,
            originalRotation.z
        );
    }
}
