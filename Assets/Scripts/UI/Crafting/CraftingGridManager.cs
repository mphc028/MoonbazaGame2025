using System.Collections.Generic;
using UnityEngine;

public class CraftingGridManager : MonoBehaviour
{
    private InventorySlot[,] grid = new InventorySlot[3, 3];
    private CraftingComponent[,] currentComponents = new CraftingComponent[3, 3];

    private void Awake()
    {
        var slots = GetComponentsInChildren<InventorySlot>();
        foreach (var slot in slots)
        {
            grid[slot.row, slot.column] = slot;
        }
    }

    public void SetComponentAt(int row, int col, CraftingComponent comp)
    {
        currentComponents[row, col] = comp;
    }

    public CraftingComponent[,] GetCurrentComponents()
    {
        return currentComponents;
    }

    public void RecalculateOutputs()
    {
        int totalElectricity = 0;
        int totalHeat = 0;
        Dictionary<string, int> resourceTotals = new Dictionary<string, int>();

        Debug.Log("=== Current Grid State ===");
        for (int r = 0; r < 3; r++)
        {
            for (int c = 0; c < 3; c++)
            {
                var comp = currentComponents[r, c];
                Debug.Log($"Slot [{r},{c}] = {(comp != null ? comp.componentName : "empty")}");

                if (comp == null) continue;

                foreach (var output in comp.outputs)
                {
                    switch (output.outputType)
                    {
                        case OutputType.Resource:
                            if (!resourceTotals.ContainsKey(output.resource.name))
                                resourceTotals[output.resource.name] = 0;
                            resourceTotals[output.resource.name] += output.amount;
                            break;

                        case OutputType.Electricity:
                            totalElectricity += output.amount;
                            break;

                        case OutputType.Heat:
                            totalHeat += output.amount;
                            break;

                        case OutputType.CustomEffect:
                            Debug.Log($"Custom effect: {output.description}");
                            break;
                    }
                }
            }
        }

        Debug.Log("=== Crafting Grid Totals ===");
        foreach (var kvp in resourceTotals)
            Debug.Log($"{kvp.Key}: {kvp.Value}");
        Debug.Log($"Electricity: {totalElectricity}");
        Debug.Log($"Heat: {totalHeat}");
    }
}
