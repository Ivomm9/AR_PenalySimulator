using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using TMPro;
public class GameManager : MonoBehaviour
{
    public static GameManager Instance { get; private set; }

    public bool isGamePaused = false;
    public int score = 0;
    public int throws = 0;

    [SerializeField] TextMeshProUGUI scoreText;
    [SerializeField] TextMeshProUGUI throwsText;

    public UnityEvent onGameRestart;

    public bool gameStarted = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject); 
            return;
        }
        UpdateScoreText();
        Instance = this; 
        DontDestroyOnLoad(gameObject); 
    }

    public void AddScore(int points)
    {
        score += points;
        UpdateScoreText();
    }

    public void AddThrows(int throwsCount)
    {
        throws += throwsCount;
        UpdateThrowsText();
    }

    public void Restart() 
    {
        onGameRestart?.Invoke();
    }

    public void UpdateThrowsText()
    {
        if (throwsText != null)
        {
            throwsText.text = "Throws: " + throws.ToString();
        }
    }

    public void UpdateScoreText()
    {
        if (scoreText != null)
        {
            scoreText.text = "Goals: " + score.ToString();
        }
    }

}
