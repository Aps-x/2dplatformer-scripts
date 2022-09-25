using UnityEngine;
public class Conveyor : MonoBehaviour
{
    [SerializeField] public float speed;
    [SerializeField] Rigidbody2D rb;
    [SerializeField] Direction direction;
    public Vector2 conveyorDirection;
    enum Direction
	{
        Left,
        Right
	}
	void Start()
	{
		switch(direction)
		{
            case Direction.Left:
                conveyorDirection = Vector2.right;
                break;
            case Direction.Right:
                conveyorDirection = Vector2.left;
                break;
        }
	}
	void FixedUpdate()
    {
        Vector3 pos = rb.position;
        rb.position += conveyorDirection * speed * Time.fixedDeltaTime;
        rb.MovePosition(pos);
    }
}