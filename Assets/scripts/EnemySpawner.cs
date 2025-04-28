using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemyPrefab;    // �G�̃v���n�u
    public float initialSpawnInterval = 2f; // �ŏ��̏o���Ԋu
    public float minSpawnInterval = 0.5f;   // �ŒZ�Ԋu
    public float spawnAcceleration = 0.05f; // �����X�s�[�h

    private float currentSpawnInterval;
    private float timer = 0f;

    void Start()
    {
        currentSpawnInterval = initialSpawnInterval;
    }

    void Update()
    {
        timer += Time.deltaTime;

        // �G���o��������
        if (timer >= currentSpawnInterval)
        {
            SpawnEnemy();
            timer = 0f;

            // �������o���Ԋu��Z������
            currentSpawnInterval = Mathf.Max(minSpawnInterval, currentSpawnInterval - spawnAcceleration);
        }
    }

    void SpawnEnemy()
    {
        // ��ʂ̊O�̍��W���v�Z
        Vector2 spawnPosition = GetOffScreenPosition();

        // �G���o��
        Instantiate(enemyPrefab, spawnPosition, Quaternion.identity);
    }

    Vector2 GetOffScreenPosition()
    {
        // �J�����̈ʒu�ƃr���[�̒[����ɂ��ĉ�ʊO�̍��W���v�Z
        Camera mainCamera = Camera.main;
        float screenWidth = mainCamera.orthographicSize * mainCamera.aspect;  // ��ʂ̕�
        float screenHeight = mainCamera.orthographicSize; // ��ʂ̍���

        // ��ʊO�̃����_���Ȉʒu�i��A���A���A�E�̂����ꂩ�j
        Vector2 offScreenPosition = new Vector2();

        int direction = Random.Range(0, 4);  // 0: ��, 1: ��, 2: ��, 3: �E
        switch (direction)
        {
            case 0: // ��
                offScreenPosition = new Vector2(Random.Range(-screenWidth, screenWidth), screenHeight + 1f);
                break;
            case 1: // ��
                offScreenPosition = new Vector2(Random.Range(-screenWidth, screenWidth), -screenHeight - 1f);
                break;
            case 2: // ��
                offScreenPosition = new Vector2(-screenWidth - 1f, Random.Range(-screenHeight, screenHeight));
                break;
            case 3: // �E
                offScreenPosition = new Vector2(screenWidth + 1f, Random.Range(-screenHeight, screenHeight));
                break;
        }

        return offScreenPosition;
    }
}
