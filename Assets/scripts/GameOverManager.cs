using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text finalScoreText;

    void Start()
    {
        finalScoreText.text = "Final Score: " + GameManager.Instance.GetFinalScore();
    }

    public void RetryGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    public void ReturnToTitle()
    {
        SceneManager.LoadScene("StartMenu");
    }
}