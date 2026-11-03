using UnityEngine;

public class ParedDestruible : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        // 1. Verificamos si es el Player
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement movementPlayer = other.gameObject.GetComponent<PlayerMovement>();

            // 2. Verificamos la habilidad
            if (movementPlayer != null && movementPlayer.abilityOcelote)
            {
                Destroy(gameObject);
            }
        }
    }
}

/* DESTRUYE LA PARED DE UNO EN UNO AL TOCARLA CON LA HABILIDAD ACTIVADA
public class ParedDestruible : MonoBehaviour
{
    private Tilemap miTilemap;

    private void Start()
    {
        miTilemap = GetComponent<Tilemap>();
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            PlayerMovement movementPlayer = other.gameObject.GetComponent<PlayerMovement>();

            if (movementPlayer != null && movementPlayer.abilityOcelote)
            {
                // Obtenemos el primer punto de contacto
                Vector3 hitPoint = other.GetContact(0).point;
                
                // Convertimos ese punto a la celda más cercana en la grilla
                Vector3Int celdaImpacto = miTilemap.WorldToCell(hitPoint);

                // --- EL MÉTODO MARTILLO ---
                // Revisamos la celda del impacto y sus 4 vecinas.
                // La primera que tenga un bloque, ¡se va!
                if (IntentarDestruir(celdaImpacto)) return;
                if (IntentarDestruir(celdaImpacto + Vector3Int.up)) return;
                if (IntentarDestruir(celdaImpacto + Vector3Int.down)) return;
                if (IntentarDestruir(celdaImpacto + Vector3Int.left)) return;
                if (IntentarDestruir(celdaImpacto + Vector3Int.right)) return;
                
                // Si llegamos aquí y nada se rompió, intentamos empujando hacia adentro
                // (Un último intento usando la normal, por si acaso)
                Vector2 normal = other.GetContact(0).normal;
                Vector3Int celdaAdentro = miTilemap.WorldToCell(hitPoint - (Vector3)(normal * 0.5f));
                IntentarDestruir(celdaAdentro);
            }
        }
    }

    // Función auxiliar para no repetir código
    bool IntentarDestruir(Vector3Int coordenada)
    {
        // Preguntamos: ¿Hay un tile en esta coordenada?
        if (miTilemap.GetTile(coordenada) != null)
        {
            // ¡Sí hay! Lo destruimos.
            miTilemap.SetTile(coordenada, null);
            Debug.Log("¡Pared destruida en: " + coordenada + "!");
            return true; // Éxito, dejamos de buscar
        }
        return false; // Aquí no había nada, seguimos buscando
    }
}
*/