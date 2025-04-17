using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class EnvironmentInteraction : MonoBehaviour
{
    [Header("Memory Reveal")]
    public GameObject swissPrefab;
    public AudioSource bottleClickSound;
    public AudioSource ambientSound;
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

    [Header("Memory UI")]
    public GameObject canvasMemory;
    public Button revealMemoryButton;
    public GameObject memoryAsset;
    public GameObject bottleD;

    [Header("Star Reveal")]
    public GameObject canvasStar;
    public Button revealStarButton;
    public GameObject swissStar;
    public AudioSource memorySave;
    public ParticleSystem starParticles;

    [Header("Sound Effects")]
    public AudioSource buttonTapSound;

    [Header("Zoom Settings")]
    public float minScale = 1f;
    public float fadeDuration = 1f;
    public float ambientFadeInDuration = 2f;

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
        canvasMemory?.SetActive(false);
        memoryAsset?.SetActive(false);
        canvasStar?.SetActive(false);
        swissStar?.SetActive(false);
        starParticles?.gameObject.SetActive(false);

        // Ambient audio setup
        if (ambientSound != null)
        {
            ambientSound.loop = true;
            ambientSound.volume = 0f;
        }

        // Button listeners
        sendToPaperPlaneButton?.onClick.AddListener(OnSendToPaperPlanePressed);
        revealFishButton?.onClick.AddListener(OnRevealFishPressed);
        revealMemoryButton?.onClick.AddListener(OnRevealMemoryPressed);
        revealStarButton?.onClick.AddListener(OnRevealStarPressed);
    }

    void Update()
    {
        HandlePinchToZoom();
    }

    public void selected()
    {
        Debug.Log("Bottle selected");

        swissPrefab?.SetActive(true);
        bottleClickSound?.Play();
        if (ambientSound != null) StartCoroutine(FadeInAmbient());

        if (annotationCanvasGroup != null)
            StartCoroutine(FadeOutAnnotation());

        StartCoroutine(ShowCanvas1AfterDelay(1f));
    }

    IEnumerator FadeInAmbient()
    {
        ambientSound.Play();
        float elapsed = 0f;

        while (elapsed < ambientFadeInDuration)
        {
            elapsed += Time.deltaTime;
            ambientSound.volume = Mathf.Lerp(0f, 1f, elapsed / ambientFadeInDuration);
            yield return null;
        }

        ambientSound.volume = 1f;
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
        buttonTapSound?.Play();

        sendToPaperPlaneButton?.transform.parent?.gameObject.SetActive(false);
        explainText?.SetActive(false);

        canvas2?.SetActive(true);
        paperPlaneAsset?.SetActive(true);
    }

    void OnRevealFishPressed()
    {
        buttonTapSound?.Play();

        canvas2?.SetActive(false);
        fishAsset?.SetActive(true);
        finalPoemText?.SetActive(true);

        canvasMemory?.SetActive(true);
    }

    void OnRevealMemoryPressed()
    {
        buttonTapSound?.Play();

        // Hide all elements except the bottle
        swissPrefab?.SetActive(false);
        annotationTitle?.SetActive(false);
        paperPlaneAsset?.SetActive(false);
        fishAsset?.SetActive(false);
        finalPoemText?.SetActive(false);
        canvasMemory?.SetActive(false);

        memoryAsset?.SetActive(true);
        bottleD?.SetActive(true);

        // Show canvasStar with button
        canvasStar?.SetActive(true);
        revealStarButton?.gameObject.SetActive(true);
    }

    void OnRevealStarPressed()
    {
        buttonTapSound?.Play();

        swissStar?.SetActive(true);
        if (memorySave != null) memorySave.Play();
        if (starParticles != null)
        {
            starParticles.gameObject.SetActive(true);
            starParticles.Play();
        }
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
