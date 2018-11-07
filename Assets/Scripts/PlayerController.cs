using System.Collections;
using System.Collections.Generic;
using Tobii.Gaming;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	private Vector2 _middlePoint;
	private Camera _mainCam;
    private int _hp = 100;
    private bool _takenDmg = false;

	// Use this for initialization
	void Start ()
	{
		_middlePoint = new Vector2(Screen.width / 2f, Screen.height / 2f);
		_mainCam = Camera.main;
	}
	
	// Update is called once per frame
	private void Update ()
	{
		Vector2 gazePoint = TobiiAPI.GetGazePoint().Screen;
		float xDif = (_middlePoint.x - gazePoint.x) * -1;
		float zDif = (_middlePoint.y - gazePoint.y) * -1;
		/*if (xDif < -300)
		{
			transform.position += new Vector3(xDif / 10000, 0, 0);
		}
		else if (xDif > 300)
		{
			transform.position += new Vector3(xDif / 10000, 0, 0);
		}
		if (zDif < -250)
		{
			transform.position += new Vector3(0, 0, zDif / 10000);
		}
		else if (zDif > 250)
		{
			transform.position += new Vector3(0, 0, zDif / 10000);
		}*/

		Vector3 direction = new Vector3(xDif, 0, zDif);
		if (direction != Vector3.zero)
		{
			transform.rotation = Quaternion.Slerp(
				transform.rotation,
				Quaternion.LookRotation(direction),
				Time.deltaTime * 2
			);
		}
		_mainCam.transform.position = new Vector3(transform.position.x, 10, transform.position.z);
	}

    public void TakeDamage(int iDamage)
    {
        if (!_takenDmg)
        {
            _takenDmg = true;
            _hp -= iDamage;
            StartCoroutine(WaitForDmg());

            Debug.Log("Dmg Taken");
            if (_hp <= 0)
            {
                //ded
                Debug.Log("U Ded");
            }

        }
    }

    private IEnumerator WaitForDmg()
    {
        yield return new WaitForSeconds(3.0f);
        _takenDmg = false;
    }
}
