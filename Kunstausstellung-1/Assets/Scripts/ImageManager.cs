using UnityEngine;
using System.Collections.Generic;

public class ImageManager : MonoBehaviour
{
    public bool evenPhase;
    public List<Texture2D> textures;
    public AudioClip[] moodAudioClips;
    public AudioClip[] voiceAudioClips;
    public int moodEffect;

    private Material planeMaterial;
    private Vector3 originalScale;
    private AudioSource audioSource;
    private MeshRenderer meshRenderer;
    private MeshCollider meshCollider;
    private GameObject player;
    private GameObject parent;
    private AudioSource voiceOverAudioSource;

    void Start()
    {
        planeMaterial = GetComponent<Renderer>()?.material;
        meshRenderer = GetComponent<MeshRenderer>();
        meshCollider = GetComponent<MeshCollider>();
        audioSource = GetComponent<AudioSource>();
        originalScale = transform.localScale;
        parent = transform.parent.gameObject;
        voiceOverAudioSource = parent.GetComponent<AudioSource>();

        player = GameObject.FindWithTag("Player");

        SetArtworkState(0);
    }

    public void SetArtworkState(int phase) {
        parent.transform.Find("Lightning")?.gameObject.SetActive(false);
        parent.transform.Find("Spot Light")?.gameObject.SetActive(false);
        if(evenPhase) {
            if(phase == 1) {
                int[] moodValues = player.GetComponent<MoodSystem>().moodValues;
                if(moodValues[0] == 1) {
                    enableArtwork(0);
                    if(moodEffect == -1) {
                        parent.transform.Find("Lightning")?.gameObject.SetActive(true);
                    }
                } else if(moodValues[0] == -1) {
                    enableArtwork(1);
                    parent.transform.Find("Lightning")?.gameObject.SetActive(true);
                }
            } else {
                ClearArtwork();
            }
        } else {
            if(phase == 0) {
                enableArtwork(0);
                if(moodEffect == 1) {
                    parent.transform.Find("Spot Light")?.gameObject.SetActive(true);
                }
            } else if(phase == 2) {
                int[] moodValues = player.GetComponent<MoodSystem>().moodValues;
                if(moodValues[0] == 1 && moodValues[1] == 1) {
                    enableArtwork(3);
                } else if((moodValues[0] == 1 && moodValues[1] == -1)) {
                    enableArtwork(2);
                    if(moodEffect == 1) {
                        parent.transform.Find("Spot Light")?.gameObject.SetActive(true);
                    }
                } else if((moodValues[0] == -1 && moodValues[1] == 1)) {
                    enableArtwork(2);
                    if(moodEffect == -1) {
                        parent.transform.Find("Spot Light")?.gameObject.SetActive(true);
                    }
                } else if(moodValues[0] == -1 && moodValues[1] == -1) {
                    enableArtwork(1);
                    if(moodEffect == -1) {
                        parent.transform.Find("Lightning")?.gameObject.SetActive(true);
                    }
                }
            } else {
                ClearArtwork();
            }
        }
    }

    void enableArtwork(int index) {
        meshRenderer.enabled = true;
        meshCollider.enabled = true;
        audioSource.clip = moodAudioClips[index];
        audioSource.Play();
        ChangeImage(textures[index]);
        voiceOverAudioSource.clip = voiceAudioClips[index];
    }

    void ClearArtwork() {
        meshRenderer.enabled = false;
        meshCollider.enabled = false;
        audioSource.clip = null;
        voiceOverAudioSource.clip = null;
    }

    private void ChangeImage(Texture2D selectedTexture)
    {
        if (selectedTexture != null)
        {
            planeMaterial.SetTexture("_MainTex", selectedTexture);
            SetImageScale(selectedTexture);
        }
    }

    private void SetImageScale(Texture2D selectedTexture)
    {
        float aspectRatio = (float)selectedTexture.width / selectedTexture.height;
        transform.localScale = new Vector3(originalScale.y * aspectRatio, originalScale.y, originalScale.z);
    }
}
