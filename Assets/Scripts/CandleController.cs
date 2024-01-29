using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CandleController : MonoBehaviour
{

    public float CandleLifeTime, LightDimmingThreshold, MinIntensity, MinCandleHeight;
    public bool IsLitFam, BurntOut;
    [SerializeField]
    private Light _light;
    [SerializeField]
    private Transform CandleVFX;
    private float _candleLifeTimer, _maxIntensity;

    private void Start()
    {

        _candleLifeTimer = CandleLifeTime;
        _maxIntensity = _light.intensity;

    }

    private void Update()
    {

        if(!IsLitFam || BurntOut)
            return;

        if(_candleLifeTimer <= 0)
        {

            Destroy(gameObject, 1f);
            BurntOut = true;
            gameObject.AddComponent<Rigidbody>();
            transform.parent = null;

        }
        else
        {

            _candleLifeTimer -= Time.deltaTime;
            CandleVFX.localScale = new Vector3(1f, Mathf.Lerp(1f, MinCandleHeight, 1f - (_candleLifeTimer / CandleLifeTime)), 1f);

        }

        if(_candleLifeTimer <= LightDimmingThreshold)
        {

            _light.intensity = Mathf.Lerp(_maxIntensity, MinIntensity, 1f - (_candleLifeTimer / LightDimmingThreshold));

        }

    }

    public void GetLitFam()
    {

        IsLitFam = true;
        _light.enabled = true;
        
    }

}
