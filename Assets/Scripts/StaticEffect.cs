using UnityEngine;

public class StaticEffect : MonoBehaviour
{
    public Transform ball; 
    private Vector3 initialOffset; 

    void Start()
    {
        if (transform.parent == null)
        {
            Debug.LogError("⚠️ Error: EfectoVisual no tiene un objeto padre asignado.");
            return;
        }

        ball = transform.parent;  // Asigna la bola como padre
        initialOffset = transform.position - ball.position; // Guarda la distancia inicial
    }

    void LateUpdate()  // 💡 Se ejecuta después de la física
    {
        transform.rotation = Quaternion.identity; // Mantiene la rotación fija

    }
}
