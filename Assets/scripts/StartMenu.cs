using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // ゲーム開始ボタン
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // 終了ボタン
    public void QuitGame()
    {
        // エディタ用（実行中止）
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // ビルド後の実行ファイル用
        Application.Quit();
#endif
    }
}
