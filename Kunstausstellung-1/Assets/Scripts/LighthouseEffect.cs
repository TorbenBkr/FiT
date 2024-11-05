using UnityEngine;

[RequireComponent(typeof(Light))]
public class LighthouseEffect : MonoBehaviour
{
    public Color lightColor = Color.white; // Farbe des Lichts, im Editor anpassbar
    public float rotationSpeed = 30f;      // Geschwindigkeit der Rotation

    private Light spotlight;

    private void Start()
    {
        // Initialisieren des Spotlights und Setzen der Farbe
        spotlight = GetComponent<Light>();

        // Sicherstellen, dass wir ein Spotlight haben
        if (spotlight.type != LightType.Spot)
        {
            Debug.LogWarning("Das Licht sollte ein Spotlight sein, um den gewünschten Effekt zu erzielen.");
            return;
        }

        // Setzen der Farbe
        spotlight.color = lightColor;
    }

    private void Update()
    {
        // Rotation explizit um die lokale Y-Achse, um eine horizontale Bewegung zu gewährleisten
        transform.Rotate(0, rotationSpeed * Time.deltaTime, 0, Space.Self);
    }
}
