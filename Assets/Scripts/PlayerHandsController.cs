using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHandsController : MonoBehaviour
{

    public Transform CandleHolder, Candle;
    [SerializeField]
    private float _placeDistance;
    [SerializeField]
    private GameObject _candleErrorPrefab;
    [SerializeField]
    private Vector3 _placeCheckOffset, _safetyBox;

    private void Update()
    {

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
                    Candle.GetComponent<CandleController>().GetLitFam();

                }

            }

        }

    }

}
