using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(AudioSource))]
public class LightningEffect : MonoBehaviour
{
    private Light pointLight;
    private AudioSource thunderSound;
    private bool hasFlashed = false;

    [SerializeField] private float maxIntensity = 1000f;
    [SerializeField] private float flashDuration = 0.2f;
    [SerializeField] private float fadeDuration = 0.5f;

    private void Awake()
    {
        pointLight = GetComponent<Light>();
        thunderSound = GetComponent<AudioSource>();

        // Start with light intensity at 0
        pointLight.intensity = 0;
    }

    private void OnEnable()
    {
        // Start the lightning effect only if it hasn't flashed yet
        if (!hasFlashed)
        {
            StartCoroutine(LightningFlashRoutine());
        }
    }

    private System.Collections.IEnumerator LightningFlashRoutine()
    {
        // Play thunder sound after the light fades out
        thunderSound.Play();
        
        hasFlashed = true;

        // Flash phase: increase intensity quickly
        float timer = 0;
        while (timer < flashDuration)
        {
            timer += Time.deltaTime;
            pointLight.intensity = Mathf.Lerp(0, maxIntensity, timer / flashDuration);
            yield return null;
        }

        // Short burst at max intensity
        pointLight.intensity = maxIntensity;
        yield return new WaitForSeconds(0.05f);

        // Fade phase: decrease intensity slowly
        timer = 0;
        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            pointLight.intensity = Mathf.Lerp(maxIntensity, 0, timer / fadeDuration);
            yield return null;
        }


        // Wait for the thunder sound to finish before deactivating the object
        yield return new WaitForSeconds(thunderSound.clip.length);

        // Deactivate the object
        gameObject.SetActive(false);
    }
}
