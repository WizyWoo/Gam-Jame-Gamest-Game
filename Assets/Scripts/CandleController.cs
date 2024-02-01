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
    [SerializeField]
    private ParticleSystem _candleGlowFX;
    private float _candleLifeTimer, _maxIntensity, _currentIntensity, _lerpIntensity, _lerpTimer;
    private Vector3 _candleVFXStartScale, _candleWickStartPos;

    private void Start()
    {

        _candleLifeTimer = CandleLifeTime;
        _maxIntensity = _light.intensity;
        _candleVFXStartScale = CandleVFX.localScale;
        _candleWickStartPos = _candleWick.localPosition;

        if(IsLitFam)
            GetLitFam();

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

            _currentIntensity = Mathf.Lerp(_maxIntensity, MinIntensity, 1f - (_candleLifeTimer / LightDimmingThreshold));

        if(_lerpTimer < 1)
        {

            _lerpTimer += Time.deltaTime * 1f;

            _light.intensity = Mathf.Lerp(0, _currentIntensity, _lerpTimer);
            if(_lerpTimer > 1)
                _lerpTimer = 1;

        }
        else
            _light.intensity = _currentIntensity;

    }

    public void GetLitFam()
    {

        IsLitFam = true;
        _candleFX.gameObject.SetActive(true);
        _lerpTimer = -0.6f;
        
    }

    public void GetQuenchedFam()
    {

        IsLitFam = false;
        _candleFX.gameObject.SetActive(false);

    }

    public void PickedUp()
    {

        Destroy(_candleGlowFX, 1f);
        gameObject.GetComponent<BoxCollider>().isTrigger = true;

    }

}
