using UnityEngine;
using UnityEngine.InputSystem;

public class Texture : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private SpriteRenderer Renderer;
    private Color colourofpixels;
    private Texture2D OriginalTexture;

    void Start()
    {
        OriginalTexture = Renderer.sprite.texture;
        Vector3 MousePos = Camera.main.ScreenToWorldPoint(Mouse.current.position.value);
        colourofpixels = OriginalTexture.GetPixel((int)MousePos.x, (int)MousePos.y);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
