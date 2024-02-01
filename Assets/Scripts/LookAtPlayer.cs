using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LookAtPlayer : MonoBehaviour
{

    private Transform _player;

    private void Start()
    {

        _player = GameObject.FindGameObjectWithTag("Player").transform;

    }

    private void Update()
    {

        transform.LookAt(_player);

    }

}
