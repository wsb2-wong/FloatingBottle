using UnityEngine;
using System.Collections;

public class EnvironmentInteraction : MonoBehaviour
{
    [Header("Memory Reveal")]
    public GameObject swissPrefab;       // Assign your Swiss memory prefab
    public AudioSource audioSource;      // Optional audio when bottle is tapped
    public GameObject annotationTitle;   // The "annotation/title" text to hide on tap
    public float fadeDuration = 1f;      // Duration of the fade in seconds

    private float initialTouchDistance;
    private Vector3 initialScale;
    private CanvasGroup annotationCanvasGroup;

    private void Start()
    {
        if (annotationTitle != null)
        {
            // Ensure there's a CanvasGroup component for fading
            annotationCanvasGroup = annotationTitle.GetComponent<CanvasGroup>();
            if (annotationCanvasGroup == null)
            {
                annotationCanvasGroup = annotationTitle.AddComponent<CanvasGroup>();
            }

            annotationCanvasGroup.alpha = 1f;
            annotationTitle.SetActive(true);
        }
    }

    private void Update()
    {
        HandlePinchToZoom();
    }

    public void selected()
    {
        Debug.Log("Bottle selected");

        if (swissPrefab != null)
            swissPrefab.SetActive(true);

        if (audioSource != null)
            audioSource.Play();

        if (annotationCanvasGroup != null)
            StartCoroutine(FadeOutAnnotation());
    }

    private IEnumerator FadeOutAnnotation()
    {
        float elapsed = 0f;
        float startAlpha = annotationCanvasGroup.alpha;

        while (elapsed < fadeDuration)
        {
            elapsed += Time.deltaTime;
            annotationCanvasGroup.alpha = Mathf.Lerp(startAlpha, 0f, elapsed / fadeDuration);
            yield return null;
        }

        annotationCanvasGroup.alpha = 0f;
        annotationTitle.SetActive(false); // Fully disable after fade-out
    }

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
