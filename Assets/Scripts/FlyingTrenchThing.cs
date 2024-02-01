using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlyingTrenchThing : MonoBehaviour
{

    public float RotateSpeed, SpeedMod, SlowMod;
    [SerializeField]
    private float _speedMod, _timer;

    private void Update()
    {

        if(transform.rotation.eulerAngles.z > 180)
        {

            _timer = Mathf.Clamp(_timer += Time.deltaTime * 0.4f, 0f, 1f);

        }
        else if(transform.rotation.eulerAngles.z < 180)
        {

            _timer = Mathf.Clamp(_timer -= Time.deltaTime * 0.4f, 0f, 1f);

        }

        _speedMod = Mathf.Lerp(SlowMod, SpeedMod, _timer);

        transform.Rotate(Vector3.forward * RotateSpeed * Time.deltaTime * _speedMod);

    }

}
