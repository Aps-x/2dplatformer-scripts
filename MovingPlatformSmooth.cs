using UnityEngine;
public class MovingPlatformSmooth : MonoBehaviour
{
    [SerializeField] Transform[] waypoints;
    [SerializeField] float speed;
    Vector3 velocity = Vector3.zero;
    int target = 0;
    void Update()
    {
        transform.position = Vector3.SmoothDamp(transform.position, waypoints[target].position, ref velocity, speed);
    }
	void LateUpdate()
	{
		if (Vector2.Distance(transform.position, waypoints[target].position) < 0.35f)
		{
            if (target == waypoints.Length - 1)
			{
                target = 0;
			}
			else
			{
				target++;
			}
		}
	}
}
