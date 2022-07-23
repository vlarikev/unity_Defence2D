using UnityEngine;

public class CameraController : MonoBehaviour
{
    private void LateUpdate()
    {
        MovementWithTouch();
        MovementTestPC();
    }
    private void MovementWithTouch()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Moved)
        {
            Vector2 touchStart = Input.GetTouch(0).deltaPosition;
            transform.Translate(-touchStart.x * 0.03f, -1, 0);
            transform.position = new Vector3(Mathf.Clamp(transform.position.x, -4.3f, 46.9f), -1, -10);
        }
    }
    private void MovementTestPC()
    {
        if (Input.GetKey(KeyCode.D))
        {
            transform.position = new Vector3(transform.position.x + 0.5f, transform.position.y, -10);
        }
        if (Input.GetKey(KeyCode.A))
        {
            transform.position = new Vector3(transform.position.x - 0.5f, transform.position.y, -10);
        }
    }
}
