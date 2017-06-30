using System;
using System.Collections;
using System.Collections.Generic;
using Ist;
using UnityEngine;
using UniRx;

public class BulletRenderer : MonoBehaviour {
    public GameObject ShotObject;

    internal List<Rigidbody2D> Bullets;
    private BatchRenderer batchRenderer;

    void Start()
	{
		Bullets = new List<Rigidbody2D>();
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
        shot.transform.parent = SpriteStudioManager.I.ManagerDraw.transform;
		rigidBody.velocity = direction * speed;
		
        Bullets.Add(rigidBody);
        var script = shot.GetComponent<EnemyShot>();
        script.DestroyEvent.Subscribe(u => Bullets.Remove(rigidBody));
    }

    private void Update()
    {
        foreach (var b in Bullets)
        {
            batchRenderer.AddInstanceTS(b.transform.position, b.transform.lossyScale);
        }
    }
}
