using UnityEngine;
using UnityEngine.SceneManagement; // ¡Importante! Necesario para cambiar de escena

public class ZonaSalida : MonoBehaviour
{
    // Escribe aquí el nombre EXACTO de tu escena del menú
    public string nombreEscenaMenu = "Menu"; 

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si lo que tocó la caja es el Jugador (para que no se active con enemigos)
        if (collision.CompareTag("Player"))
        {
            Debug.Log("¡Tocado! Regresando al menú...");
            SceneManager.LoadScene(nombreEscenaMenu);
        }
    }
}
