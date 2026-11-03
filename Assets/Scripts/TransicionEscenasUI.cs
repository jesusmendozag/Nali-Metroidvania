using UnityEngine;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class TransicionEscenasUI : MonoBehaviour
{
    [Header("Disolver")]
    [SerializeField] AudioClip inicioPickUpSFX;

    public CanvasGroup disolverCanvasGroup;
    public float tiempoDisolverEntrada;
    public float tiempoDisolverSalida;
    public static TransicionEscenasUI instancia;

    private AudioSource audioSource;

    private void Awake()
    {
        if (instancia == null)
        {
            instancia = this;
        }
        else
        {
            Destroy(gameObject);
        }

        audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        DisolverEntrada();
    }

    private void DisolverEntrada()
    {
        LeanTween.alphaCanvas(disolverCanvasGroup, 0f, tiempoDisolverEntrada).setOnComplete(() =>
        {
            disolverCanvasGroup.interactable = false;
            disolverCanvasGroup.blocksRaycasts = false;
        });
    }

    public void DisolverSalida(int indexEscena)
    {
        if (inicioPickUpSFX != null && audioSource != null)
        {
            
            audioSource.PlayOneShot(inicioPickUpSFX); 
        }

        LeanTween.alphaCanvas(disolverCanvasGroup, 1f, tiempoDisolverSalida).setOnComplete(() =>
        {
            SceneManager.LoadScene(indexEscena);
        });
    }
}
