using UnityEngine;

public class PickUp : MonoBehaviour
{
    [SerializeField] AudioClip coinPickUpSFX;
    [SerializeField] int pointsForCoinPickup = 5;

    private string PLAYER_PLAYER = "Player";

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.tag == PLAYER_PLAYER)
        {
            FindAnyObjectByType<Gamesession>().AddToScore(pointsForCoinPickup);

            AudioSource.PlayClipAtPoint(coinPickUpSFX, Camera.main.transform.position);
            Destroy(gameObject);
        }
    }
}