using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;    // 敵のプレハブ
    public float initialSpawnInterval = 2f; // 最初の出現間隔
    public float minSpawnInterval = 0.5f;   // 最短間隔
    public float spawnAcceleration = 0.05f; // 減少スピード

    private float currentSpawnInterval;
    private float timer = 0f;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // 敵を出現させる
        if (timer >= currentSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;

            // 少しずつ出現間隔を短くする
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnAcceleration);
        }
    }

    void SpawnEnemy()
    {
        // 画面の外の座標を計算
        Vector2 spawnPosition = GetOffScreenPosition();

        // 敵を出現
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector2 GetOffScreenPosition()
    {
        // カメラの位置とビューの端を基にして画面外の座標を計算
        Camera mainCamera = Camera.main;
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;  // 画面の幅
        float screenHeight = mainCamera.orthographicSize; // 画面の高さ

        // 画面外のランダムな位置（上、下、左、右のいずれか）
        Vector2 offScreenPosition = new Vector2();

        int direction = Random.Range(0, 4);  // 0: 上, 1: 下, 2: 左, 3: 右
        switch (direction)
        {
            case 0: // 上
                offScreenPosition = new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight + 1f);
                break;
            case 1: // 下
                offScreenPosition = new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight - 1f);
                break;
            case 2: // 左
                offScreenPosition = new Vector2(-screenWidth - 1f, Random.Range(-screenHeight, screenHeight));
                break;
            case 3: // 右
                offScreenPosition = new Vector2(screenWidth + 1f, Random.Range(-screenHeight, screenHeight));
                break;
        }

        return offScreenPosition;
    }
}
