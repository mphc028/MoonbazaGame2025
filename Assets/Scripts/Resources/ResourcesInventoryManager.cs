using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;

public class ResourcesInventoryManager : MonoBehaviour
{
    [Header("UI Setup")]
    [SerializeField] private Transform resourceGridParent;
    [SerializeField] private GameObject resourceUIPrefab;

    private Dictionary<Resource, int> resourceInventory = new Dictionary<Resource, int>();
    private Dictionary<Resource, GameObject> resourceUIObjects = new Dictionary<Resource, GameObject>();
    public static ResourcesInventoryManager Instance { get; private set; }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
    }

    public void AddResource(Resource resource, int amount)
    {
        if (resource == null || amount <= 0) return;

        if (resourceInventory.ContainsKey(resource))
        {
            resourceInventory[resource] += amount;
            UpdateResourceUI(resource);
            PlayPopEffect(resourceUIObjects[resource].transform);
        }
        else
        {
            resourceInventory.Add(resource, amount);
            CreateResourceUI(resource);
            PlayPopEffect(resourceUIObjects[resource].transform);
        }
    }

    public bool RemoveResource(Resource resource, int amount)
    {
        if (resource == null || amount <= 0) return false;

        if (resourceInventory.ContainsKey(resource))
        {
            if (resourceInventory[resource] >= amount)
            {
                resourceInventory[resource] -= amount;
                UpdateResourceUI(resource);
                PlayPopEffect(resourceUIObjects[resource].transform);

                if (resourceInventory[resource] <= 0)
                {
                    PlayPopEffect(resourceUIObjects[resource].transform);
                    Destroy(resourceUIObjects[resource]);
                    resourceUIObjects.Remove(resource);
                    resourceInventory.Remove(resource);
                }
                return true;
            }
        }
        return false;
    }

    private void CreateResourceUI(Resource resource)
    {
        GameObject uiElement = Instantiate(resourceUIPrefab, resourceGridParent);

        TMP_Text quantityText = uiElement.transform.GetChild(0).GetComponent<TMP_Text>();
        Image icon = uiElement.transform.GetChild(1).GetComponent<Image>();

        icon.sprite = resource.icon;
        quantityText.text = resourceInventory[resource].ToString();

        resourceUIObjects.Add(resource, uiElement);
    }

    private void UpdateResourceUI(Resource resource)
    {
        if (!resourceUIObjects.ContainsKey(resource)) return;

        GameObject uiElement = resourceUIObjects[resource];
        TMP_Text quantityText = uiElement.transform.GetChild(0).GetComponent<TMP_Text>();
        quantityText.text = resourceInventory[resource].ToString();
    }

    private void PlayPopEffect(Transform target)
    {
        target.DOKill();
        target.localScale = Vector3.one;
        target.DOShakeScale(0.25f, strength: 0.6f, vibrato: 20, randomness: 0, fadeOut: true);
    }

    public int GetResourceAmount(Resource resource)
    {
        if (resource == null) return 0;
        return resourceInventory.ContainsKey(resource) ? resourceInventory[resource] : 0;
    }
}
