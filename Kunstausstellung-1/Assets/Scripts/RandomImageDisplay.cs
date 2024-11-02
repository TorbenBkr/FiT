using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomImageDisplay : MonoBehaviour
{
    public List<Texture2D> textures; // Liste von Texturen im Unity Inspector hinzufügen
    public float maxDelaySeconds = 4f; // Maximale Zufallsverzögerung in Sekunden
    public AudioClip[] moodAudioClips; // Array von Audiodateien für jedes Bild

    private AudioSource audioSource; // Player AudioSource (externe Quelle)
    private AudioSource selfAudio;   // Eigene AudioSource für das Abspielen der Clips
    private Material planeMaterial;
    private Vector3 originalScale;
    private int currentIndex = 0; // Hält den Index des aktuellen Bildes

    void Start()
    {
        // Player-GameObject mit dem Tag "Player" finden und dessen AudioSource abrufen
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            audioSource = player.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("Keine AudioSource auf dem Player-Objekt gefunden.");
                return;
            }
        }
        else
        {
            Debug.LogError("Kein GameObject mit dem Tag 'Player' gefunden.");
            return;
        }


        selfAudio = GetComponent<AudioSource>();

        planeMaterial = GetComponent<Renderer>().material;
        originalScale = transform.localScale;

        if (textures.Count == 0)
        {
            Debug.LogError("Keine Texturen in der Liste. Bitte Texturen im Inspector hinzufügen.");
            return;
        }
        if (moodAudioClips.Length < textures.Count)
        {
            Debug.LogError("Nicht genügend Audiodateien für die Anzahl der Texturen. Bitte fügen Sie mehr Audiodateien hinzu.");
            return;
        }

        // Starte die Überwachung des Audioclips für das Bildwechseln
        StartCoroutine(CheckAudioEndRoutine());
    }

    IEnumerator CheckAudioEndRoutine()
    {
        while (true)
        {
            // Warte, bis das Audio des Players anfängt zu spielen
            yield return new WaitUntil(() => audioSource.isPlaying);

            // Warte, bis das Audio des Players zu Ende ist
            yield return new WaitWhile(() => audioSource.isPlaying);

            // Zufällige Verzögerung zwischen 0 und maxDelaySeconds
            float randomDelay = Random.Range(0, maxDelaySeconds);
            yield return new WaitForSeconds(randomDelay);

            // Nächstes Bild auswählen
            currentIndex = (currentIndex + 1) % textures.Count;
            Texture2D selectedTexture = textures[currentIndex];

            // Bild auf Material anwenden
            planeMaterial.SetTexture("_MainTex", selectedTexture); // für Albedo/Base Map
            planeMaterial.SetTexture("_BaseMap", selectedTexture); // Alternative für URP/HDRP Shader

            setImageScale(selectedTexture);

            // Abspielen des zugehörigen Audio-Clips
            if (selfAudio != null && moodAudioClips.Length > currentIndex)
            {
                selfAudio.clip = moodAudioClips[currentIndex];
                selfAudio.Play();
            }
        }
    }

    void setImageScale(Texture2D selectedTexture) {
            // Proportionen des Bildes berechnen und Plane skalieren
            float aspectRatio = (float)selectedTexture.width / selectedTexture.height;
            transform.localScale = new Vector3(originalScale.y * aspectRatio, originalScale.y, originalScale.z);
    }
}
