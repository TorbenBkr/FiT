using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AudioMoveObject : MonoBehaviour
{
    // Referenz auf das AudioSource-Objekt
    public AudioSource audioSource;

    // Das GameObject, das nach dem Abspielen der Audio bewegt wird
    public GameObject objectToMove;

    // Die Geschwindigkeit, mit der das Objekt nach unten bewegt wird
    public float moveSpeed = 2f;

    // Ob das Objekt bewegt werden soll
    private bool shouldMove = false;

    void Start()
    {
        // Hole die AudioSource-Komponente
        audioSource = GetComponent<AudioSource>();
    }

    void OnTriggerEnter(Collider other)
    {
        // Prüfen, ob das kollidierende Objekt den Tag "Player" hat
        if (other.CompareTag("Player")) 
        {
            // Starte die Coroutine, um zu warten, bis das Audio abgespielt wurde
            StartCoroutine(WaitForAudioToEnd());
        }
    }

    // Coroutine, die wartet, bis das Audio fertig ist
    private IEnumerator WaitForAudioToEnd()
    {
        // Warten, bis das Audio abgespielt wurde
        while (audioSource.isPlaying)
        {
            yield return null; // Warten auf den nächsten Frame
        }

        // Nachdem das Audio fertig ist, das Flag setzen, um das Objekt zu bewegen
        shouldMove = true;
    }

    void Update()
    {
        // Wenn das Flag gesetzt ist, bewege das Objekt nach unten
        if (shouldMove)
        {
            objectToMove.transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        }
    }
}
