using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine.EventSystems;

public class CraftingManager : MonoBehaviour
{
    [Header("Categories Setup")]
    public CraftingCategory[] allCategories;
    public int[] enabled_categories;

    [Header("UI Setup")]
    public GameObject categoryButtonPrefab;
    public GameObject componentButtonPrefab;
    public Transform categoryParent;
    public Transform componentGridParent;

    [Header("Data Setup")]
    public CraftingComponent[] allComponents;

    private Dictionary<int, Button> categoryButtons = new Dictionary<int, Button>();
    private int currentCategoryId = -1;

    [SerializeField]
    private Sprite category, categorySelected;

    [Header("Crafted Components Setup")]
    public GameObject craftedComponentPrefab;
    public Transform componentListParent;
    private List<CraftingComponent> craftedComponents = new List<CraftingComponent>();

    private void Start()
    {
        GenerateCategoriesUI();

        if (enabled_categories.Length > 0)
        {
            ShowCategory(enabled_categories[0]);
        }
    }

    private void GenerateCategoriesUI()
    {
        foreach (int id in enabled_categories)
        {
            CraftingCategory category = allCategories.FirstOrDefault(c => c.id == id);
            if (category == null) continue;

            GameObject newCategoryGO = Instantiate(categoryButtonPrefab, categoryParent);

            Button btn = newCategoryGO.transform.GetChild(0).GetComponent<Button>();
            if (btn != null)
            {
                int capturedId = id;
                btn.onClick.AddListener(() => ShowCategory(capturedId));
                categoryButtons.Add(id, btn);
            }

            Image icon = newCategoryGO.transform.GetChild(0).GetChild(0).GetComponent<Image>();
            if (icon != null)
                icon.sprite = category.icon;

            newCategoryGO.name = $"Category_{category.categoryName}";
        }
    }

    public void ShowCategory(int categoryId)
    {
        if (currentCategoryId == categoryId) return;

        foreach (Transform child in componentGridParent)
            Destroy(child.gameObject);

        List<CraftingComponent> components = allComponents
            .Where(c => c.category != null && c.category.id == categoryId)
            .ToList();

        foreach (CraftingComponent comp in components)
        {
            GameObject compGO = Instantiate(componentButtonPrefab, componentGridParent);

            Button btn = compGO.GetComponentInChildren<Button>();
            if (btn != null)
            {
                CraftingComponent capturedComp = comp;
                btn.onClick.AddListener(() => Craft(capturedComp));

                EventTrigger trigger = btn.gameObject.AddComponent<EventTrigger>();
            }

            compGO.name = $"Component_{comp.componentName}";
            compGO.transform.GetChild(0).GetChild(0).GetComponentInChildren<Image>().sprite = comp.icon;
        }

        UpdateCategorySelection(categoryId);
    }


    private void UpdateCategorySelection(int selectedId)
    {
        currentCategoryId = selectedId;

        foreach (var kvp in categoryButtons)
        {
            Image buttonImage = kvp.Value.GetComponent<Image>();
            if (buttonImage != null)
            {
                buttonImage.sprite = (kvp.Key == currentCategoryId) ? categorySelected : category;
            }
        }
    }



    private void Craft(CraftingComponent comp)
    {
        foreach (var req in comp.craftingCost)
        {
            if (ResourcesInventoryManager.Instance.GetResourceAmount(req.resource) < req.amount)
            {
                Debug.LogWarning($"Not enough {req.resource.name} to craft {comp.componentName}");
                return;
            }
        }

        foreach (var req in comp.craftingCost)
        {
            ResourcesInventoryManager.Instance.RemoveResource(req.resource, req.amount);
        }

        craftedComponents.Add(comp);

        GameObject craftedGO = Instantiate(craftedComponentPrefab, componentListParent);
        craftedGO.name = $"Crafted_{comp.componentName}";

        Image icon = craftedGO.transform.GetChild(1).GetComponentInChildren<Image>();
        if (icon != null) icon.sprite = comp.icon;

        Text txt = craftedGO.transform.GetChild(0).GetComponentInChildren<Text>();
        if (txt != null) txt.text = comp.componentName;

        DraggableItem draggable = craftedGO.transform.GetChild(1).GetComponent<DraggableItem>();
        if (draggable != null)
        {
            draggable.Initialize(comp);
        }


        PlayPopEffect(craftedGO.transform);


        Debug.Log($"Crafted {comp.componentName}");
    }

    private void PlayPopEffect(Transform target)
    {
        target.DOKill();
        target.localScale = Vector3.one;
        target.DOShakeScale(0.25f, strength: 0.6f, vibrato: 20, randomness: 0, fadeOut: true);
    }
}
