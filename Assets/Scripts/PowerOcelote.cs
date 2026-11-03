using UnityEngine;

public class PowerOcelote : MonoBehaviour
{

    [SerializeField] AudioClip powerOceloteSound;

    private string PLAYER_TAG = "Player";

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == PLAYER_TAG)
        {
            AudioSource.PlayClipAtPoint(powerOceloteSound, Camera.main.transform.position);

            // Busca el script PlayerMovement del jugador
            PlayerMovement player = collision.GetComponent<PlayerMovement>();
            // Si lo encuentra, activa la habilidad Ocelote
            if (player != null)
            {
                player.abilityOcelote = true;
            }

            Destroy(gameObject);
        }
    }
}