using UnityEngine;
using System.Collections;

public class EnemyLogic : MonoBehaviour {

    public int Health = 100;

    void ApplyDamage() {

        Health -= MeleeSystem.Damage;

    }

    void Update() {
        if (Health < 1) {
            Destroy(this.gameObject);
        }
    }

}
