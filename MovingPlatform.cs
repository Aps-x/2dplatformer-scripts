using UnityEngine;
public class MovingPlatform : MonoBehaviour
{
    [SerializeField] Rigidbody2D rb;
    [SerializeField] public Axis axis;
    [SerializeField] StartDirection direction;
    [SerializeField] float Up_Right_Limit;
    [SerializeField] float Down_Left_Limit;
    [SerializeField] float platformSpeed;
    public Vector2 platformMovement;
    Vector2 movementDir;
    public enum Axis
    {
        Vertical,
        Horizontal
    }
    enum StartDirection
	{
        Up,
        Down,
        Left,
        Right
	}
    void Start()
    {
        switch (direction)
		{
            case StartDirection.Up:
                movementDir = Vector2.up;
                break;
            case StartDirection.Down:
                movementDir = Vector2.down;
                break;
            case StartDirection.Left:
                movementDir = Vector2.left;
                break;
            case StartDirection.Right:
                movementDir = Vector2.right;
                break;
        }
    }
    void FixedUpdate()
    {
        if (axis == Axis.Vertical)
		{
            if (transform.position.y > Up_Right_Limit)
            {
                movementDir = Vector2.down;
            }
            if (transform.position.y < Down_Left_Limit)
            {
                movementDir = Vector2.up;
            }
        }
        else if (axis == Axis.Horizontal)
		{
            if (transform.position.x > Up_Right_Limit)
            {
                movementDir = Vector2.left;
            }
            if (transform.position.x < Down_Left_Limit)
            {
                movementDir = Vector2.right;
            }
        }
        platformMovement = movementDir * platformSpeed;
        rb.MovePosition(rb.position + platformMovement * Time.fixedDeltaTime); 
    }
}