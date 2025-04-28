using UnityEngine;

public class PlayerShooting : MonoBehaviour
{
    public GameObject bulletPrefab;
    public Transform firePoint;
    public float bulletSpeed = 10f;
    public float fireCooldown = 0.5f; // クールタイム（秒）

    private float fireTimer = 0f;

    void Update()
    {
        fireTimer += Time.deltaTime;

        // 左クリック ＆ クールタイム経過チェック
        if (Input.GetMouseButton(0) && fireTimer >= fireCooldown)
        {
            fireTimer = 0f;
            Shoot();
        }
    }

    void Shoot()
    {
        SoundManager.Instance.PlaySFX(SoundManager.Instance.shootSfx);

        if (bulletPrefab == null || firePoint == null)
        {
            Debug.LogWarning("BulletPrefab または FirePoint が設定されていません！");
            return;
        }

        // マウスカーソルの位置取得（スクリーン座標 → ワールド座標）
        Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        mousePos.z = 0f; // z座標を固定

        // 発射位置からマウス方向への正規化ベクトル
        Vector2 direction = (mousePos - firePoint.position).normalized;

        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, Quaternion.identity);
        Rigidbody2D rb = bullet.GetComponent<Rigidbody2D>();
        if (rb != null)
        {
            rb.linearVelocity = direction * bulletSpeed;
        }

        // 5秒後に自動で消える
        Destroy(bullet, 1.5f);
    }
}
