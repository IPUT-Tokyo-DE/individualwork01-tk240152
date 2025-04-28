using UnityEngine;

public class Enemy : MonoBehaviour
{
    public float moveSpeed = 2f;
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float shootInterval = 2f;
    public GameObject deathEffectPrefab; // 追加：死亡エフェクトプレハブ

    private Transform player;
    private float shootTimer = 0f;

    void Start()
    {
        GameObject playerObj = GameObject.FindGameObjectWithTag("Player");
        if (playerObj != null)
        {
            player = playerObj.transform;
        }
    }

    void Update()
    {
        if (player == null) return;

        Vector2 dir = (player.position - transform.position).normalized;
        transform.position += (Vector3)(dir * moveSpeed * Time.deltaTime);

        shootTimer += Time.deltaTime;
        if (shootTimer >= shootInterval)
        {
            shootTimer = 0f;
            ShootAtPlayer();
        }
    }

    void ShootAtPlayer()
    {
        if (bulletPrefab == null || firePoint == null) return;

        Vector2 dir = (player.position - firePoint.position).normalized;
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.gravityScale = 0;
            rb.linearVelocity = dir * 5f;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            Die(); // 弾に当たったら死亡
            Destroy(other.gameObject); // 弾も消す
        }
    }

    void Die()
    {
        // 破裂エフェクト表示
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }

        SoundManager.Instance.PlaySFX(SoundManager.Instance.enemyDeathSfx);

        // スコア加算
        if (ScoreManager.Instance != null)
        {
            ScoreManager.Instance.AddScore(100); // 100点加算（好きな値にしてOK）
        }


        Destroy(gameObject); // 敵を削除
    }
}
