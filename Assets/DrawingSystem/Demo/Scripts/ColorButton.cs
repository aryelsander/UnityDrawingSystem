using UnityEngine;
using UnityEngine.UI;

public class ColorButton : MonoBehaviour
{
    [SerializeField] private Image colorImage;
    [SerializeField] private Button button;
    private Color color;
    public Color Color { get => color;}
    public Button Button { get => button;}

    public void SetColorButton(Color color)
    {
        this.color = color;
        colorImage.color = color;
    }
}

