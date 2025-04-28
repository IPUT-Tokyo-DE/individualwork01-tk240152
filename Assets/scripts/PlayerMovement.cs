using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed = 5f;
    public Vector2 moveBoundsMin = new Vector2(-8f, -4.5f);
    public Vector2 moveBoundsMax = new Vector2(8f, 4.5f);

    void Update()
    {
        Vector2 moveInput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        Vector2 newPos = (Vector2)transform.position + moveInput.normalized * moveSpeed * Time.deltaTime;

        // âÊñ ì‡Ç…êßå¿
        newPos.x = Mathf.Clamp(newPos.x, moveBoundsMin.x, moveBoundsMax.x);
        newPos.y = Mathf.Clamp(newPos.y, moveBoundsMin.y, moveBoundsMax.y);

        transform.position = newPos;
    }
}
