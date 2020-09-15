using UnityEngine;
using UnityEngine.UI;

public class BrushSizeButton : MonoBehaviour
{
    [SerializeField] private Text brushSizeText;
    [SerializeField] private Button button;
    private float brushSize;

    public Button Button { get => button;}
    public float BrushSize { get => brushSize; }

    public void SetBrushSize(float brushSize)
    {
        this.brushSizeText.text = brushSize.ToString();
        this.brushSize = brushSize;
    }

}