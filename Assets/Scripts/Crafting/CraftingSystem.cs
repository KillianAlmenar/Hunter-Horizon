using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CraftingSystem : MonoBehaviour
{
    public List<Recipe> recipes = new List<Recipe>();
    [SerializeField] private int recipeWanted;
    [SerializeField]Animator m_Animator;
    [SerializeField] NPCSoundPlayer soundPlayer;

    public delegate void OnCraftSuccess();
    public static event OnCraftSuccess CraftSuccessEvent;

    [SerializeField] private GameObject PopUpNotEnough;

    private void Start()
    {
        recipes = Resources.Load<RecipeDataBase>("RecipeDatabase").recipes;
    }

    /// <summary>
    /// Fonction principale de craft
    /// </summary>
    /// <param name="resultItem"></param>
    public void Craft(Item resultItem)
    {
        if (!IsResultItemValid(resultItem))
        {
            Debug.LogError("The final craft item does not inherit from Item.");
            return;
        }

        Recipe recipe;
        if (FindRecipe(resultItem, out recipe))
        {
            if (Inventory.instance.HasItems(recipe.requiredItems))
            {
                //Inventory.instance.RemoveItems(recipe.requiredItems);
                //Inventory.instance.AddItem(resultItem);

                StartCoroutine(CraftAnim(recipe.requiredItems, resultItem));
                //Debug.Log("Crafted: " + resultItem.itemName);
                //CraftSuccessEvent?.Invoke();

                soundPlayer.PlayBuy();
            }
            else
            {
                Instantiate(PopUpNotEnough);
                //Debug.Log("Cannot craft " + resultItem.itemName + ". Missing required items.");
            }
        }
        else
        {
            //Debug.Log("Cannot find recipe for " + resultItem.itemName);
        }
    }

    IEnumerator CraftAnim(List<Item> itemRemove, Item resultItem)
    {
        m_Animator.SetTrigger("Craft");

        yield return new WaitForSeconds(2f);
        

        Inventory.instance.RemoveItems(itemRemove);
        Inventory.instance.AddItem(resultItem);

        CraftSuccessEvent?.Invoke();
    }

    /// <summary>
    /// Verifie si c'est un Item (herite de la classe Item)
    /// </summary>
    /// <param name="resultItem"></param>
    /// <returns></returns>
    private bool IsResultItemValid(Item resultItem)
    {
        return resultItem != null;
    }

    /// <summary>
    /// Verifie si il existe la recette pour l'Item demandé
    /// </summary>
    /// <param name="resultItem"></param>
    /// <param name="foundRecipe"></param>
    /// <returns></returns>
    private bool FindRecipe(Item resultItem, out Recipe foundRecipe)
    {
        foreach (Recipe recipe in recipes)
        {
            if (recipe.resultItem == resultItem)
            {
                foundRecipe = recipe;
                return true;
            }
        }
        foundRecipe = null;
        return false;
    }

    private void Update()
    {
        //Debug
        if (Input.GetKeyDown(KeyCode.O))
        {
            Inventory.instance.AddItem(Resources.Load<ItemDataBase>("ItemsDatabase").items[0]);
            /*
            if (recipes.Count > recipeWanted)
            {
                for (int i = 0; i < recipes[recipeWanted].requiredItems.Count; i++)
                {
                    Inventory.instance.AddItem(recipes[recipeWanted].requiredItems[i]);
                }
                CraftSuccessEvent?.Invoke();
            }
            */
            CraftSuccessEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.L))
        {
            Inventory.instance.AddItem(Resources.Load<ItemDataBase>("ItemsDatabase").items[1]);
            CraftSuccessEvent?.Invoke();
        }

        if (Input.GetKeyDown(KeyCode.P) && recipes.Count > recipeWanted)
        {
            Craft(recipes[recipeWanted].resultItem);
            CraftSuccessEvent?.Invoke();
        }
    }
}

