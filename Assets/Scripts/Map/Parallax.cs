using UnityEngine;

public class Parallax : MonoBehaviour
{
    // Reference to the main camera GameObject
    [SerializeField] private GameObject camera;

    // The parallax effect intensity
    [SerializeField] private float parallaxEffect;

    private float startPosition; // The initial position of the parallax object

    void Start()
    {
        startPosition = transform.position.x; // Store the initial position of the object
    }

    void Update()
    {
        // Calculate the horizontal distance based on the camera's position and the parallax effect
        float dist = (camera.transform.position.x * parallaxEffect);

        // Apply the parallax effect by updating the object's position
        transform.position = new Vector2(startPosition + dist, transform.position.y);
    }
}
