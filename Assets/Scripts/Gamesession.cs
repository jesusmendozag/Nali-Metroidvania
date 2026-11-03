using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Gamesession : MonoBehaviour
{

    [SerializeField] int totalScore = 0;
    [SerializeField] int playerHealth = 0;

    [SerializeField] TMP_Text scoreText;
    [SerializeField] TMP_Text healthText;
    [SerializeField] Slider healthSlider;
    [SerializeField] Image healthBar;


    bool isAlive = true;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        healthSlider.maxValue = playerHealth;
        healthSlider.value = playerHealth;
        scoreText.text = totalScore.ToString();
        healthText.text = playerHealth.ToString() + "/" + healthSlider.maxValue.ToString();
    }

    void Update()
    {
        if (!isAlive) { return; }
        DieProcess();
    }

    public void AddToScore(int pointsToAdd)
    {
        totalScore += pointsToAdd;
        scoreText.text = totalScore.ToString();
    }

    public void AddToHealth(int totalHealth)
    {
        playerHealth += totalHealth;
        healthText.text = playerHealth.ToString() + "/" + healthSlider.maxValue.ToString();
        healthSlider.value = playerHealth;
    }

    public void RemoveHealth(int damage)
    {
        playerHealth -= damage;
        healthText.text = playerHealth.ToString() + "/" + healthSlider.maxValue.ToString();
        healthSlider.value = playerHealth;
    }

    void DieProcess()
    {
        if (playerHealth <= 0)
        {
            isAlive = false;
            FindAnyObjectByType<PlayerMovement>().Die();
        }
    }

}
