using UnityEngine;
public class Portal : MonoBehaviour
{
	[SerializeField] Transform destinationPortal;
	[SerializeField] AudioSource audiosource;
	[SerializeField] AudioClip portalSound;
	float portalDelay = 0.1f;
	void OnTriggerEnter2D(Collider2D other)
	{
		if (other.CompareTag("Player"))
		{
			Player player = other.GetComponent<Player>();
			if (player.lastTeleportTime < Time.time - portalDelay)
			{
				player.lastTeleportTime = Time.time;
				player.transform.position = destinationPortal.transform.position;
				audiosource.PlayOneShot(portalSound);
			}
		}
	}
}