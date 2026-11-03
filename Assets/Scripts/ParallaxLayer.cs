using UnityEngine;

[DefaultExecutionOrder(100)]
public class ParallaxLayer : MonoBehaviour
{
    [Tooltip("0 = se mueve con el mundo (cerca). 1 = pegado a la cámara (muy lejano)")]
    [Range(0f, 1f)] [SerializeField] float parallaxX = 0.5f;
    [Range(0f, 1f)] [SerializeField] float parallaxY = 1f;
    [SerializeField] bool repeatHorizontal = true;

    Transform cam;
    Vector3 anchor;
    float tileWidth;

    void Start()
    {
        cam = Camera.main.transform;
        anchor = transform.position;

        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        tileWidth = sr.sprite.bounds.size.x * transform.lossyScale.x;
    }

    void LateUpdate()
    {
        Vector3 cp = cam.position;

        // La capa sigue a la cámara solo una fracción de su movimiento
        float x = anchor.x + (cp.x - anchor.x) * parallaxX;
        float y = anchor.y + (cp.y - anchor.y) * parallaxY;

        if (repeatHorizontal)
        {
            float rel = x - cp.x;
            rel = Mathf.Repeat(rel + tileWidth * 0.5f, tileWidth) - tileWidth * 0.5f;
            x = cp.x + rel;
        }

        transform.position = new Vector3(x, y, anchor.z);
    }
}