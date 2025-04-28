using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject deathEffectPrefab; // �j��G�t�F�N�g�̃v���n�u
    private bool isDead = false;

    public void Die()
    {
        if (isDead) return;

        isDead = true;

        // �G�t�F�N�g����
        if (deathEffectPrefab != null)
        {
            Instantiate(deathEffectPrefab, transform.position, Quaternion.identity);
        }
        // �Q�[���I�[�o�[
        GameManager.Instance.GameOver();

        // �v���C���[�폜
        Destroy(gameObject);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        // �G�܂��͓G�e�ɓ��������玀�S
        if (other.CompareTag("Enemy") || other.CompareTag("EnemyBullet"))
        {
            Die();
        }
    }
}
