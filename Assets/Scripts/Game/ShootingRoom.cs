using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShootingRoom : MonoBehaviour {
    public Camera Camera;

	// Use this for initialization
	void Start () {
        var collider = GetComponent<BoxCollider2D>();
        var bottomLeft = Camera.ViewportToWorldPoint(Vector2.zero);
        var topRight = Camera.ViewportToWorldPoint(Vector2.one);
        var size = topRight - bottomLeft;
        collider.transform.localScale = size;
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "PlayerShot" || collision.tag == "EnemyShot")
        {
            Destroy(collision.gameObject);
        }
    }
}
