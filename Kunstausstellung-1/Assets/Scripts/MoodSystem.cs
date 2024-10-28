using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoodSystem : MonoBehaviour
{
    public int mood = 0;
    private int lastMood = -1; // Speichert den letzten Wert von mood, um Änderungen zu verfolgen
    private AudioSource audioSource;
    private Texture2D lastSeenArtworkTexture; // Speichert die letzte gesehene Textur

    public AudioClip[] moodAudioClips;
    
    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        
        // Sicherstellen, dass genügend AudioClips vorhanden sind
        if (moodAudioClips.Length < 5)
        {
            Debug.LogError("Nicht genügend AudioClips für alle Moods vorhanden. Bitte 5 Clips hinzufügen.");
        }
    }

    // Update wird einmal pro Frame aufgerufen
    void Update()
    {
        // Überprüfe, ob die Audio abgespielt wurde
        if (!audioSource.isPlaying && audioSource.time == 0)
        {
            // Raycast zur Überprüfung, ob der Spieler ein Artwork ansieht
            if (IsLookingAtArtwork(out Texture2D currentTexture))
            {
                // Prüfen, ob das aktuelle Artwork-Bild ein neues ist
                if (currentTexture != lastSeenArtworkTexture && currentTexture != null)
                {
                    // Mood um 1 erhöhen
                    mood += 1;

                    // Speichere die aktuelle Textur als die zuletzt gesehene Textur
                    lastSeenArtworkTexture = currentTexture;
                }
            }
        }

        
        // Nur wenn sich die Stimmung ändert, wird der Sound abgespielt
        if (mood != lastMood && mood > 0 && mood <= moodAudioClips.Length)
        {
            lastMood = mood; // Aktualisiere den letzten Mood-Wert
            PlayMoodAudio(mood - 1); // Passenden Sound abspielen
        }
    }


    private void PlayMoodAudio(int clipIndex)
    {
        // Setze das neue AudioClip und spiele es nur einmal ab
        audioSource.clip = moodAudioClips[clipIndex];
        audioSource.Play();
    }

    // Funktion, um zu prüfen, ob der Spieler ein Artwork ansieht
    bool IsLookingAtArtwork(out Texture2D artworkTexture)
    {
        artworkTexture = null;

        // Ray aus der Kamera des Spielers abfeuern
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        RaycastHit hit;

        // Überprüfen, ob der Ray ein Objekt mit dem Tag "Artwork" trifft
        if (Physics.Raycast(ray, out hit))
        {
            if (hit.collider.CompareTag("Artwork"))
            {
                // Hole die Textur des Artwork-Objekts
                Renderer renderer = hit.collider.GetComponent<Renderer>();
                if (renderer != null)
                {
                    artworkTexture = renderer.material.GetTexture("_MainTex") as Texture2D;
                }
                return true;
            }
        }
        return false;
    }
}