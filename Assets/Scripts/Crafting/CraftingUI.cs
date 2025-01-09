using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class CraftingUI : MonoBehaviour
{
    public CraftingSystem craftingSystem;
    public GameObject recipePrefab;
    [SerializeField] private GameObject itemCasePrefab;
    [SerializeField] private GameObject recipeListScroll;
    [SerializeField] private GameObject resultPreviewItem;

    private int actualRecipe = 0;

    private Image resultImage;
    private TextMeshProUGUI resultText;

    private void Start()
    {
        InitializeUI();
    }

    private void OnEnable()
    {
        //InitializeUI();

        /*
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None;
        */

        GameManager.instance.UpdateCursor();
    }

    private void OnDisable()
    {
        //DestroyRecipesList();
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    private void InitializeUI()
    {
        PopulateRecipes();
        resultImage = resultPreviewItem.GetComponent<Image>();
        resultText = resultPreviewItem.GetComponentInChildren<TextMeshProUGUI>();

        UpdateResultPreview();

        GetComponentInChildren<Scrollbar>().value = 1f;
    }

    private void Update()
    {
        if (recipeListScroll.transform.childCount == 0)
        {
            InitializeUI();
        }
        DisplaySelectedRecipe();
    }

    /// <summary>
    /// Update l'image + description de l'item de la recette selectionné
    /// </summary>
    private void UpdateResultPreview()
    {
        if (craftingSystem.recipes.Count != 0)
        {
            Item item = craftingSystem.recipes[actualRecipe].resultItem;
            resultImage.sprite = item.itemIcon;
            resultText.text = $"{item.itemName} :\n{item.description}";
        }
    }

    /// <summary>
    /// Instancie toutes les recettes
    /// </summary>
    private void PopulateRecipes()
    {
        int idRecipe = 0;
        foreach (Recipe recipe in craftingSystem.recipes)
        {
            CreateRecipeUI(recipe, idRecipe);
            idRecipe++;
        }

        if (recipeListScroll.transform.childCount != 0)
            recipeListScroll.transform.GetChild(0).GetComponentInChildren<Button>().Select();
    }

    private void DestroyRecipesList()
    {
        for (int i = 0; i < recipeListScroll.transform.childCount; i++)
        {
            Destroy(recipeListScroll.transform.GetChild(i).gameObject);
        }
    }

    /// <summary>
    /// Instancie et parametre le prefab d'une recette
    /// </summary>
    /// <param name="recipe"></param>
    /// <param name="idRecipe"></param>
    private void CreateRecipeUI(Recipe recipe, int idRecipe)
    {
        GameObject recipeInstance = Instantiate(recipePrefab, recipeListScroll.transform);
        recipeInstance.GetComponent<RecipeUi>().Id = idRecipe;

        SetResultItemIcon(recipeInstance, recipe);
        ConfigureRequiredItems(recipeInstance, recipe);
        AddCraftingListener(recipeInstance, recipe);
    }

    /// <summary>
    /// parametre l'image de l'Item final
    /// </summary>
    /// <param name="recipeInstance"></param>
    /// <param name="recipe"></param>
    private void SetResultItemIcon(GameObject recipeInstance, Recipe recipe)
    {
        Transform itemCaseTransform = recipeInstance.transform.Find("ItemCase");
        Image itemImage = itemCaseTransform.Find("Image").GetComponent<Image>();

        if (recipe.resultItem != null)
        {
            itemImage.sprite = recipe.resultItem.itemIcon;
        }
        else
        {
            Debug.LogError($"The recipe result item does not inherit from Item. Recipe: {recipe.resultItem.itemName}");
        }
    }

    /// <summary>
    /// Ajoute les Items necessaires a la recette
    /// </summary>
    /// <param name="recipeInstance"></param>
    /// <param name="recipe"></param>
    private void ConfigureRequiredItems(GameObject recipeInstance, Recipe recipe)
    {
        Transform recipeItemsContainer = recipeInstance.transform.Find("RecipeItemsContainer");
        foreach (Item item in recipe.requiredItems)
        {
            if (item != null)
            {
                UpdateOrCreateItemIcon(recipeItemsContainer, item);
            }
            else
            {
                Debug.LogError($"Recipe for Item: {recipe.resultItem.itemName} has an item that does not inherit from Item.");
            }
        }
    }

    /// <summary>
    /// Ajoute ou incremente la quantité d'un Item pour la recette
    /// </summary>
    /// <param name="recipeItemsContainer"></param>
    /// <param name="item"></param>
    private void UpdateOrCreateItemIcon(Transform recipeItemsContainer, Item item)
    {
        Transform existingItemIcon = recipeItemsContainer.Find(item.itemName);
        if (existingItemIcon == null)
        {
            GameObject itemIcon = Instantiate(itemCasePrefab, recipeItemsContainer);
            itemIcon.name = item.itemName;
            itemIcon.transform.Find("Image").GetComponent<Image>().sprite = item.itemIcon;
            itemIcon.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = "1";
        }
        else
        {
            int currentCount = int.Parse(existingItemIcon.transform.Find("Number").GetComponent<TextMeshProUGUI>().text);
            existingItemIcon.transform.Find("Number").GetComponent<TextMeshProUGUI>().text = (currentCount + 1).ToString();
        }
    }

    /// <summary>
    /// Ajoute un comportement quand on appuie sur le boutton de craft
    /// </summary>
    /// <param name="recipeInstance"></param>
    /// <param name="recipe"></param>
    private void AddCraftingListener(GameObject recipeInstance, Recipe recipe)
    {
        recipeInstance.GetComponentInChildren<Button>().onClick.AddListener(() => craftingSystem.Craft(recipe.resultItem));
    }

    /// <summary>
    /// Pour savoir quel recette afficher dans la section du bas
    /// </summary>
    private void DisplaySelectedRecipe()
    {
        RecipeUi[] recipeUi = FindObjectsOfType<RecipeUi>();
        foreach (RecipeUi ui in recipeUi)
        {
            if (ui.selected && actualRecipe != ui.Id)
            {
                actualRecipe = ui.Id;
                UpdateResultPreview();
                break;
            }
        }
    }
}
