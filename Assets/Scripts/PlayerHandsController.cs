using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{

    public Transform CandleHolder, Candle;
    [SerializeField]
    private float _placeDistance;
    [SerializeField]
    private GameObject _candleErrorPrefab, _diaphragm;
    [SerializeField]
    private ParticleSystem _blowEffect, _diaphragmEffect;
    [SerializeField]
    private Vector3 _placeCheckOffset, _safetyBox;
    private int _blows;
    private float _blowTimer;

    

    private void Update()
    {

        if(_blowTimer > 0)
        {

            _blowTimer -= Time.deltaTime;
            if(_blowTimer <= 0)
            {

                _blowTimer = 0;
                _blows = 0;

            }

        }

        if(Input.GetKeyDown(KeyCode.E))
        {

            RaycastHit hit;
            if(Candle)
            {

                if(Physics.Raycast(transform.position, transform.forward, out hit, _placeDistance, LayerMask.GetMask("Surface")))
                {

                    if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Surface"))
                    {

                        Collider[] colliders = Physics.OverlapBox(hit.point + _placeCheckOffset, _safetyBox, transform.root.rotation);

                        if(colliders.Length == 0)
                        {

                            Candle.parent = null;
                            Candle.position = hit.point;
                            Candle.rotation = transform.root.rotation;
                            Candle = null;

                        }
                        else
                        {

                            Destroy(Instantiate(_candleErrorPrefab, hit.point, transform.root.rotation), 1.2f);
                            Debug.Log("hit" + colliders[0].name);

                        }

                    }

                }

                return;

            }

            if(Physics.Raycast(transform.position, transform.forward, out hit, _placeDistance, LayerMask.GetMask("Candle")))
            {

                if(hit.collider.gameObject.layer == LayerMask.NameToLayer("Candle"))
                {

                    Candle = hit.collider.transform;
                    Candle.parent = CandleHolder;
                    Candle.localPosition = Vector3.zero;
                    Candle.localRotation = Quaternion.identity;
                    Candle.GetComponent<CandleController>().PickedUp();

                }

            }

        }

        if(Input.GetKeyDown(KeyCode.B))
        {

            _blowEffect.Play();

            if(Candle)
            {

                Candle.GetComponent<CandleController>().GetQuenchedFam();

            }

            _blowTimer = 0.5f;
            _blows++;

            if(_blows >= 5)
            {

                _blows = 0;
                Instantiate(_diaphragm, transform.position + (transform.forward * 0.4f) + (transform.up * 0.2f), transform.rotation).GetComponent<Rigidbody>().velocity = transform.forward * 10;
                _diaphragmEffect.Play();

            }

        }

        if(Input.GetKeyDown(KeyCode.F))
        {

            if(Candle)
            {

                if(!Candle.GetComponent<CandleController>().IsLitFam)
                    Candle.GetComponent<CandleController>().GetLitFam();
                else if(Candle.GetComponent<CandleController>().IsLitFam)
                {

                    _blowEffect.Play();
                    SoundManager.instance.PlaySound(1);
                    Candle.GetComponent<CandleController>().GetQuenchedFam();

                    _blowTimer = 0.5f;
                    _blows++;

                    if(_blows >= 5)
                    {

                        _blows = 0;
                        Instantiate(_diaphragm, transform.position + (transform.forward * 0.4f) + (transform.up * 0.2f), transform.rotation).GetComponent<Rigidbody>().velocity = transform.forward * 10;
                        _diaphragmEffect.Play();

                    }

                }

            }

        }

    }

}
