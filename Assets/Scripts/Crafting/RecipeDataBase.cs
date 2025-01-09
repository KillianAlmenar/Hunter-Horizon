using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RecipeDatabase")]
public class RecipeDataBase : ScriptableObject
{
    public List<Recipe> recipes = new List<Recipe>();
}
