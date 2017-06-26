using System;
using System.Collections;
using System.Collections.Generic;
using Ist;
using UnityEngine;
using UniRx;

public class BulletRenderer : MonoBehaviour {
    public GameObject ShotObject;
    public Script_SpriteStudio_ManagerDraw View;

    private List<Rigidbody2D> bullets;
    private BatchRenderer batchRenderer;

    void Start()
	{
		bullets = new List<Rigidbody2D>();
		batchRenderer = GetComponent<BatchRenderer>();
    }

    public void Shoot(Vector3 source, float angle, float speed)
	{
		var x = Mathf.Sin(angle * Mathf.Deg2Rad);
		var y = Mathf.Cos(angle * Mathf.Deg2Rad);
		var direction = new Vector2(x, y);

		var shot = Instantiate(ShotObject);
		var rigidBody = shot.GetComponent<Rigidbody2D>();
		shot.transform.position = source;
        shot.transform.parent = View.transform;
		rigidBody.velocity = direction * speed;
		
        bullets.Add(rigidBody);
        var script = shot.GetComponent<EnemyShot>();
        script.DestroyEvent.Subscribe(u => bullets.Remove(rigidBody));
    }

    private void Update()
    {
        foreach (var b in bullets)
        {
            batchRenderer.AddInstanceTS(b.transform.position, b.transform.localScale);
        }
    }
}
