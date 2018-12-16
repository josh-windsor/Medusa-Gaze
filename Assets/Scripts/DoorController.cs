using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class DoorController : MonoBehaviour
{

	private static PlayerController _player;
	private static Camera _mainCam;

	private Renderer _myRenderer;


	private void Start()
	{
		_myRenderer = GetComponent<Renderer>();
		if (_mainCam == null)
		{
			_mainCam = Camera.main;
		}
		if (_player == null)
		{
			_player = GameObject.Find("Player").GetComponent<PlayerController>();
		}
	}


	// Update is called once per frame
	void Update () {
		Vector2 gazePoint = TobiiAPI.GetGazePoint().Screen;
		Vector2 screenPoint = _mainCam.WorldToScreenPoint(transform.position);
		if (Vector2.Distance(gazePoint, screenPoint) < 250f && Vector3.Distance(transform.position, _player.transform.position) < 5f)
		{
			Color c = _myRenderer.material.color;
			if (c.a < 0.01f)
			{
				Destroy(this.gameObject);
			}
			c.a = c.a - 0.003f;
			_myRenderer.material.color = c;
		}

	}
}
