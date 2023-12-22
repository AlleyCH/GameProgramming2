using UnityEngine;

public class DayNightCycle : MonoBehaviour
{

    public event System.EventHandler OnChanged;
    public float dayDuration = 10.0f;
    public bool IsNight { get; private set; }
    public Color nightColor = Color.white * 0.1f;
    private Color dayColor;
    private Light lightComponent;

    void Start()
    {

        lightComponent = GetComponent<Light>();
        dayColor = lightComponent.color;

    }

    void Update()
    {

        float lightIntensity = 0.5f + Mathf.Sin(Time.time * 2.0f * Mathf.PI / dayDuration) / 2.0f;

        bool shouldBeNight = lightIntensity < 0.3f;

        if (IsNight != shouldBeNight)
        {

            IsNight = shouldBeNight;

            // Invoke event handler (if set).

            OnChanged?.Invoke(this,

              System.EventArgs.Empty);

        }

        lightComponent.color = Color.Lerp(nightColor,dayColor, lightIntensity);
    }

}
