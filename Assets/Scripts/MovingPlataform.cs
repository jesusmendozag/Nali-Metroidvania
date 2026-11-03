using UnityEngine;

public class MovingPlataform : MonoBehaviour
{
    [SerializeField] Transform targetStart;
    [SerializeField] Transform targetEnd;
    [SerializeField] GameObject player;
    [SerializeField] float moveSpeed = 1f;

    // Update is called once per frame
    void Update()
    {
        movePlataform();
    }

    void movePlataform()
    {
        transform.position = Vector3.Lerp(targetStart.position, targetEnd.position, Mathf.PingPong(Time.time * moveSpeed, 1));
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            player.transform.parent = transform;
        }
    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == player)
        {
            player.transform.parent = null;
        }
    }
}
