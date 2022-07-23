using UnityEngine;

public class Cloud : MonoBehaviour
{
    private float speed;
    private void Start()
    {
        speed = Random.Range(0.2f, 0.4f);
    }
    private void Update()
    {
        if (transform.position.x > 61)
            RestartCloud();

        transform.Translate(Vector3.right * speed * Time.deltaTime);
    }
    private void RestartCloud()
    {
        transform.position = new Vector3(-18, transform.position.y, transform.position.z);
        speed = Random.Range(0.2f, 0.4f);
    }
}
