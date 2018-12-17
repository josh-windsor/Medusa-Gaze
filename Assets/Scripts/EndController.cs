using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndController : MonoBehaviour
{

    private UiController _UIController;

    private void Awake()
    {
        _UIController = GameObject.Find("Canvas").GetComponent<UiController>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.name == "Player")
        {
            _UIController.WinGame();
        }
    }
}
