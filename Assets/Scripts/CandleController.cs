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
    private Transform CandleVFX, _candleFX, _candleWick;
    private float _candleLifeTimer, _maxIntensity;
    private Vector3 _candleVFXStartScale, _candleWickStartPos;

    private void Start()
    {

        _candleLifeTimer = CandleLifeTime;
        _maxIntensity = _light.intensity;
        _candleVFXStartScale = CandleVFX.localScale;
        _candleWickStartPos = _candleWick.localPosition;

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
            CandleVFX.localScale = new Vector3(_candleVFXStartScale.x, _candleVFXStartScale.y * Mathf.Lerp(1f, MinCandleHeight, 1f - (_candleLifeTimer / CandleLifeTime)), _candleVFXStartScale.z);
            _candleWick.localPosition = new Vector3(_candleWickStartPos.x, _candleWickStartPos.y * Mathf.Lerp(1f, MinCandleHeight, 1f - (_candleLifeTimer / CandleLifeTime)), _candleWickStartPos.z);

        }

        if(_candleLifeTimer <= LightDimmingThreshold)
        {

            _light.intensity = Mathf.Lerp(_maxIntensity, MinIntensity, 1f - (_candleLifeTimer / LightDimmingThreshold));

        }

    }

    public void GetLitFam()
    {

        IsLitFam = true;
        _candleFX.gameObject.SetActive(true);
        
    }

    public void GetQuenchedFam()
    {

        IsLitFam = false;
        _candleFX.gameObject.SetActive(false);

    }

}
