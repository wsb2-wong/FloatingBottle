using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation; // include xr library

public class ImageTracker : MonoBehaviour
{
    [SerializeField]
    ARTrackedImageManager m_TrackedImageManager; 
    public GameObject Bottle; // Prefab you want to appear on marker image

    void OnEnable() => m_TrackedImageManager.trackedImagesChanged += OnChanged;

    void OnDisable() => m_TrackedImageManager.trackedImagesChanged -= OnChanged;

    void OnChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        AudioSource source = GetComponent<AudioSource>();

        foreach (ARTrackedImage newImage in eventArgs.added)
        {
            Debug.Log("Found image");
            GameObject newObject = GameObject.Instantiate(Bottle);
            newObject.transform.SetParent(newImage.transform, false);

            // Make bottle smaller
            newObject.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f); // Adjust scale as needed

            // Rotate bottle horizontally (lie it down)
            newObject.transform.localRotation = Quaternion.Euler(90f, 0f, 0f);

            // Move bottle forward a bit on the Z-axis but keep it centered (no offset on X)
            newObject.transform.localPosition = new Vector3(0f, 0f, 0.2f); // Adjust Z (0.2f) to how far "in front" you want it
            
            source.Play();
        }
    }
}
