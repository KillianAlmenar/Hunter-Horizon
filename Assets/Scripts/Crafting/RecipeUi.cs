using UnityEngine.EventSystems;
using UnityEngine;
using UnityEngine.UI;

public class RecipeUi : MonoBehaviour, IPointerClickHandler
{
    public int Id;
    public bool selected = false;

    [SerializeField] private Image backGround;
    [SerializeField] private Color colorSelected;
    [SerializeField] private ButtonSelectDetect button;

    private static RecipeUi[] allRecipeUis;

    private void Start()
    {
        if (allRecipeUis == null)
        {
            allRecipeUis = FindObjectsOfType<RecipeUi>();
        }
    }

    private void SelectRecipe()
    {
        foreach (RecipeUi ui in allRecipeUis)
        {
            ui.selected = false;
            button.IsSelected = false;
        }

        selected = true;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        SelectRecipe();
    }

    private void Update()
    {
        if (button.IsSelected)
        {
            SelectRecipe();
        }

        backGround.color = selected ? colorSelected : Color.white;
        //selected = false;
    }
}
