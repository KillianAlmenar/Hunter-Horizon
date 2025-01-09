using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeatherManager : MonoBehaviour
{
    [HideInInspector]
    public WeatherManager instance;
    [SerializeField]
    GameObject[] Clouds;
    [SerializeField]
    GameObject RainPref;
    [SerializeField]
    GameObject[] StormPref;
    [SerializeField]
    private WEATHER weather;
    private Color CloudColor = Color.white;
    [SerializeField]
    private float transitionSpeed = 1;
    private int[] weatherProba = new int[(int)WEATHER.COUNT];
    [SerializeField]
    GameObject Lightning;
    [SerializeField]
    private float lightningDelay;
    [SerializeField]
    LoopSoundPlayer rainSoundPlayer;

    private enum WEATHER
    {
        SUN,
        RAIN,
        STORM,
        COUNT
    }

    private void Awake()
    {
        if (instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
        }

        for (int i = 0; i < weatherProba.Length; i++)
        {
            weatherProba[i] = 50;
        }
    }

    private void Start()
    {
        switch (Inventory.instance.GetQuestElements())
        {
            case Elements.Element.NONE:
                DayNightManager.instance.currentTime = 0.5f;
                weather = (WEATHER)Random.Range(0, (int)WEATHER.COUNT);
                break;
            case Elements.Element.FIRE:
                weather = WEATHER.SUN;
                DayNightManager.instance.currentTime = 0.5f;
                break;
            case Elements.Element.WIND:
                weather = WEATHER.SUN;
                DayNightManager.instance.currentTime = 0f;
                break;
            case Elements.Element.ELECTRICITY:
                weather = WEATHER.STORM;
                DayNightManager.instance.currentTime = 0f;
                break;
            case Elements.Element.ICE:
                weather = WEATHER.RAIN;
                DayNightManager.instance.currentTime = 0.5f;
                break;
            default:
                break;
        }

        WeatherCycle();

    }

    private void Update()
    {

        if (Input.GetKeyDown(KeyCode.F1))
        {
            StopAllCoroutines();
            StartCoroutine("Sun");
        }

        if (Input.GetKeyDown(KeyCode.F2))
        {
            StopAllCoroutines();
            StartCoroutine("Rain");
        }

        if (Input.GetKeyDown(KeyCode.F3))
        {
            StopAllCoroutines();
            StartCoroutine("Storm");
        }

    }

    private void WeatherCycle()
    {
        switch (weather)
        {
            case WEATHER.SUN:
                StopAllCoroutines();
                StartCoroutine("Sun");
                break;
            case WEATHER.RAIN:
                StopAllCoroutines();
                StartCoroutine("Rain");
                break;
            case WEATHER.STORM:
                StartCoroutine("Storm");
                break;
        }
    }

    private IEnumerator Sun()
    {
        foreach (GameObject storm in StormPref)
        {
            if (storm.GetComponent<ParticleSystem>().isPlaying)
            {
                storm.GetComponent<ParticleSystem>().Stop();
            }
        }
        RainPref.GetComponent<ParticleSystem>().Stop();

        rainSoundPlayer.Stop();

        while (Clouds[0].GetComponent<MeshRenderer>().material.GetFloat("_CloudPower") < 17)
        {
            float cloudPower = Clouds[0].GetComponent<MeshRenderer>().material.GetFloat("_CloudPower");

            if (cloudPower < 17)
            {
                cloudPower += transitionSpeed * 10;
                Clouds[0].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
                Clouds[1].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
            }
            else
            {
                cloudPower = 0;
                Clouds[0].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
                Clouds[1].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
            }
            yield return new WaitForSeconds(transitionSpeed);
        }

        Clouds[0].GetComponent<MeshRenderer>().material.SetColor("_CloudColor", Color.white);
        Clouds[1].GetComponent<MeshRenderer>().material.SetColor("_CloudColor", Color.white);

        yield return null;
    }

    private IEnumerator Rain()
    {
        foreach (GameObject storm in StormPref)
        {
            if (storm.GetComponent<ParticleSystem>().isPlaying)
            {
                storm.GetComponent<ParticleSystem>().Stop();
            }
        }
        CloudColor = Clouds[0].GetComponent<MeshRenderer>().material.GetColor("_CloudColor");

        while (Clouds[0].GetComponent<MeshRenderer>().material.GetFloat("_CloudPower") != 1.5)
        {

            float cloudPower = Clouds[0].GetComponent<MeshRenderer>().material.GetFloat("_CloudPower");

            CloudColor = Clouds[0].GetComponent<MeshRenderer>().material.GetColor("_CloudColor");
            Color NormalizedAdd = new Color(Color.white.r - CloudColor.r, Color.white.g - CloudColor.g, Color.white.b - CloudColor.b);
            float norme = Mathf.Sqrt((NormalizedAdd.r * NormalizedAdd.r) + (NormalizedAdd.g * NormalizedAdd.g) + (NormalizedAdd.b * NormalizedAdd.b));

            if (norme != 0)
            {
                NormalizedAdd = new Color(NormalizedAdd.r / norme, NormalizedAdd.g / norme, NormalizedAdd.b / norme);

                CloudColor += NormalizedAdd * (transitionSpeed);
            }


            if (CloudColor.r > 1)
            {
                CloudColor = Color.white;
            }

            if (cloudPower < 1.4)
            {
                cloudPower += transitionSpeed * 3;
                Clouds[0].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
                Clouds[1].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
            }
            else if (cloudPower > 1.6)
            {
                cloudPower -= transitionSpeed * 10;
                Clouds[0].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
                Clouds[1].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
            }
            else
            {
                cloudPower = 1.5f;
                Clouds[0].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
                Clouds[1].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
            }

            Clouds[0].GetComponent<MeshRenderer>().material.SetColor("_CloudColor", new Color(CloudColor.r, CloudColor.g, CloudColor.b, 1));
            Clouds[1].GetComponent<MeshRenderer>().material.SetColor("_CloudColor", new Color(CloudColor.r, CloudColor.g, CloudColor.b, 1));

            yield return new WaitForSeconds(transitionSpeed);
        }

        RainPref.GetComponent<ParticleSystem>().Play();

        rainSoundPlayer.Play();

        yield return null;
    }

    private IEnumerator Storm()
    {
        RainPref.GetComponent<ParticleSystem>().Stop();

        CloudColor = Clouds[0].GetComponent<MeshRenderer>().material.GetColor("_CloudColor");

        while (CloudColor != Color.black)
        {
            float cloudPower = Clouds[0].GetComponent<MeshRenderer>().material.GetFloat("_CloudPower");

            CloudColor = Clouds[0].GetComponent<MeshRenderer>().material.GetColor("_CloudColor");
            Color NormalizedAdd = new Color(Color.black.r - CloudColor.r, Color.black.g - CloudColor.g, Color.black.b - CloudColor.b);

            float norme = Mathf.Sqrt((NormalizedAdd.r * NormalizedAdd.r) + (NormalizedAdd.g * NormalizedAdd.g) + (NormalizedAdd.b * NormalizedAdd.b));

            NormalizedAdd = new Color(NormalizedAdd.r / norme, NormalizedAdd.g / norme, NormalizedAdd.b / norme);

            CloudColor += NormalizedAdd * (transitionSpeed);

            if (CloudColor.r < 0)
            {
                CloudColor = Color.black;
            }

            if (cloudPower > 0.77)
            {
                cloudPower -= transitionSpeed * 10;
                Clouds[0].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
                Clouds[1].GetComponent<MeshRenderer>().material.SetFloat("_CloudPower", cloudPower);
            }

            Clouds[0].GetComponent<MeshRenderer>().material.SetColor("_CloudColor", new Color(CloudColor.r, CloudColor.g, CloudColor.b, 1));
            Clouds[1].GetComponent<MeshRenderer>().material.SetColor("_CloudColor", new Color(CloudColor.r, CloudColor.g, CloudColor.b, 1));

            yield return new WaitForSeconds(transitionSpeed);
        }
        foreach (GameObject storm in StormPref)
        {
            storm.GetComponent<ParticleSystem>().Play();
        }

        rainSoundPlayer.Play();

        yield return null;

        RainPref.GetComponent<ParticleSystem>().Clear();
        RainPref.GetComponent<ParticleSystem>().Stop();

        yield return new WaitForSeconds(1f);

        while (true)
        {
            Vector3 randomPos = new Vector3(Random.Range(-220, 50), 0, Random.Range(-160, 300));

            RaycastHit[] hits = Physics.RaycastAll(randomPos, Vector3.up);

            if (hits.Length == 0 || hits[0].transform.gameObject.name == "CloudMesh")
            {
                GameObject thunder = Instantiate(Lightning, randomPos, Quaternion.identity);
                Destroy(thunder, 1);

            }

            yield return new WaitForSeconds(lightningDelay);
        }

    }


}
