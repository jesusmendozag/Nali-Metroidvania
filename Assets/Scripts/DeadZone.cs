using UnityEngine;

public class ZonaMuerte : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Verificamos si el objeto que tocó esta zona tiene la etiqueta "Player"
        if (collision.CompareTag("Player"))
        {
            // Buscamos el script PlayerMovement en ese objeto
            PlayerMovement jugador = collision.GetComponent<PlayerMovement>();
            
            // Si encontramos el script, ejecutamos su función Die()
            if (jugador != null)
            {
                jugador.Die();
            }
        }
    }
}