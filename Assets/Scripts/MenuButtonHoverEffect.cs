using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class MenuButtonHoverEffect : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public Image icon;         // Ícono que aparecerá al pasar el mouse
    public TextMeshProUGUI buttonText;    // Texto del botón
    public Color hoverColor = Color.red;  // Color del texto al pasar el mouse
    private Color originalColor;        // Color original del texto
    public AudioSource hoverMenuSource;
    public AudioClip hoverSfx;

    void Start()
    {
        hoverMenuSource = GetComponent<AudioSource>();
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
        hoverMenuSource.PlayOneShot(hoverSfx);
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
