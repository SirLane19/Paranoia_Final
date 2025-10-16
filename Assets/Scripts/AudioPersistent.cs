using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Supaya audio tidak di destroy
public class AudioPersistent : MonoBehaviour
{
    public GameObject BGM;
    // Start is called before the first frame update
    void Start()
    {
        if (BGM != null)
        {
            AudioSource audio = BGM.GetComponent<AudioSource>();

            if (audio != null && audio.isPlaying)
                audio.Play();
            else
                Debug.LogWarning("BGM Belum diset.");
        } else
                Debug.LogWarning("Lah.");
    }
}
