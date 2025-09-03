using UnityEngine;
using UnityEngine.UI;

public class HoldToCraft : MonoBehaviour
{
    [Header("Hold Settings")]
    public float holdDuration = 2f;          // Time in seconds to craft
    public Sprite[] progressFrames;          // 8-frame cursor animation
    public Vector2 cursorOffset = Vector2.zero;

    private float holdTimer = 0f;
    private bool isHolding = false;
    private int currentFrame = 0;

    private CraftingComponent currentComponent;
    private Image cursorImage;

    private void Start()
    {
        GameObject cursorGO = new GameObject("CursorProgress");
        cursorGO.transform.SetParent(transform);
        cursorGO.transform.localPosition = Vector3.zero;
        cursorImage = cursorGO.AddComponent<Image>();
        cursorImage.raycastTarget = false;  // Don't block clicks
        cursorImage.enabled = false;
    }

    private void LateUpdate()
    {
        if (isHolding)
        {
            holdTimer += Time.deltaTime;
            float frameTime = holdDuration / progressFrames.Length;
            currentFrame = Mathf.Min(progressFrames.Length - 1, (int)(holdTimer / frameTime));
            cursorImage.sprite = progressFrames[currentFrame];

            if (holdTimer >= holdDuration)
            {
                CompleteCraft();
            }

            cursorImage.transform.position = (Vector2)Input.mousePosition + cursorOffset;
        }
    }

    public void BeginHold(CraftingComponent comp)
    {
        currentComponent = comp;
        isHolding = true;
        holdTimer = 0f;
        currentFrame = 0;
        cursorImage.enabled = true;
    }

    public void EndHold()
    {
        isHolding = false;
        holdTimer = 0f;
        currentFrame = 0;
        cursorImage.enabled = false;
    }

    private void CompleteCraft()
    {
        Debug.Log($"Crafted {currentComponent.componentName}");
        EndHold();
        // TODO: consume resources
    }
}
