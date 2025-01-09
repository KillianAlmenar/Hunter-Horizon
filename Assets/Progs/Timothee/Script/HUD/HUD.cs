using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
public class HUD : MonoBehaviour
{
    [Header("Parameters")]
    private float fadingSpeed = 5;
    private ThirdPersonPlayer player;

    [Header("UI Image")]
    [SerializeField] private Image hpBar;
    [SerializeField] private List<Image> rageBars;

    [Header("InteractText")]
    [SerializeField] private GameObject interactText;
    PlayerInteraction playerInteraction;

    #region Local Var
    private bool startCanvaFade = false;
    private bool fadeType = false; //true = fade in false = fade out
    private CanvasGroup canvaGroup;
    private Image currentRageBar;
    // 

    #endregion

    private void Awake()
    {
        player = GameObject.FindWithTag("Player").GetComponent<ThirdPersonPlayer>();
        playerInteraction = player.GetComponent<PlayerInteraction>();
    }
    private void Start()
    {
        GameManager.instance.InGameHUD = gameObject;
        canvaGroup = GetComponent<CanvasGroup>();
        
    }
    private void Update()
    {
        DisplayHp(player.GetHealth());
        DisplayRage(player.GetRage());
        DisplayTextInteraction();

        if (startCanvaFade)
        {
            CallFading(fadeType);
        }
    }

    #region Fading UI Functions

    private void CallFading(bool _in)
    {
        if (_in == true)
        {
            if (canvaGroup.alpha < 1)
            {
                canvaGroup.alpha += fadingSpeed * Time.deltaTime;
            }
            else
            {
                startCanvaFade = false;
            }
        }
        else
        {
            if (canvaGroup.alpha > 0)
            {
                canvaGroup.alpha -= fadingSpeed * Time.deltaTime;
            }
            else
            {
                startCanvaFade = false;
            }
        }
    }
    public void Fade(bool _in)
    {
        startCanvaFade = true;
        fadeType = _in;
    }
    #endregion

    #region Display Bars Functions

    public void DisplayHp(float _health)
    {
        if (_health >= 0 && _health <= player.GetMaxHealth())
        {
            hpBar.fillAmount = _health / player.GetMaxHealth();
        }

    }

    public void DisplayRage(float _rage)
    {
        if (_rage >= 0 && _rage < player.GetMaxRage() && _rage < player.GetMaxRage()) //Display if correct data
        {
            int currentBar = (int)(_rage/(player.GetMaxRage()/4));

            currentRageBar = rageBars[currentBar];
            currentRageBar.fillAmount =(_rage - currentBar * player.GetMaxRage()/4) / (player.GetMaxRage()/4);


            for (int i = currentBar + 1; i < rageBars.Count; i++)
            {
                if (i < rageBars.Count)
                    rageBars[i].fillAmount = 0;
            }

            for (int i = currentBar - 1; i >= 0; i--)
            {
                if (i >= 0)
                    rageBars[i].fillAmount = 1;
            }

        }

        if (_rage == player.GetMaxRage())
        {
            foreach (Image img in rageBars)
            {
                img.fillAmount = 1;
            }
        }

    }
    #endregion

    #region DisplayTextInteraction

    private void UpdateInteractTextContent()
    {
        interactText.GetComponent<TMP_Text>().text = playerInteraction.InteractTextContent;
    }

    private void DisplayTextInteraction()
    {
        UpdateInteractTextContent();

        if (playerInteraction.IsInInteractRange)
        {
            interactText.SetActive(true);
        }
    }

    public void PlayTextInAnim()
    {
        interactText.transform.localScale = Vector3.zero;
        interactText.GetComponent<Animator>().Play("TextIn");
    }

    public void PlayTextOutAnim() 
    {
        interactText.GetComponent<Animator>().Play("TextOut");
    }
    #endregion
}
