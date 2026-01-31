using UnityEngine;
using UnityEngine.EventSystems;

public class DraggableFile : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private CanvasGroup canvasGroup;
    private RectTransform rectTransform;
    private Vector2 startPosition;
    public string fileCategory;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();

        if (canvasGroup == null)
            Debug.LogError($"CRITICAL: {gameObject.name} is missing a CanvasGroup component!");
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    Debug.Log($"---> START DRAG: {gameObject.name}");
    //    startPosition = rectTransform.anchoredPosition;
    //    if (canvasGroup != null)
    //    {
    //        canvasGroup.alpha = 0.6f;
    //        canvasGroup.blocksRaycasts = false; // The Folder needs to see through this!
    //    }
    //}

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta;
    }

    public bool wasSuccessfullyDropped = false; 
    public void OnBeginDrag(PointerEventData eventData)
    {
        wasSuccessfullyDropped = false;
        startPosition = rectTransform.anchoredPosition;

        if (canvasGroup != null)
        {
            canvasGroup.alpha = 0.5f;
            canvasGroup.blocksRaycasts = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (canvasGroup != null)
        {
            canvasGroup.alpha = 1f;
            canvasGroup.blocksRaycasts = true; 
        }

        if (!wasSuccessfullyDropped)
        {
            rectTransform.anchoredPosition = startPosition;
        }
    }

    //public void OnBeginDrag(PointerEventData eventData)
    //{
    //    wasSuccessfullyDropped = false; // Reset at start
    //    startPosition = rectTransform.anchoredPosition;

    //    if (canvasGroup != null)
    //    {
    //        canvasGroup.alpha = 0.5f; // Should be visibly transparent now
    //        canvasGroup.blocksRaycasts = false; // ESSENTIAL for the Folder to see the drop
    //    }
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    if (canvasGroup != null)
    //    {
    //        canvasGroup.alpha = 1f;
    //        canvasGroup.blocksRaycasts = true;
    //    }

    //    // ONLY snap back if the Folder didn't catch the drop
    //    if (!wasSuccessfullyDropped)
    //    {
    //        rectTransform.anchoredPosition = startPosition;
    //    }
    //    Debug.Log("I dropped on: " + eventData.pointerCurrentRaycast.gameObject.name);
    //}

    //public void OnEndDrag(PointerEventData eventData)
    //{
    //    Debug.Log($"<--- END DRAG: {gameObject.name}");
    //    if (canvasGroup != null)
    //    {
    //        canvasGroup.alpha = 1f;
    //        canvasGroup.blocksRaycasts = true;
    //    }

    //    // Logic: If it didn't find a folder, it stays parented to the panel and snaps back
    //    rectTransform.anchoredPosition = startPosition;
    //}
}