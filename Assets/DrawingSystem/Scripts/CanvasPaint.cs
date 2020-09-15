using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

[RequireComponent(typeof(Image))]
public class CanvasPaint : MonoBehaviour, IDragHandler, IEndDragHandler, IPointerDownHandler, IPointerUpHandler
{
    private Image canvasPaintImage = null;
    private TrailRenderer currentTrail = null;
    private DrawingSystem drawingSystem;
    private bool wasDragged = false;
    private bool canDrag = true;
    private void Awake()
    {
        canvasPaintImage = GetComponent<Image>();
        drawingSystem = GetComponentInParent<DrawingSystem>();
    }

    private void OnBeginDrag(PointerEventData eventData)
    {
        wasDragged = true;
    }
    public void OnDrag(PointerEventData eventData)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(eventData.position);

        CanvasPaint canvasPaint = eventData.pointerCurrentRaycast.gameObject?.GetComponent<CanvasPaint>();

        if (canvasPaint && canDrag)
        {
            drawingSystem.OnPainting?.Invoke(worldPosition, currentTrail);
        }
        else if(canvasPaint && !canDrag)
        {
            drawingSystem.OnEndPaint?.Invoke(currentTrail);
            drawingSystem.OnComplete?.Invoke();
            canDrag = true; 
            currentTrail = drawingSystem.OnStartPaint?.Invoke();
            currentTrail.transform.position = worldPosition;
        }
        else
        {
            canDrag = false;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canDrag = true;
        if (!wasDragged)
            return;

        wasDragged = false;
        drawingSystem.OnEndPaint?.Invoke(currentTrail);
        drawingSystem.OnComplete?.Invoke();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        Vector2 worldPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        CanvasPaint canvasPaint = eventData.pointerCurrentRaycast.gameObject?.GetComponent<CanvasPaint>();
        
        if (canvasPaint)
        {
            currentTrail = drawingSystem.OnStartPaint?.Invoke();
            currentTrail.transform.position = worldPosition;
        }
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        canDrag = true;
        if (wasDragged)
            return;

        wasDragged = false;
        drawingSystem.OnEndPaint?.Invoke(currentTrail);
        drawingSystem.OnComplete?.Invoke();
    }
}

