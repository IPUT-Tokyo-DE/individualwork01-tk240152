using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject deathEffectPrefab; // 破裂エフェクトのプレハブ
    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // エフェクト生成
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        // ゲームオーバー
        GameManager.Instance.GameOver();

        // プレイヤー削除
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // 敵または敵弾に当たったら死亡
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            Die();
        }
    }
}
