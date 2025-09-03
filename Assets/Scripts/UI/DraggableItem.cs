using UnityEngine;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections;

public class DraggableItem : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public CraftingComponent craftingComponent;
    

    private Transform originalParent;
    private Vector3 originalPosition;
    private CanvasGroup canvasGroup;
    private Canvas canvas;

    private bool isLocked = false;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = gameObject.AddComponent<CanvasGroup>();
    }



    public void Initialize(CraftingComponent comp)
    {
        craftingComponent = comp;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (isLocked) return;

        originalParent = transform.parent;
        originalPosition = transform.position;

        canvasGroup.blocksRaycasts = false;
        transform.SetParent(canvas.transform);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (isLocked) return;
        canvasGroup.blocksRaycasts = true;

        if (eventData.pointerEnter != null)
        {
            InventorySlot slot = eventData.pointerEnter.GetComponent<InventorySlot>();
            if (slot != null && slot.IsEmpty())
            {
                transform.SetParent(slot.transform);
                transform.position = slot.transform.position;
                slot.SetItem(this);
                isLocked = true;
                canvasGroup.blocksRaycasts = false;

                PlayConsumeAnimation();
                return;
            }
        }

        StartCoroutine(SmoothReturn());
    }

    private void PlayConsumeAnimation()
    {
        if (originalParent != null)
        {
            originalParent
                .DOScaleY(0f, 0.1f)
                .SetEase(Ease.InBack)
                .OnComplete(() =>
                {
                    Destroy(originalParent.gameObject);
                });
        }
    }

    private IEnumerator SmoothReturn()
    {
        float time = 0f;
        float duration = 0.25f;
        Vector3 startPos = transform.position;

        while (time < duration)
        {
            time += Time.deltaTime;
            float t = Mathf.Sin((time / duration) * Mathf.PI * 0.5f);
            transform.position = Vector3.Lerp(startPos, originalPosition, t);
            yield return null;
        }

        transform.position = originalPosition;
        transform.SetParent(originalParent);
    }
}
