using UnityEngine;

public class EnvironmentInteraction : MonoBehaviour
{
    [Header("Memory Reveal")]
    public GameObject swissPrefab;       // Assign your Swiss memory prefab
    public AudioSource audioSource;      // Optional audio when bottle is tapped
    public GameObject annotationTitle;   // The "annotation/title" text to hide on tap

    private float initialTouchDistance;
    private Vector3 initialScale;

    private void Start()
    {
        // When BottleD appears (e.g., on image scan), show the title
        if (annotationTitle != null)
        {
            annotationTitle.SetActive(true);
        }
    }

    private void Update()
    {
        HandlePinchToZoom();
    }

    // Called when the bottle is tapped
    public void selected()
    {
        Debug.Log("Bottle selected");

        if (swissPrefab != null)
            swissPrefab.SetActive(true);

        if (audioSource != null)
            audioSource.Play();

        if (annotationTitle != null)
            annotationTitle.SetActive(false);  // Hide the title when tapped
    }

    // Zoom using two-finger pinch
    void HandlePinchToZoom()
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
    }
}
