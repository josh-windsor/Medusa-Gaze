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
	private LineRenderer _playerLine;

	private void Start ()
	{
		_myRenderer = GetComponent<Renderer>();
		GameObject line = new GameObject("Line");
		line.transform.parent = this.transform;
		_playerLine = line.AddComponent<LineRenderer>();
		_playerLine.startColor = Color.red;
		_playerLine.endColor = Color.red;
		_playerLine.startWidth = 0.2f;
		_playerLine.endWidth = 0.2f;
		_playerLine.material = new Material(Shader.Find("Particles/Alpha Blended Premultiply"));
		if (_player == null)
		{
			_player = GameObject.Find("Player").GetComponent<PlayerController>();
		}
		if (_mainCam == null)
		{
			_mainCam = Camera.main;
		}
	}

	// Update is called once per framec
	private void FixedUpdate ()
	{
		float step = _speed * Time.deltaTime;
		if (Vector3.Distance(transform.position, _player.transform.position) < 40)
		{
			transform.position = Vector3.MoveTowards(transform.position, _player.transform.position, step);

			Vector3 pos = Camera.main.WorldToViewportPoint(transform.position);

			if (pos.x < 0.0 || 1.0 < pos.x || pos.y < 0.0 || 1.0 < pos.y)
			{
				_playerLine.gameObject.SetActive(true);
				_playerLine.SetPositions(new[] { _player.transform.position, transform.position });
			}
			else
			{
				_playerLine.gameObject.SetActive(false);
			}
		}
		else
		{
			_playerLine.gameObject.SetActive(false);
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
