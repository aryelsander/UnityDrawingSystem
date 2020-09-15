using System;
using System.Collections.Generic;
using UnityEngine;

public class DrawingSystem : MonoBehaviour
{
    [SerializeField] private TrailRenderer drawPrefab = null;
    [SerializeField] private Brush currentBrush = null;

    private Stack<TrailRenderer> drawedLines = new Stack<TrailRenderer>();
    private Stack<TrailRenderer> redoDrawedLines = new Stack<TrailRenderer>();
    private CanvasPaint canvasPaint = null;
    public Func<TrailRenderer> OnStartPaint;
    public Action<Vector2, TrailRenderer> OnPainting;
    public Action<TrailRenderer> OnEndPaint;
    public Action<TrailRenderer> OnUndoDraw;
    public Action<TrailRenderer> OnRedoDraw;
    public Action OnComplete;
    public Brush CurrentBrush { get => currentBrush; }
    public int DrawedLinesCount { get => drawedLines.Count; }
    public int RedoDrawedLinesCount { get => redoDrawedLines.Count; }
    public bool CanUndo { get => drawedLines.Count > 0; }
    public bool CanRedo { get => redoDrawedLines.Count > 0; }

    public void UndoDraw()
    {
        if (CanUndo)
        {
            TrailRenderer lastDrawed = drawedLines.Pop();
            lastDrawed.gameObject.SetActive(false);
            redoDrawedLines.Push(lastDrawed);
            OnUndoDraw?.Invoke(lastDrawed);
        }
    }
    public void RedoDraw()
    {
        if (CanRedo)
        {
            TrailRenderer lastUndrawed = redoDrawedLines.Pop();
            lastUndrawed.gameObject.SetActive(true);
            drawedLines.Push(lastUndrawed);
            OnRedoDraw?.Invoke(lastUndrawed);
        }
    }
    public void Clear()
    {
        ClearRedo();
        ClearUndo();
        OnComplete();
    }

    private void ClearUndo()
    {
        foreach (TrailRenderer trailRenderer in drawedLines)
        {
            Destroy(trailRenderer.gameObject);
        }

        drawedLines.Clear();
    }

    protected virtual void Awake()
    {
        canvasPaint = GetComponentInChildren<CanvasPaint>();
    }
    private TrailRenderer StartPaint()
    {
        TrailRenderer trailInstance = Instantiate(drawPrefab, transform.position, Quaternion.identity);
        trailInstance.transform.SetParent(canvasPaint.transform);
        SetBrushToTrail(trailInstance);
        drawedLines.Push(trailInstance);
        return trailInstance;
    }
    private void Painting(Vector2 position, TrailRenderer trailRenderer)
    {
        trailRenderer.transform.position = position;
    }
    private void EndPaint(TrailRenderer trailRenderer)
    {
        ClearRedo();
        
    }

    private void ClearRedo()
    {
        foreach (TrailRenderer trailRenderer in redoDrawedLines)
        {
            Destroy(trailRenderer.gameObject);
        }

        redoDrawedLines.Clear();
    }

    private void SetBrushToTrail(TrailRenderer trailInstance)
    {
        trailInstance.startWidth = currentBrush.Size;
        trailInstance.endWidth = currentBrush.Size;
        trailInstance.startColor = currentBrush.Color;
        trailInstance.endColor = currentBrush.Color;
    }

    protected virtual void OnEnable()
    {
        OnStartPaint += StartPaint;
        OnPainting += Painting;
        OnEndPaint += EndPaint;
    }
    protected virtual void OnDisable()
    {
        OnStartPaint -= StartPaint;
        OnPainting -= Painting;
        OnEndPaint -= EndPaint;
    }
}
