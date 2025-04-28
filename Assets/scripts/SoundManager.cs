using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class SoundManager : MonoBehaviour
{
    public static SoundManager Instance;

    public AudioSource bgmSource;
    public AudioSource sfxSource;

    [Header("BGM Clips")]
    public AudioClip titleBgm;     // タイトルシーンのBGM
    public AudioClip mainBgm;      // メインシーンのBGM
    public AudioClip gameOverBgm;  // ゲームオーバーシーンのBGM

    [Header("SFX Clips")]
    public AudioClip shootSfx;
    public AudioClip enemyDeathSfx;

    [Header("Settings")]
    public float bgmFadeDuration = 1.0f;
    [Range(0f, 1f)] public float bgmVolume = 1.0f;
    [Range(0f, 1f)] public float sfxVolume = 1.0f;

    // シーン名を定数として定義
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

        // AudioSourceがアサインされていない場合は自動で作成
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

        // シーンが変更されたときに呼ばれるイベントを登録
        SceneManager.sceneLoaded += OnSceneLoaded;

        // ゲーム開始時にアクティブなシーンがタイトルシーンであればタイトルBGMを再生
        //if (SceneManager.GetActiveScene().name == TitleSceneName)
        //{
            PlayBGM(titleBgm);
        //}
    }

    void OnDestroy()
    {
        // シーンが変更されたときに呼ばれるイベントを解除
        if (Instance == this)
        {
            SceneManager.sceneLoaded -= OnSceneLoaded;
        }
    }

    // シーンロード時に呼ばれる
    void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        // タイトルシーンのBGMを再生
        if (scene.name == TitleSceneName)
        {
            PlayBGM(titleBgm);
        }
        // メインシーンのBGMを再生
        else if (scene.name == MainSceneName)
        {
            PlayBGM(mainBgm);
        }
        // ゲームオーバーシーンの場合、BGMを変更する（必要に応じて）
        else if (scene.name == GameOverSceneName)
        {
            PlayBGM(gameOverBgm);
        }
    }

    // スタートボタンが押されたときに呼ばれるメソッド
    public void OnStartButtonPressed()
    {
        // タイトルBGMが再生中であればフェードアウトさせてメインBGMをフェードイン、その後シーン遷移
        StartCoroutine(FadeOutTitleAndFadeInMain());
    }

    // BGMを再生（フェードイン付き）
    public void PlayBGM(AudioClip clip)
    {
        if (bgmSource.clip == clip && bgmSource.isPlaying) return; // 同じBGMが再生中なら何もしない

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

    // BGMを停止（フェードアウト付き）
    public void StopBGM()
    {
        if (bgmSource.isPlaying)
        {
            StartCoroutine(FadeOut(bgmSource, bgmFadeDuration));
        }
    }

    // SFXを再生
    public void PlaySFX(AudioClip clip)
    {
        if (sfxSource != null && clip != null)
        {
            sfxSource.PlayOneShot(clip, sfxVolume);
        }
    }

    // BGMフェードイン
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

    // BGMフェードアウト
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
        audioSource.volume = bgmVolume; // ボリュームを戻しておく
    }

    // BGMフェードアウト後に新しいBGMを再生
    private IEnumerator FadeOutAndPlay(AudioClip newClip, float duration)
    {
        yield return StartCoroutine(FadeOut(bgmSource, duration));
        bgmSource.clip = newClip;
        bgmSource.volume = 0f;
        bgmSource.Play();
        yield return StartCoroutine(FadeIn(bgmSource, bgmVolume, duration));
    }

    // タイトルBGMをフェードアウトさせてメインBGMをフェードイン、その後シーンをロード
    private IEnumerator FadeOutTitleAndFadeInMain()
    {
        // タイトルBGMが再生中であればフェードアウト
        if (bgmSource.clip == titleBgm && bgmSource.isPlaying)
        {
            yield return StartCoroutine(FadeOut(bgmSource, bgmFadeDuration));
        }

        // メインBGMを再生（フェードイン）
        PlayBGM(mainBgm);

        // シーンをロード
        SceneManager.LoadScene(MainSceneName);
    }
}