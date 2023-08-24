using UnityEngine;
using Cinemachine;

public class Parallax : MonoBehaviour
{
    [SerializeField] private CinemachineVirtualCamera camera;
    [SerializeField] private float parallaxEffect;

    private float length;
    private float startPosition;

    void Start()
    {
        startPosition = transform.position.x;
        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    void FixedUpdate()
    {

        float dist = (camera.transform.position.x * parallaxEffect);

        float temp = (camera.transform.position.x * (1 - parallaxEffect));

        // if (cameraSwitcher.getActiveCamera() == virtualCamera)
        // {
        transform.position = new Vector2(startPosition + dist, transform.position.y);

        if (temp > startPosition + length) startPosition += length;
        else if (temp < startPosition - length) startPosition -= length;
        // }
        // else
        // {
        //     transform.position = new Vector2(originalPos.x, originalPos.y);
        // }
    }
}
