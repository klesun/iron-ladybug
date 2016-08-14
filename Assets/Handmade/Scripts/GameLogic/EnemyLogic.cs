using UnityEngine;
using System.Collections;
using AssemblyCSharp;

/** 
 * this class contains _only_ logic of how 
 * ai reacts on circumstances in the world
 * ALL npc logic must be placed into NpcControl (todo: rename)
 */
public class EnemyLogic : MonoBehaviour 
{
	public SpaceTrigger enemyDetectionRadius;
	public NpcControl npc;
	private HeroControl enemy = null;

	const float EPEE_LENGTH = 1.5f;

	void Start()
	{
		enemyDetectionRadius.callback = OnUfo;
	}

    void Update() 
	{
		if (enemy != null) {
			// TODO: with delay
			npc.Face (enemy.transform.position);

			var enemyDistance = enemy.transform.position - transform.position;
			var attackDistance = GetAttackDistance (npc.CanAttack() 
				? npc.GetVelocity() 
				: Vector3.zero);

			if (enemy.npc.IsGrounded () &&
				enemyDistance.magnitude <= attackDistance
			) {
				npc.Attack ();
			} else {
				print ("moving " + enemyDistance + " " + attackDistance);
				npc.Move (CeilAxis(enemyDistance));
			}
		}
    }

	void OnUfo(Collider collider)
	{
		foreach (var hero in collider.gameObject.GetComponents<HeroControl>()) {
			enemy = hero;
		}
	}

	float GetAttackDistance(Vector3 speed)
	{
		float t = 2 * NpcControl.LOUNGE_HEIGHT / 9.80665f;
		return (speed.magnitude + NpcControl.LOUNGE_LENGTH) * t + EPEE_LENGTH;
	}

	/** 
	 * transforms every axis of vector to either -1 or 1 
	 * if it is less or greater than zero accordingly 
	 * need this to simulate how human would press buttons
	*/
	Vector3 CeilAxis(Vector3 v)
	{
		return new Vector3 (
			Mathf.Sign(v.x),
			0, //Mathf.Sign(v.y),
			Mathf.Sign(v.z)
		);
	}
}
