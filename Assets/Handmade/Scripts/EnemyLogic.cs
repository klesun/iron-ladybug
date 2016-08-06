using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {

    public int Health = 100;

    void ApplyDamage() {

        Health -= MeleeSystem.Damage;

    }

    void Update() {
        if (Health <= 0) {
            Destroy(this.gameObject);
        }
    }

}
