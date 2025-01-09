using UnityEngine;

public class DayNightManager : MonoBehaviour
{
    public static DayNightManager instance;

    [SerializeField] Light dayLight;
    [SerializeField] Light nightLight;
    [SerializeField] Gradient directionalColor;
    [SerializeField] Gradient ambientColor;
    [SerializeField] Gradient fogColor;
    [SerializeField] Gradient skyboxColor;
    [SerializeField] AnimationCurve nightLevel;
    [SerializeField] float fogFactor;
    [SerializeField] float cycleDuration;
    [SerializeField] float startTime;
    [HideInInspector] public bool isNight;
    public float currentTime;
    public bool isPaused;
    public bool auto;
    Vector3 sunDirection;
    Light currentDayLight;
    Light currentNightLight;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
        instance.currentDayLight = dayLight;
        instance.currentNightLight = nightLight;
    }
    
    void Update()
    {
        if (isPaused)
            return;

        const float DAY_START = 0.2f;
        const float DAY_END = 0.78f;

        if (auto)
        {
            currentTime = (startTime + Time.time) % cycleDuration / cycleDuration;
        }
        currentTime %= 1f;
        isNight = currentTime < DAY_START || currentTime > DAY_END;
        float night = nightLevel.Evaluate(currentTime);
        
        RenderSettings.ambientLight = ambientColor.Evaluate(currentTime);

        sunDirection = new Vector3((360f * currentTime) - 90f, 170f, 0f);
        currentNightLight.transform.localRotation = Quaternion.Euler(-sunDirection);
        currentDayLight.transform.localRotation = Quaternion.Euler(sunDirection);
        currentDayLight.color = directionalColor.Evaluate(currentTime);

        RenderSettings.skybox.SetVector("_SunDirection", currentDayLight.transform.forward);
        RenderSettings.skybox.SetVector("_SunUp", currentDayLight.transform.up);
        RenderSettings.skybox.SetVector("_SunRight", currentDayLight.transform.right);
        RenderSettings.skybox.SetColor("_SkyTint", skyboxColor.Evaluate(currentTime));
        RenderSettings.skybox.SetFloat("_NightLevel", night);
    }
}
