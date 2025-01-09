using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using System.Linq;

public class ShowQuest : MonoBehaviour
{
    private Vector3 startPos;
    [SerializeField] float speed;

    [SerializeField] TextMeshProUGUI questName;
    [SerializeField] TextMeshProUGUI questDescription;
    [SerializeField] TextMeshProUGUI questQuantity;

    [SerializeField] Button acceptButton;

    [SerializeField] Image BackGround;
    [SerializeField] Sprite[] backGrounds;

    QuestDataBase database;

    Quest searchQuest = new();

    private GameObject m_button;


    private void OnEnable()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
    }

    private IEnumerator Grow()
    {
        float time = 0; 
        while (transform.localScale.x < 1)
        {
            time += 1f * Time.deltaTime * speed;

            transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(1f,1f,1f), time);
            m_button.transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), time * 2f);

            //transform.position = Vector3.Lerp(startPos, new Vector3(Screen.width/2, Screen.height / 2, 0), time);
            yield return null;
        }
    }

    private IEnumerator UnGrow()
    {
        float time = 0;
        while (transform.localScale.x > 0)
        {
            time += 1f * Time.deltaTime * speed;

            transform.localScale = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), time);
            m_button.transform.localScale = Vector3.Lerp(new Vector3(0f, 0f, 0f), new Vector3(1f, 1f, 1f), time*2f);

            //transform.position = Vector3.Lerp(new Vector3(Screen.width / 2, Screen.height / 2, 0), startPos, time);

            if (transform.localScale.x <= 0)
                gameObject.SetActive(false);
            yield return null;
        }
    }

    private IEnumerator DesApear()
    {
        float time = 0;
        while (transform.localScale.x > 0)
        {
            time += 1f * Time.deltaTime * speed;

            transform.GetComponent<CanvasGroup>().alpha = Vector3.Lerp(new Vector3(1f, 1f, 1f), new Vector3(0f, 0f, 0f), time).x;

            if (transform.GetComponent<CanvasGroup>().alpha <= 0)
            {
                gameObject.SetActive(false);
                transform.localScale = new Vector3(0f, 0f, 0f);
                transform.GetComponent<CanvasGroup>().alpha = 1;
            }
                
            yield return null;
        }
    }


    public void ActiveShowQuest(Vector3 _startPos,string quest_Id, GameObject button)
    {
        if (!gameObject.activeSelf)
        {
            database = Resources.Load<QuestDataBase>("QuestDatabase");

            searchQuest = database.GetQuestsById(quest_Id);

            m_button = button;

            Quest temp = searchQuest;

            questName.text = temp.questName;
            questDescription.text = temp.questDescription;
            questQuantity.text = "0 / " + temp.quantity.ToString();

            BackGround.sprite = backGrounds[(int)temp.type];


            gameObject.SetActive(true);
            //transform.position = startPos;
            //startPos = _startPos;

            StartCoroutine(Grow());

            acceptButton.Select();
        }
    }

    public void DeActiveShowQuest()
    {
        StartCoroutine(UnGrow());
        QuestManager.Instance.SelectFirstQuest();
    }

    public void AcceptQuest()
    {
        if (transform.localScale.x >= 1)
        {
            Inventory.instance.activeQuest.Add(searchQuest.Clone());
            m_button.transform.SetAsLastSibling();
            Destroy(m_button);

            m_button = null;
            StartCoroutine(DesApear());
            QuestManager.Instance.SelectFirstQuest();
        }

    }

    private void OnDisable()
    {
        transform.localScale = new Vector3(0f, 0f, 0f);
        gameObject.SetActive(false);
        m_button.transform.localScale = new Vector3(1f, 1f, 1f);
    }
}
