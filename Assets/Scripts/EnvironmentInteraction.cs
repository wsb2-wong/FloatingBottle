using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnvironmentInteraction : MonoBehaviour
{
    [Header("Memory Reveal")]
    public GameObject swissPrefab;
    public AudioSource audioSource;
    public GameObject annotationTitle;

    [Header("Canvas 1 UI")]
    public GameObject explainText;
    public Button sendToPaperPlaneButton;

    [Header("Canvas 2 UI")]
    public GameObject canvas2;
    public Button revealFishButton;
    public GameObject paperPlaneAsset;

    [Header("Final Reveal")]
    public GameObject finalPoemText;
    public GameObject fishAsset;

    [Header("Zoom Settings")]
    public float minScale = 1f;
    public float fadeDuration = 1f;

    private float initialTouchDistance;
    private Vector3 initialScale;
    private CanvasGroup annotationCanvasGroup;

    void Start()
    {
        // Setup canvas group for annotation fade
        if (annotationTitle != null)
        {
            annotationCanvasGroup = annotationTitle.GetComponent<CanvasGroup>() ?? annotationTitle.AddComponent<CanvasGroup>();
            annotationCanvasGroup.alpha = 1f;
            annotationTitle.SetActive(true);
        }

        // Initial UI visibility
        explainText?.SetActive(false);
        canvas2?.SetActive(false);
        paperPlaneAsset?.SetActive(false);
        fishAsset?.SetActive(false);
        finalPoemText?.SetActive(false);

        // Button listeners
        if (sendToPaperPlaneButton != null)
            sendToPaperPlaneButton.onClick.AddListener(OnSendToPaperPlanePressed);

        if (revealFishButton != null)
            revealFishButton.onClick.AddListener(OnRevealFishPressed);
    }

    void Update()
    {
        HandlePinchToZoom();
    }

    public void selected()
    {
        Debug.Log("Bottle selected");

        swissPrefab?.SetActive(true);
        audioSource?.Play();

        if (annotationCanvasGroup != null)
            StartCoroutine(FadeOutAnnotation());

        StartCoroutine(ShowCanvas1AfterDelay(1f));
    }

    IEnumerator FadeOutAnnotation()
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
        annotationTitle?.SetActive(false);
    }

    IEnumerator ShowCanvas1AfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        explainText?.SetActive(true);
        sendToPaperPlaneButton?.gameObject.SetActive(true);
    }

    void OnSendToPaperPlanePressed()
    {
        // Hide canvas 1
        sendToPaperPlaneButton?.transform.parent?.gameObject.SetActive(false);
        explainText?.SetActive(false);

        // Show canvas 2 with plane
        canvas2?.SetActive(true);
        paperPlaneAsset?.SetActive(true);
    }

    void OnRevealFishPressed()
    {
        // Hide canvas 2
        canvas2?.SetActive(false);

        // Show final poem with fish
        fishAsset?.SetActive(true);
        finalPoemText?.SetActive(true);
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
                float clampedFactor = Mathf.Max(scaleFactor, minScale / initialScale.x);
                transform.localScale = initialScale * clampedFactor;
            }
        }
    }
}
