using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mob : EnemyShot
{
    [SerializeField]
    private Vector3 pushOnShoot;

    private Rigidbody2D rigidbody;

	// Use this for initialization
	void Start () {
        rigidbody = GetComponent<Rigidbody2D>();
	}

    private void OnCollisionEnter2D(Collision2D collision)
	{
		if (collision.collider.tag == Def.PlayerShotTag)
		{
			SoundManager.I.PlaySe(SeKind.Hit);
			rigidbody.AddForce(pushOnShoot);
			Destroy(collision.gameObject);
		}        
    }
}
