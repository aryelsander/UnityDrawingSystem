using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DrawingExample : MonoBehaviour
{
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private List<float> brushSizes = new List<float>();
    [SerializeField] private ColorButton colorButton;
    [SerializeField] private BrushSizeButton brushSizeButton;
    [SerializeField] private Transform colorsContainer;
    [SerializeField] private Transform brushSizeContainer;
    [SerializeField] private Button undo = null;
    [SerializeField] private Button redo = null;
    [SerializeField] private Button clearButton = null;
    private DrawingSystem drawingSystem;

    private void Awake()
    {
        drawingSystem = GetComponent<DrawingSystem>();
        undo.onClick.AddListener(() => UndoButton());
        redo.onClick.AddListener(() => RedoButton());
        clearButton.onClick.AddListener(() => drawingSystem.Clear());
        foreach (Color color in colors)
        {
            ColorButton colorButtonInstance = Instantiate(colorButton, colorsContainer);
            colorButtonInstance.SetColorButton(color);
            colorButtonInstance.Button.onClick.AddListener(() => drawingSystem.CurrentBrush.SetBrush(colorButtonInstance.Color,drawingSystem.CurrentBrush.Size));
        }
        foreach (float brushSize in brushSizes)
        {
            BrushSizeButton brushSizeButtonInstance = Instantiate(brushSizeButton, brushSizeContainer);
            brushSizeButtonInstance.SetBrushSize(brushSize);
            brushSizeButtonInstance.Button.onClick.AddListener(() => drawingSystem.CurrentBrush.SetBrush(drawingSystem.CurrentBrush.Color, brushSizeButtonInstance.BrushSize));
        }
    }
    public void UndoButton()
    {
        drawingSystem.UndoDraw();
        CheckButtons();
    }
    public void RedoButton()
    {
        drawingSystem.RedoDraw();
        CheckButtons();
    }
    public void CheckButtons()
    {
        undo.interactable = drawingSystem.CanUndo;
        redo.interactable = drawingSystem.CanRedo;
    }

    private void OnEnable()
    {
        drawingSystem.OnComplete += CheckButtons;
    }

    private void OnDisable()
    {
        drawingSystem.OnComplete -= CheckButtons;
    }
}
