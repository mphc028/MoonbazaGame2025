using UnityEngine;

public class InventorySlot : MonoBehaviour
{
    private DraggableItem currentItem;
    private CraftingGridManager gridManager;

    [Header("Grid Position (for crafting manager)")]
    public int row;
    public int column;

    private void Awake()
    {
        gridManager = GameObject.FindWithTag("CraftingGrid")?.GetComponent<CraftingGridManager>();
    }

    public bool IsEmpty()
    {
        return currentItem == null;
    }

    public void SetItem(DraggableItem item)
    {
        currentItem = item;

        if (gridManager != null)
        {
            CraftingComponent comp = item != null ? item.craftingComponent : null;
            Debug.Log($"SetItem at [{row},{column}] = {(comp != null ? comp.componentName : "null")}");
            gridManager.SetComponentAt(row, column, comp);
            gridManager.RecalculateOutputs();
        }
    }

    public void Clear()
    {
        currentItem = null;

        if (gridManager != null)
        {
            gridManager.SetComponentAt(row, column, null);
            gridManager.RecalculateOutputs();
        }
    }

    public DraggableItem GetItem()
    {
        return currentItem;
    }
}
