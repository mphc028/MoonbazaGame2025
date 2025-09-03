using UnityEngine;

[CreateAssetMenu(fileName = "CraftingCategory", menuName = "Scriptable Objects/CraftingCategory")]
public class CraftingCategory : ScriptableObject
{
    [Header("Category Information")]

    [Tooltip("Unique identifier for the category.")]
    public int id;

    [Tooltip("Display name of the crafting category.")]
    public string categoryName;

    [Tooltip("Icon representing this category in the UI.")]
    public Sprite icon;
}
