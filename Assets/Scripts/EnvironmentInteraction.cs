using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnvironmentInteraction : MonoBehaviour
{
    public GameObject swissPrefab; // Drag your Swiss object here in the inspector
    public AudioSource audioSource;

   public void selected()
   {
        Debug.Log("Bottle selected");

        if (swissPrefab != null)
        {
            swissPrefab.SetActive(true);
        }

        if (audioSource != null)
        {
            audioSource.Play();
        }
   }
}

