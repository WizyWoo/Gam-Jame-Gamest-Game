using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelPieceController : MonoBehaviour
{

    public List<Vector3> Available;
    public GameObject ZPWall, ZNWall, XPWall, XNWall;
    public int MaxBranches;
    public List<TunnelPieceController> ConnectedRooms;
    public List<Transform> CandleSpawnPoints;
    public int MaxCandles, MinCandles;
    public GameObject Candle;

    private void Awake()
    {

        int x = Random.Range(MinCandles, MaxCandles);

        for(int i = 0; i < x; i++)
        {

            int y = Random.Range(0, CandleSpawnPoints.Count);

            Instantiate(Candle, CandleSpawnPoints[y].position, Quaternion.identity);
            CandleSpawnPoints.RemoveAt(y);

        }

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
