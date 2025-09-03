using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    private PlayerController playerController;
    private CameraController cameraController;
    private RotableProp playerRotationController;

    #region Weapon Switching (Tab Key)
    [Header("Weapon Settings")]
    public Sprite[] weaponIcons;
    public int currentWeaponIndex;

    [Header("UI References")]
    public Image hotBarSlot;

    private InputAction tabAction, spaceAction, scrollAction, inventoryAction;

    private void Awake()
    {
        tabAction = new InputAction(binding: "<Keyboard>/tab");
        spaceAction = new InputAction(binding: "<Keyboard>/space");
        scrollAction = new InputAction(type: InputActionType.Value, binding: "<Mouse>/scroll");
        inventoryAction = new InputAction(binding: "<Keyboard>/e");
    }


    private void OnEnable()
    {
        tabAction.performed += OnTabPerformed;
        tabAction.Enable();

        spaceAction.performed += OnSpacePerformed;
        spaceAction.Enable();

        scrollAction.performed += OnScrollHotbar;
        scrollAction.Enable();

        inventoryAction.performed += OnInventoryPerformed;
        inventoryAction.Enable();
    }

    private void OnDisable()
    {
        tabAction.performed -= OnTabPerformed;
        tabAction.Disable();

        spaceAction.performed -= OnSpacePerformed;
        spaceAction.Disable();

        scrollAction.performed -= OnScrollHotbar;
        scrollAction.Disable();

        inventoryAction.performed -= OnInventoryPerformed;
        inventoryAction.Disable();
    }

    private void OnTabPerformed(InputAction.CallbackContext ctx)
    {
        CycleWeapon(1);
    }

    private void OnSpacePerformed(InputAction.CallbackContext ctx)
    {
        ToggleSlotDetail(selectedHotbarIndex);
    }

    private void CycleWeapon(int direction)
    {
        currentWeaponIndex = (currentWeaponIndex + direction + weaponIcons.Length) % weaponIcons.Length;
        ChangeWeapon(currentWeaponIndex);
    }

    public void ChangeWeapon(int index)
    {
        if (weaponIcons.Length == 0 || index < 0 || index >= weaponIcons.Length)
        {
            Debug.LogWarning("Invalid weapon index!");
            return;
        }

        currentWeaponIndex = index;
        UpdateHotbarIcon();
    }

    public void UpdateHotbarIcon()
    {
        if (hotBarSlot != null && weaponIcons.Length > 0)
            hotBarSlot.sprite = weaponIcons[currentWeaponIndex];
    }
    #endregion

    #region Secondary Hotbar (Scroll Wheel Selection)
    [Header("Secondary Hotbar Settings")]
    public GameObject hotbarSlotPrefab;
    public Transform hotbarContainer;
    public int slotCount = 8;

    [Header("Secondary Hotbar UI")]
    public Sprite defaultSlotSprite;
    public Sprite selectedSlotSprite;

    private Image[] hotbarSlots;
    private int selectedHotbarIndex;
    private bool[] slotDetailStates;

    private void Start()
    {
        hotbarSlots = new Image[slotCount];
        slotDetailStates = new bool[slotCount];
        for (int i = 0; i < slotCount; i++)
        {
            GameObject slot = Instantiate(hotbarSlotPrefab, hotbarContainer);
            hotbarSlots[i] = slot.GetComponent<Image>();
            hotbarSlots[i].sprite = defaultSlotSprite;
            slotDetailStates[i] = false;
        }
        UpdateSelectedHotbar(0);
        playerController = GameObject.FindWithTag("Player").GetComponentInChildren<PlayerController>();
        playerRotationController = GameObject.FindWithTag("Player").GetComponent<RotableProp>();
        cameraController = GameObject.FindWithTag("MainCamera").GetComponentInChildren<CameraController>();
    }

    private void OnScrollHotbar(InputAction.CallbackContext ctx)
    {
        Vector2 scroll = ctx.ReadValue<Vector2>();
        if (scroll.y > 0.1f)
            ChangeHotbarSelection(-1);
        else if (scroll.y < -0.1f)
            ChangeHotbarSelection(1);
    }

    private void ChangeHotbarSelection(int direction)
    {
        selectedHotbarIndex = (selectedHotbarIndex + direction + slotCount) % slotCount;
        ResetSlotDetail(selectedHotbarIndex);
        UpdateSelectedHotbar(selectedHotbarIndex);
    }

    private void UpdateSelectedHotbar(int newIndex)
    {
        for (int i = 0; i < hotbarSlots.Length; i++)
        {
            hotbarSlots[i].sprite = (i == newIndex) ? selectedSlotSprite : defaultSlotSprite;
            hotbarSlots[i].transform.GetComponent<Animator>().SetBool("selected", i == newIndex);
            hotbarSlots[i].transform.GetComponent<Animator>().SetBool("detail", slotDetailStates[i]);
        }
    }

    private void ToggleSlotDetail(int index)
    {
        slotDetailStates[index] = !slotDetailStates[index];
        hotbarSlots[index].transform.GetComponent<Animator>().SetBool("detail", slotDetailStates[index]);
    }

    private void ResetSlotDetail(int index)
    {
        slotDetailStates[index] = false;
        hotbarSlots[index].transform.GetComponent<Animator>().SetBool("detail", false);
    }
    #endregion

    #region Inventory (E Key)
    [Header("Inventory UI")]
    public Animator inventoryAnimator;

    private bool isInventoryOpen = false;

    private void OnInventoryPerformed(InputAction.CallbackContext ctx)
    {
        isInventoryOpen = !isInventoryOpen;

        playerController.SetCursor(isInventoryOpen ? 2 : 1);
        cameraController.SetCameraLock(isInventoryOpen);
        playerRotationController.SetIdleRotate(isInventoryOpen);


        inventoryAnimator.SetBool("open", isInventoryOpen);
    }
    #endregion
}
