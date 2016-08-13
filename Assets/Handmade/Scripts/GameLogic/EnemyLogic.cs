using UnityEngine;
using System.Collections;
using AssemblyCSharp;

public class EnemyLogic : MonoBehaviour, IPiercable 
{
	public AudioClip hitSound;
    public int health = 100;

	private Rigidbody body;
	private bool isDead = false;

	void Start()
	{
		body = GetComponent<Rigidbody> ();
		body.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
	}

    void ApplyDamage() 
	{
        health -= MeleeSystem.Damage;
    }

    void Update() 
	{
		if (health <= 0 && !isDead) {
			isDead = true;
			body.constraints = 0;
			body.velocity += transform.right * 5;
            // Destroy(this.gameObject);
        }
    }

	public void pierce()
	{
		health -= 50;
		AudioSource.PlayClipAtPoint(hitSound, transform.position);
	}
}
