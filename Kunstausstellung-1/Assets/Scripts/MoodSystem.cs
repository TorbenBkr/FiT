using UnityEngine;
using System.Collections;

public class MoodSystem : MonoBehaviour
{
    public int currentPhase = 0;  // Phase index, ranging from 0 to 3
    public int[] moodValues = new int[4];  // Mood for each phase, values from -1, 0, 1
    private int currentMood = 0;  // Current mood of the viewer, clamped between -1 and 1
    public GameObject viewedArtwork;  // Current artwork being viewed
    private bool isVoiceOverPlaying = false;  // Blocks raycast if a voice-over is playing
    public GameObject parentObject;
    public ImageManager manager;

    private AudioSource voiceOver;

    private const int maxPhase = 3; // Define max phase for clarity

    void Start()
    {
        moodValues = new int[4]; // Initialize mood values to zero
    }

    void Update()
    {
        if (!isVoiceOverPlaying)
        {
            RaycastForArtwork();
        }
    }

    private void RaycastForArtwork()
    {
        Ray ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0));
        if (Physics.Raycast(ray, out RaycastHit hit) && hit.collider.CompareTag("Artwork"))
        {
            viewedArtwork = hit.collider.gameObject;
            manager = viewedArtwork.GetComponent<ImageManager>();
            parentObject = viewedArtwork.transform.parent.gameObject;
            voiceOver = parentObject.GetComponent<AudioSource>();
            if(!isVoiceOverPlaying) {
                voiceOver.Play();
                moodValues[currentPhase] = manager.moodEffect;
                isVoiceOverPlaying = true;
                StartCoroutine(EndVoiceOver(voiceOver));
            }
        }
    }

    private IEnumerator EndVoiceOver(AudioSource voiceOver)
    {
        yield return new WaitForSeconds(voiceOver.clip.length);
        IncreasePhase();
    }

    private void IncreasePhase()
    {
        if (currentPhase < maxPhase)
        {
            currentPhase++;
            StartCoroutine(HandleArtworkTransition());
        }
    }

    private IEnumerator HandleArtworkTransition()
    {
        float randomValue = Random.Range(0f, 2f);
        yield return new WaitForSeconds(randomValue); // Duration for the transition effect

        GameObject[] artworks = GameObject.FindGameObjectsWithTag("Artwork");
        foreach (GameObject artwork in artworks)
        {
            artwork.GetComponent<MeshRenderer>().enabled = false;
            ImageManager manager = artwork.GetComponent<ImageManager>();
            if (manager != null)
            {
                manager.SetArtworkState(currentPhase);
            }
        }
        isVoiceOverPlaying = false;
    }
}
