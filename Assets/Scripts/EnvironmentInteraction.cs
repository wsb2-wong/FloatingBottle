using UnityEngine;

public class EnvironmentInteraction : MonoBehaviour
{
    [Header("Memory Reveal")]
    public GameObject swissPrefab;       // Drag the Swiss memory here
    public AudioSource audioSource;      // Optional audio

    private float initialTouchDistance;
    private Vector3 initialScale;

    private void Update()
    {
        HandleTouchGestures();
    }

    public void selected()
    {
        Debug.Log("Bottle selected");

        if (swissPrefab != null)
            swissPrefab.SetActive(true);

        if (audioSource != null)
            audioSource.Play();
    }

    void HandleTouchGestures()
    {
        if (Input.touchCount == 2)
        {
            Touch t0 = Input.GetTouch(0);
            Touch t1 = Input.GetTouch(1);

            float currentDistance = Vector2.Distance(t0.position, t1.position);

            if (t0.phase == TouchPhase.Began || t1.phase == TouchPhase.Began)
            {
                initialTouchDistance = currentDistance;
                initialScale = transform.localScale;
            }
            else
            {
                float scaleFactor = currentDistance / initialTouchDistance;
                transform.localScale = initialScale * scaleFactor;
            }
        }

        if (Input.touchCount == 1)
        {
            Touch t = Input.GetTouch(0);

            if (t.phase == TouchPhase.Moved)
            {
                float rotateSpeed = 0.2f;
                transform.Rotate(0, -t.deltaPosition.x * rotateSpeed, 0, Space.World);
            }
        }
    }
}
