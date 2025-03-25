using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;

public class ImageTracker : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager;

    public GameObject Bottle; // Prefab to appear on marker image


    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        AudioSource source = GetComponent<AudioSource>();

        foreach (ARTrackedImage newImage in eventArgs.added)
        {
            Debug.Log("Found image");

            // Instantiate the bottle prefab
            GameObject newObject = Instantiate(Bottle);

            // Parent it to the marker so it follows the image
            newObject.transform.SetParent(newImage.transform, false);

            // Position offset - move a bit forward relative to the marker (e.g., Z = 0.3m)
            //newObject.transform.localPosition = new Vector3(0.1f, 0f, 2f);
            // Scale it so it's 0.17 meters long (assuming uniform scale, adjust as needed)
            //newObject.transform.localScale = Vector3.one * 0.17f;

            //source.Play();

            // OPTIONAL: Example of how you can also randomly spawn rock instances

        }
    }
}
