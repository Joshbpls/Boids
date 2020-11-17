using UnityEngine;

public class CameraController : MonoBehaviour
{

    public float moveSpeed;
    public float scrollSpeed;
    private Vector3 movement;
    
    void Start()
    {
        movement = new Vector3(0, 0, 0);
    }
    
    void Update()
    {
        movement.x = Input.GetAxisRaw("Horizontal");
        movement.y = Input.GetAxisRaw("Vertical");
        movement.z = Input.GetAxisRaw("Mouse ScrollWheel");
        if (movement.x != 0 || movement.y != 0)
        {
            transform.position += movement * (moveSpeed * Time.deltaTime);
        }

        if (movement.z != 0)
        {
            
            transform.position += movement * (scrollSpeed * Time.deltaTime);
        }
    }
}
