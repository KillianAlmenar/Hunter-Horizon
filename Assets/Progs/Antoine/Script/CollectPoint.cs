using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CollectPoint : MonoBehaviour, IInteractable
{
    [SerializeField] GameObject PrefabToSpawn;
    [SerializeField] GameObject DisplayImage;
    [SerializeField] public string ID;
    public float duration;

    private bool recolting = false;
    [SerializeField]
    public List<Item> Items;
    [SerializeField] Slider slider;
    [SerializeField] GameObject sliderGo;

    [SerializeField] Transform Canvas;
    [SerializeField] GameObject itemFirstDiscovery;
    [SerializeField] GameObject itemDiscovery;
    [SerializeField] Transform spawnPoPUp;

    private float actualSize = 0;
    private bool collectedOneTime = false;


    public int min;
    public int max;


    private void Awake()
    {
        actualSize = gameObject.transform.localScale.x;
    }

    public void Interact()
    {
        if (!recolting)
        {
            sliderGo.SetActive(true);
            recolting = true;
            slider.value = 0;
            StartCoroutine(FillSlider());
            StartCoroutine(ReduceCollectable());
            collectedOneTime = true;
        }
    }

    private IEnumerator FillSlider()
    {
        float timer = 0f;
        float startValue = slider.value;
        float endValue = 1f;

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float progress = Mathf.Clamp01(timer / duration);
            slider.value = Mathf.Lerp(startValue, endValue, progress);
            yield return null;
        }

        slider.value = endValue;

        InstantiatePoPUp(Items[GetRandomItems()]);

        GameManager.instance.IsInAnInteraction = false;
        sliderGo.SetActive(false);
        recolting = false;
    }

    private IEnumerator ReduceCollectable()
    {
        if (!collectedOneTime)
        {
            while (gameObject.transform.localScale.x > actualSize / 2)
            {
                gameObject.transform.localScale -= new Vector3(1,1, 1) * Time.deltaTime;
                yield return null;
            }
        }
        else
        {
            Destroy(gameObject,duration+0.1f);
            while (gameObject.transform.localScale.x > 0)
            {
                gameObject.transform.localScale -= new Vector3(1, 1, 1) * Time.deltaTime;
                yield return null;
            }

            
        }


        actualSize = gameObject.transform.localScale.x;
        yield return null;
    }

    private int GetRandomItems()
    {
        return Random.Range(0, Items.Count);
    }
    private void InstantiatePoPUp(Item items)
    {
        int amount = Random.Range(min, max);
        for (int i = 0; i < amount; i++)
        {
            Inventory.instance.AddItem(items);
            QuestManager.Instance.UpdatePlayerQuest(ID);
        }

        GameObject go = Instantiate(itemDiscovery, spawnPoPUp.position, Quaternion.identity, Canvas);
        go.GetComponent<ItemPoPUp>().Name.GetComponent<TextMeshProUGUI>().text = items.itemName;
        go.GetComponent<ItemPoPUp>().Description.GetComponent<TextMeshProUGUI>().text = new string("x " + amount);
        go.GetComponent<ItemPoPUp>().Item.GetComponent<Image>().sprite = items.itemIcon;
        if (checkIfFirstTime())
        {
            go = Instantiate(itemFirstDiscovery, spawnPoPUp.position, Quaternion.identity, Canvas);
            go.GetComponent<ItemPoPUp>().Name.GetComponent<TextMeshProUGUI>().text = items.itemName;
            go.GetComponent<ItemPoPUp>().Description.GetComponent<TextMeshProUGUI>().text = items.description;
            go.GetComponent<ItemPoPUp>().Item.GetComponent<Image>().sprite = items.itemIcon;
        }

    }

    private bool checkIfFirstTime()
    {
        return false;
    }
    private void Update()
    {

    }
}
