using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPieceController : MonoBehaviour
{

    public List<Vector3> Available;
    public List<TunnelPieceController> ConnectedTunnels;
    public GameObject ZPWall, ZNWall, XPWall, XNWall;
    public bool Used, Triggered;

    private void Awake()
    {

        Available = new List<Vector3>()
        {
                
            Vector3.forward,
            Vector3.back,
            Vector3.right,
            Vector3.left
    
        };

    }

    public void RemoveWall(Vector3 direction)
    {

        if (direction == Vector3.forward)
        {

            Destroy(ZPWall);

        }
        else if (direction == Vector3.back)
        {

            Destroy(ZNWall);

        }
        else if (direction == Vector3.right)
        {

            Destroy(XPWall);

        }
        else if (direction == Vector3.left)
        {

            Destroy(XNWall);

        }
        
        Available.Remove(direction);

    }

    public void TriggerGeneration(Vector3 forceDirection, int forwardBy = 0)
    {

        TunnelGenerator.INSTANCE.GenerateTunnelFrom(this, forceDirection, forwardBy);
        Used = true;

    }

    private void OnTriggerEnter(Collider other)
    {

        if (!Triggered && other.tag == "Player")
        {

            Triggered = true;
            Destroy(gameObject.GetComponent<BoxCollider>());

            for(int i = 0; i < ConnectedTunnels.Count; i++)
            {

                if(ConnectedTunnels[i].Used == false)
                {

                    ConnectedTunnels[i].TriggerGeneration(Vector3.zero);

                }

            }

        }

    }

}
