using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip titleBgm;     // �^�C�g���V�[����BGM
    public AudioClip mainBgm;      // ���C���V�[����BGM
    public AudioClip gameOverBgm;  // �Q�[���I�[�o�[�V�[����BGM

    [Header("SFX Clips")]
    public AudioClip shootSfx;
    public AudioClip enemyDeathSfx;

    [Header("Settings")]
    public float bgmFadeDuration = 1.0f;
    [Range(0f, 1f)] public float bgmVolume = 1.0f;
    [Range(0f, 1f)] public float sfxVolume = 1.0f;

    // �V�[������萔�Ƃ��Ē�`
    private const string TitleSceneName = "StartMenu";
    private const string MainSceneName = "MainScene";
    private const string GameOverSceneName = "GameOverScene";

    void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
        DontDestroyOnLoad(gameObject);

        // AudioSource���A�T�C������Ă��Ȃ��ꍇ�͎����ō쐬
        if (bgmSource == null)
        {
            GameObject bgmObj = new GameObject("BGM Source");
            bgmSource = bgmObj.AddComponent<AudioSource>();
            bgmSource.transform.SetParent(transform);
            bgmSource.volume = bgmVolume;
            bgmSource.loop = true;
        }

        if (sfxSource == null)
        {
            GameObject sfxObj = new GameObject("SFX Source");
            sfxSource = sfxObj.AddComponent<AudioSource>();
            sfxSource.transform.SetParent(transform);
            sfxSource.volume = sfxVolume;
        }

        // �V�[�����ύX���ꂽ�Ƃ��ɌĂ΂��C�x���g��o�^
        SceneManager.sceneLoaded += OnSceneLoaded;

        // �Q�[���J�n���ɃA�N�e�B�u�ȃV�[�����^�C�g���V�[���ł���΃^�C�g��BGM���Đ�
        //if (SceneManager.GetActiveScene().name == TitleSceneName)
        //{
            PlayBGM(titleBgm);
        //}
    }

    void OnDestroy()
    {
        // �V�[�����ύX���ꂽ�Ƃ��ɌĂ΂��C�x���g������
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // �V�[�����[�h���ɌĂ΂��
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // �^�C�g���V�[����BGM���Đ�
        if (scene.name == TitleSceneName)
        {
            PlayBGM(titleBgm);
        }
        // ���C���V�[����BGM���Đ�
        else if (scene.name == MainSceneName)
        {
            PlayBGM(mainBgm);
        }
        // �Q�[���I�[�o�[�V�[���̏ꍇ�ABGM��ύX����i�K�v�ɉ����āj
        else if (scene.name == GameOverSceneName)
        {
            PlayBGM(gameOverBgm);
        }
    }

    // �X�^�[�g�{�^���������ꂽ�Ƃ��ɌĂ΂�郁�\�b�h
    public void OnStartButtonPressed()
    {
        // �^�C�g��BGM���Đ����ł���΃t�F�[�h�A�E�g�����ă��C��BGM���t�F�[�h�C���A���̌�V�[���J��
        StartCoroutine(FadeOutTitleAndFadeInMain());
    }

    // BGM���Đ��i�t�F�[�h�C���t���j
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying) return; // ����BGM���Đ����Ȃ牽�����Ȃ�

        if (bgmSource.isPlaying)
        {
            StartCoroutine(FadeOutAndPlay(clip, bgmFadeDuration));
        }
        else
        {
            bgmSource.clip = clip;
            bgmSource.volume = 0f;
            bgmSource.Play();
            StartCoroutine(FadeIn(bgmSource, bgmVolume, bgmFadeDuration));
        }
    }

    // BGM���~�i�t�F�[�h�A�E�g�t���j
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            StartCoroutine(FadeOut(bgmSource, bgmFadeDuration));
        }
    }

    // SFX���Đ�
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // BGM�t�F�[�h�C��
    private IEnumerator FadeIn(AudioSource audioSource, float targetVolume, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        audioSource.volume = targetVolume;
    }

    // BGM�t�F�[�h�A�E�g
    private IEnumerator FadeOut(AudioSource audioSource, float duration)
    {
        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, 0f, currentTime / duration);
            yield return null;
        }
        audioSource.Stop();
        audioSource.volume = bgmVolume; // �{�����[����߂��Ă���
    }

    // BGM�t�F�[�h�A�E�g��ɐV����BGM���Đ�
    private IEnumerator FadeOutAndPlay(AudioClip newClip, float duration)
    {
        yield return StartCoroutine(FadeOut(bgmSource, duration));
        bgmSource.clip = newClip;
        bgmSource.volume = 0f;
        bgmSource.Play();
        yield return StartCoroutine(FadeIn(bgmSource, bgmVolume, duration));
    }

    // �^�C�g��BGM���t�F�[�h�A�E�g�����ă��C��BGM���t�F�[�h�C���A���̌�V�[�������[�h
    private IEnumerator FadeOutTitleAndFadeInMain()
    {
        // �^�C�g��BGM���Đ����ł���΃t�F�[�h�A�E�g
        if (bgmSource.clip == titleBgm && bgmSource.isPlaying)
        {
            yield return StartCoroutine(FadeOut(bgmSource, bgmFadeDuration));
        }

        // ���C��BGM���Đ��i�t�F�[�h�C���j
        PlayBGM(mainBgm);

        // �V�[�������[�h
        SceneManager.LoadScene(MainSceneName);
    }
}