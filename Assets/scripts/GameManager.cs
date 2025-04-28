using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    private int finalScore;

    void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);

            // シーンが変わったら呼ばれるイベントに登録
            SceneManager.sceneLoaded += OnSceneLoaded;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    void OnDestroy()
    {
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // シーンロード時に呼ばれる
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "MainScene")
        {
            if (SoundManager.Instance != null)
            {
                SoundManager.Instance.PlayBGM(SoundManager.Instance.mainBgm);
            }
        }
    }

    void Start()
    {
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayBGM(SoundManager.Instance.mainBgm);
        }
    }

    public void GameOver()
    {
        StartCoroutine(DelayGameOver());
    }

    IEnumerator DelayGameOver()
    {
        finalScore = ScoreManager.Instance.GetScore();

        yield return new WaitForSeconds(0.5f);

        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.StopBGM();
            SoundManager.Instance.PlayBGM(SoundManager.Instance.gameOverBgm);
        }

        SceneManager.LoadScene("GameOverScene");
    }

    public int GetFinalScore()
    {
        return finalScore;
    }
}
