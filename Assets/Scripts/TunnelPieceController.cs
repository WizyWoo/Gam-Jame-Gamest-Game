using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPieceController : MonoBehaviour
{

    public List<Vector3> Available;
    public GameObject ZPWall, ZNWall, XPWall, XNWall;
    public int MaxBranches;

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

}
