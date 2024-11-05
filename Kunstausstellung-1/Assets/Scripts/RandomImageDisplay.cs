using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomImageDisplay : MonoBehaviour
{
    public List<Texture2D> textures; // List of textures to be added in the Unity Inspector
    public AudioClip[] moodAudioClips; // Array of audio clips for each image

    private AudioSource audioSource; // External audio source
    private AudioSource selfAudio;   // Own AudioSource to play the clips
    private Material planeMaterial;
    private Vector3 originalScale;

    void Start()
    {
        // Find the Player GameObject with the tag "Player" and get its AudioSource
        GameObject player = GameObject.FindWithTag("Player");
        if (player != null)
        {
            audioSource = player.GetComponent<AudioSource>();
            if (audioSource == null)
            {
                Debug.LogError("No AudioSource found on the Player object.");
                return;
            }
        }
        else
        {
            Debug.LogError("No GameObject with the tag 'Player' found.");
            return;
        }

        selfAudio = GetComponent<AudioSource>();
        planeMaterial = GetComponent<Renderer>().material;
        originalScale = transform.localScale;

        if (textures.Count == 0)
        {
            Debug.LogError("No textures in the list. Please add textures in the Inspector.");
            return;
        }
        if (moodAudioClips.Length < textures.Count)
        {
            Debug.LogError("Not enough audio clips for the number of textures. Please add more audio clips.");
            return;
        }

        // Start the coroutine to check when to change the image
        StartCoroutine(CheckAudioEndRoutine());
    }

    IEnumerator CheckAudioEndRoutine()
    {
        while (true)
        {
            // Wait until the player's audio starts playing
            yield return new WaitUntil(() => audioSource.isPlaying);

            // Wait until the player's audio stops playing
            yield return new WaitWhile(() => audioSource.isPlaying);

            // Random delay between 20 and 60 seconds
            float randomDelay = Random.Range(2f, 10f);
            yield return new WaitForSeconds(randomDelay);

            // Randomly decide to show an image or not
            if (Random.value > 0.5f) // 50% chance to display an image
            {
                // Select a random image from the list
                int randomIndex = Random.Range(0, textures.Count);
                Texture2D selectedTexture = textures[randomIndex];

                // Apply the image to the material
                planeMaterial.SetTexture("_MainTex", selectedTexture); // for Albedo/Base Map
                planeMaterial.SetTexture("_BaseMap", selectedTexture); // Alternative for URP/HDRP Shader

                // Adjust the scale of the plane to match the texture's aspect ratio
                setImageScale(selectedTexture);

                // Play the corresponding audio clip
                if (selfAudio != null && moodAudioClips.Length > randomIndex)
                {
                    selfAudio.clip = moodAudioClips[randomIndex];
                    selfAudio.Play();
                }
            }
            else
            {
                // Clear the texture to show no image
                planeMaterial.SetTexture("_MainTex", null);
                planeMaterial.SetTexture("_BaseMap", null);
            }
        }
    }

    void setImageScale(Texture2D selectedTexture)
    {
        // Calculate aspect ratio and scale the plane accordingly
        float aspectRatio = (float)selectedTexture.width / selectedTexture.height;
        transform.localScale = new Vector3(originalScale.y * aspectRatio, originalScale.y, originalScale.z);
    }
}
