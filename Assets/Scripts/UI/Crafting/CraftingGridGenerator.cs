using UnityEngine;
using UnityEngine.UI;

public class CraftingGridGenerator : MonoBehaviour
{
    [Header("Prefab to spawn in grid")]
    public GameObject craftingSlotPrefab;

    [Header("Grid size")]
    public int rows = 3;
    public int columns = 3;

    private void Start()
    {
        GenerateGrid();
    }

    private void GenerateGrid()
    {
        if (craftingSlotPrefab == null)
        {
            Debug.LogError("CraftingSlot prefab is not assigned!");
            return;
        }

        foreach (Transform child in transform)
        {
            Destroy(child.gameObject);
        }

        GridLayoutGroup grid = GetComponent<GridLayoutGroup>();
        if (grid == null)
        {
            grid = gameObject.AddComponent<GridLayoutGroup>();
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;
            grid.cellSize = new Vector2(16, 16);
            grid.spacing = new Vector2(0, 0);
        }
        else
        {
            grid.constraint = GridLayoutGroup.Constraint.FixedColumnCount;
            grid.constraintCount = columns;
        }

        for (int row = 0; row < rows; row++)
        {
            for (int col = 0; col < columns; col++)
            {
                GameObject slot = Instantiate(craftingSlotPrefab, transform);
                slot.name = $"CraftingSlot_{row}_{col}";

                InventorySlot slotComponent = slot.GetComponent<InventorySlot>();
                if (slotComponent != null)
                {
                    slotComponent.row = row;
                    slotComponent.column = col;
                }
            }
        }

    }
}
