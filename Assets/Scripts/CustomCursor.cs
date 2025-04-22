using UnityEngine;

public class CustomCursor : MonoBehaviour
{
    public Texture2D cursorDefault;
    public Texture2D cursorClick;
    public Vector2 hotspot = Vector2.zero;

    void Start()
    {
        Cursor.SetCursor(cursorDefault, hotspot, CursorMode.Auto);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Cuando se presiona clic
        {
            Cursor.SetCursor(cursorClick, hotspot, CursorMode.Auto);
        }
        else if (Input.GetMouseButtonUp(0)) // Cuando se suelta el clic
        {
            Cursor.SetCursor(cursorDefault, hotspot, CursorMode.Auto);
        }
    }
}
