using System;
using UnityEngine;

[Serializable]
public class Brush
{
    [SerializeField] private Color color = Color.black;
    [SerializeField] private float size = 0.1f;

    public Color Color { get => color; }
    public float Size { get => size; }

    public Brush()
    {
        color = Color.black;
        size = 0.1f;
    }
    public Brush(Color color, float size)
    {
        this.color = color;
        this.size = size;
    }
    public void SetBrush(Color color, float size)
    {
        this.color = color;
        this.size = size;
    }

}