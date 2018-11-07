using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private float _speed = 1.0f;

	private static PlayerController _player;
	private static Camera _mainCam;
	private Renderer _myRenderer;

	private void Start ()
	{
		_myRenderer = GetComponent<Renderer>();
		if (_mainCam == null)
		{
			_mainCam = Camera.main;
			_player = GameObject.Find("Player").GetComponent<PlayerController>();
		}
	}

	// Update is called once per frame
	private void Update ()
	{
		float step = _speed * Time.deltaTime;
		transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);

		if (_player.transform.position != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				Quaternion.LookRotation(_player.transform.position),
				Time.deltaTime
			);
		}

		if (Vector3.Distance(transform.position, _player.transform.position) < 2f)
		{
			_player.TakeDamage(5);
		}

		Vector2 gazePoint = TobiiAPI.GetGazePoint().Screen;
		Vector2 screenPoint = _mainCam.WorldToScreenPoint(transform.position);
		if (Vector2.Distance(gazePoint, screenPoint) < 250f)
		{
			_myRenderer.material.color = Color.blue;
		}
		else
		{
			_myRenderer.material.color = Color.red;
		}
	}
}
