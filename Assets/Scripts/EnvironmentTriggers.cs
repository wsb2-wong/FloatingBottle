using System.Collections;
using UnityEngine;

public class EnvironmentTriggers : MonoBehaviour
{
    public GameObject environmentAsset;
    private AudioSource audioSource;
    private bool hasActivated = false;

    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        if (environmentAsset != null)
        {
            environmentAsset.SetActive(false); // Start hidden
        }
    }

    void Update()
    {
        if (Input.touchCount > 0 && !hasActivated)
        {
            Touch touch = Input.GetTouch(0);
            if (touch.phase == TouchPhase.Began)
            {
                Ray ray = Camera.main.ScreenPointToRay(touch.position);
                RaycastHit hit;

                if (Physics.Raycast(ray, out hit))
                {
                    if (hit.transform == transform)
                    {
                        hasActivated = true;
                        StartCoroutine(ShowEnvironment());
                    }
                }
            }
        }
    }

    IEnumerator ShowEnvironment()
    {
        if (environmentAsset != null)
        {
            environmentAsset.SetActive(true);

            // Gradually fade in
            Renderer[] renderers = environmentAsset.GetComponentsInChildren<Renderer>();
            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.materials)
                {
                    Color color = mat.color;
                    color.a = 0;
                    mat.color = color;
                }
            }

            float duration = 2f;
            float elapsed = 0f;

            while (elapsed < duration)
            {
                foreach (Renderer rend in renderers)
                {
                    foreach (Material mat in rend.materials)
                    {
                        Color color = mat.color;
                        color.a = Mathf.Lerp(0, 1, elapsed / duration);
                        mat.color = color;
                    }
                }
                elapsed += Time.deltaTime;
                yield return null;
            }

            // Ensure final alpha is 1
            foreach (Renderer rend in renderers)
            {
                foreach (Material mat in rend.materials)
                {
                    Color color = mat.color;
                    color.a = 1f;
                    mat.color = color;
                }
            }

            if (audioSource != null)
                audioSource.Play();
        }
    }
}
