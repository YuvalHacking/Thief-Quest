using UnityEngine;
using Cinemachine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private GameObject camera;
    [SerializeField] private float parallaxEffect;

    private float length;
    private float startPosition;

    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void Update()
    {

        float dist = (camera.transform.position.x * parallaxEffect);

        // float temp = (camera.transform.position.x * (1 - parallaxEffect));

        transform.position = new Vector2(startPosition + dist, transform.position.y);

        // if (temp > startPosition + length) startPosition += length;
        // else if (temp < startPosition - length) startPosition -= length;

    }
}
