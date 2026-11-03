using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] float loadDelay = 2f;

    public void StartFirstLevel()
    {
        TransicionEscenasUI.instancia.DisolverSalida(SceneManager.GetActiveScene().buildIndex + 2);
    }

    public void LoadMainMenu()
    {
        StartCoroutine(delayMenu()); 
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    IEnumerator delayMenu()
    {
        yield return new WaitForSeconds(loadDelay);
        SceneManager.LoadScene(0);
    }
}
