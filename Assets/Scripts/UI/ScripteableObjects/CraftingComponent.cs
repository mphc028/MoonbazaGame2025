using System;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "CraftingComponent", menuName = "Scriptable Objects/CraftingComponent")]
public class CraftingComponent : ScriptableObject
{
    [Header("Basic Info")]
    public int id;
    public string componentName;
    public Sprite icon;
    public CraftingCategory category;

    [Header("Crafting Cost")]
    public List<CraftingRequirement> craftingCost = new List<CraftingRequirement>();

    [Header("Outputs / Effects")]
    public List<CraftingOutput> outputs = new List<CraftingOutput>();
}

[Serializable]
public class CraftingRequirement
{
    public Resource resource;
    public int amount;
}

[Serializable]
public class CraftingOutput
{
    public OutputType outputType;
    public Resource resource; 
    public int amount;

    public string description; 
}

public enum OutputType
{
    Resource,     // e.g. "Iron +2"
    Electricity,  // e.g. "+10 power"
    Heat,         // maybe more later
    CustomEffect  // fallback for special cases
}
