using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class EnemyController : MonoBehaviour {

	private static PlayerController _player;
	private static Camera _mainCam;

	private float _speed = 1.0f;
	private Renderer _myRenderer;
	private bool _stunningPlayer;

	private void Start ()
	{
		_myRenderer = GetComponent<Renderer>();
		if (_player == null)
		{
			_player = GameObject.Find("Player").GetComponent<PlayerController>();
		}
		if (_mainCam == null)
		{
			_mainCam = Camera.main;
		}
	}

	// Update is called once per frame
	private void Update ()
	{
		float step = _speed * Time.deltaTime;
		if (Vector3.Distance(transform.position, _player.transform.position) < 40)
		{
			transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);
		}

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
			_player.TakeDamage(10);
		}

		Vector2 gazePoint = TobiiAPI.GetGazePoint().Screen;
		Vector2 screenPoint = _mainCam.WorldToScreenPoint(transform.position);
		if (Vector2.Distance(gazePoint, screenPoint) < 250f && !_stunningPlayer)
		{
			_stunningPlayer = true;
			StartCoroutine(StunPlayer());
		}
	}

	private IEnumerator StunPlayer()
	{
		_myRenderer.material.color = Color.blue;
		StartCoroutine(_player.StunPlayer());
		yield return new WaitForSeconds(3);
		_myRenderer.material.color = Color.red;
		_stunningPlayer = false;
	}
}
