using UnityEngine;
using UnityEngine.SceneManagement;

public class StartMenu : MonoBehaviour
{
    // �Q�[���J�n�{�^��
    public void StartGame()
    {
        SceneManager.LoadScene("MainScene");
    }

    // �I���{�^��
    public void QuitGame()
    {
        // �G�f�B�^�p�i���s���~�j
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#else
        // �r���h��̎��s�t�@�C���p
        Application.Quit();
#endif
    }
}
