using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class PlayMusicOnTrigger : MonoBehaviour
{
    public float volumeInsideCollision = 0.2f; // Lautstärke, wenn das Objekt in der Kollision ist
    private float originalVolume;
    public GameObject player;
    private AudioSource audioSourcePlayer; // Der AudioSource des Spielers (oder eines anderen Objekts)
    
    private AudioSource audioSource;

    public bool allowReplay = true; // Bestimmt, ob der Sound erneut abgespielt werden soll
    private int playCounter = 0;    // Zählt, wie oft der Sound abgespielt wurde
    
    public float fadeDuration = 0.1f; // Dauer des Ausblendens in Sekunden

    private Coroutine fadeOutCoroutine;

    void Start()
    {
        // AudioSource des aktuellen GameObjects holen
        audioSource = GetComponent<AudioSource>();
        audioSourcePlayer = player.GetComponent<AudioSource>();

        // Sicherstellen, dass die audioSourcePlayer gesetzt ist
        if (audioSourcePlayer != null)
        {
            // Die Original-Lautstärke von audioSourcePlayer speichern
            originalVolume = 1f;
        }
        else
        {
            Debug.LogError("AudioSourcePlayer ist nicht gesetzt!");
        }
    }

    void OnTriggerEnter(Collider other)
    {
        // Prüfen, ob das kollidierende Objekt den Tag "Player" hat
        if (other.CompareTag("Player") && audioSourcePlayer != null)
        {
            // Überprüfen, ob der Sound erneut abgespielt werden darf
            if (allowReplay || playCounter == 0)
            {
                audioSource.volume = 2f;
                audioSource.Play();
                audioSourcePlayer.volume = volumeInsideCollision;
                playCounter++; // Zähler erhöhen
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        // Prüfen, ob das kollidierende Objekt den Tag "Player" hat
        if (other.CompareTag("Player") && audioSourcePlayer != null)
        {
            audioSourcePlayer.volume = originalVolume;
            audioSource.Stop();
        }
    }

    // Methode zum Zurücksetzen des Zählers, falls benötigt
    public void ResetPlayCounter()
    {
        playCounter = 0;
    }
}
