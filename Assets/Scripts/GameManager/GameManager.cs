using UnityEngine;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    private ResourcesInventoryManager inventoryManager;
    [SerializeField] private Resource[] resource;
    private InputAction spaceAction;

    private void Awake()
    {
        inventoryManager = GetComponent<ResourcesInventoryManager>();

        spaceAction = new InputAction(type: InputActionType.Button, binding: "<Keyboard>/space");
        spaceAction.performed += OnSpace;
    }

    private void OnEnable()
    {
        spaceAction.Enable();
    }

    private void OnDisable()
    {
        spaceAction.Disable();
    }

    private void Start()
    {
        if (resource.Length > 0 && resource[0] != null)
        {
            inventoryManager.AddResource(resource[0], 10);
        }
    }

    private void OnSpace(InputAction.CallbackContext context)
    {
        if (resource.Length > 0 && resource[0] != null)
        {
            inventoryManager.AddResource(resource[0], 10);
            Debug.Log($"Added 10 {resource[0].resourceName}");
        }
    }
}
