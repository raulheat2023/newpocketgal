using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class MenuButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;         // Ícono que aparecerá al pasar el mouse
    public Text buttonText;    // Texto del botón
    public Color hoverColor = Color.red;  // Color del texto al pasar el mouse
    private Color originalColor;          // Color original del texto

    void Start()
    {
        // Guardar el color original del texto
        if (buttonText != null)
        {
            originalColor = buttonText.color;
        }

        // Asegurarse de que el ícono esté desactivado al inicio
        if (icon != null)
        {
            icon.gameObject.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        // Activar el ícono y cambiar el color del texto
        if (icon != null)
        {
            icon.gameObject.SetActive(true);
        }
        if (buttonText != null)
        {
            buttonText.color = hoverColor;
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Desactivar el ícono y restaurar el color del texto
        if (icon != null)
        {
            icon.gameObject.SetActive(false);
        }
        if (buttonText != null)
        {
            buttonText.color = originalColor;
        }
    }
}
