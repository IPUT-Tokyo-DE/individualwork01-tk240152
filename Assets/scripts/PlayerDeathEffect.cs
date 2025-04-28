using UnityEngine;

public class PlayerDeathEffect : MonoBehaviour
{
    public GameObject piecePrefab; // 飛び散るパーツのプレハブ
    public int pieceCount = 8;     // 飛ばす数
    public float explodeForce = 5f; // 飛ばす力

    void Start()
    {
        Explode();
    }

    void Explode()
    {
        for (int i = 0; i < pieceCount; i++)
        {
            // 角度を均等に分けて発射方向を計算
            float angle = (360f / pieceCount) * i;
            Vector2 direction = Quaternion.Euler(0, 0, angle) * Vector2.right;

            // パーツを生成
            GameObject piece = Instantiate(piecePrefab, transform.position, Quaternion.identity);

            // Rigidbody2Dに力を加える
            Rigidbody2D rb = piece.GetComponent<Rigidbody2D>();
            if (rb != null)
            {
                rb.gravityScale = 0;
                rb.linearVelocity = direction * explodeForce;
            }

            // 1秒後にパーツを削除
            Destroy(piece, 1f);
        }

        // 自分（エフェクト本体）も削除
        Destroy(gameObject);
    }
}