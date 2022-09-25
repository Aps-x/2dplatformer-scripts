using UnityEngine;
public class JumpPad : MonoBehaviour
{
	[SerializeField] Animator animator;
	[SerializeField] float bounceForce;
	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.CompareTag("Player"))
		{
			collision.gameObject.GetComponent<Rigidbody2D>().velocity = Vector2.zero;
			collision.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.up * bounceForce, ForceMode2D.Impulse);
			animator.SetTrigger("BounceTrigger");
		}
	}
}