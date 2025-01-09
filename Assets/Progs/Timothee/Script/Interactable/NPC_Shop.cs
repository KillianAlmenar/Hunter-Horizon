using System;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.UI;

public class NPC_Shop : MonoBehaviour, IInteractable
{
    private bool isOpen = false;

    [Header("Dialogue")]
    DialogueTrigger dialogueTrigger;
    [SerializeField] DialogueManager dialogueManager;

    [SerializeField] protected Animator animator;

    [Header("Menu")]
    [SerializeField] protected GameObject shopCanvas;
    [SerializeField] protected GameObject choices;
    [SerializeField] TMP_FontAsset shopFont;

    [Header("Slots and Layouts")]
    [SerializeField] protected GameObject itemSlotPrefab;
    [SerializeField] protected Transform buyItemCG;

    [Header("Shop Content")]
    [SerializeField] private List<Item> shopItemList;

    [SerializeField] GameObject NotEnoughGolds;

    [SerializeField] NPCSoundPlayer soundPlayer;

    #region GetterForEditor
    public List<Item> ShopArray
    {
        get => shopItemList;
        set
        {
            shopItemList = value;
        }
    }
    #endregion
    public void Interact()
    {
        if (!isOpen)
        {
            //Interaction code here
            dialogueTrigger = GetComponent<DialogueTrigger>();
            dialogueTrigger.TriggerDialogue();

            isOpen = true;

            dialogueManager.OnEndDialogue += OpenShopInterface;
        }
    }
    public void OpenShopInterface()
    {
        shopCanvas.SetActive(true);
        choices.SetActive(true);
        dialogueManager.OnEndDialogue-= OpenShopInterface;

        animator.SetTrigger("Open");
        foreach (Button button in choices.GetComponentsInChildren<Button>())
        {
            button.interactable = true;
        }
    }

    public void PlayCloseShopAnim()
    {
        animator.SetTrigger("Close");
        foreach (Button button in choices.GetComponentsInChildren<Button>())
        {
            button.interactable = false;
        }
    }
    public void CloseShopInterface()
    {
        choices.SetActive(false);
        shopCanvas.SetActive(false);
    }

    public void CanReInteract()
    {
        isOpen = false;
    }

    //Slots relative
    public void InstantiateSlotsInMenu()
    {
        int i = 0; //count

        //Fill buy menu
        foreach (Item item in shopItemList)
        {
            GameObject _itemSlot = Instantiate(itemSlotPrefab, buyItemCG);

            _itemSlot.transform.Find("Name").gameObject.GetComponent<TMP_Text>().text = shopItemList[i].itemName;
            _itemSlot.transform.Find("Name").gameObject.GetComponent<TMP_Text>().font = shopFont;

            _itemSlot.transform.Find("Icon").gameObject.GetComponent<RawImage>().texture = shopItemList[i].itemIcon.texture;
            _itemSlot.transform.Find("Value").gameObject.GetComponent<TMP_Text>().text = Convert.ToString(shopItemList[i].itemValue) + " Golds";
            _itemSlot.transform.Find("Value").gameObject.GetComponent<TMP_Text>().font = shopFont;

            _itemSlot.transform.Find("BuyButton").gameObject.GetComponent<Button>().onClick.AddListener(() => Purchase(item));

            i++;
        }
    }
    public void DestroySlotsInMenu()
    {
        //Clear buy mat slot instance
        for (int i = 0; i < buyItemCG.childCount; i++)
        {
            Destroy(buyItemCG.GetChild(i).gameObject);
        }
    }

    public void Purchase(Item item)
    {
        if (Inventory.instance.golds >= item.itemValue) //Enough money
        {
            //Debug.Log("Bought " + item.itemName + " for " + item.itemValue + " golds");
            //Debug.Log("Player remaining gold : " + Inventory.instance.golds);

            //Transaction
            Inventory.instance.AddItem(item);
            Inventory.instance.golds -= item.itemValue;

            soundPlayer.PlayBuy();
        }
        else
        {
            Instantiate(NotEnoughGolds);
        }
    }
}
