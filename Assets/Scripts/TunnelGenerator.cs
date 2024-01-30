using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TunnelGenerator : MonoBehaviour
{

    public static TunnelGenerator INSTANCE;
    [SerializeField]
    private GameObject TunnelBridge, TunnelMiddle;
    [SerializeField]
    private TunnelPieceController _startPiece;
    private struct _tunnelPieceLoc
    {

        public Vector2 Location;
        public TunnelPieceController PieceController;

    }
    private List<_tunnelPieceLoc> _tunnelPieces;

    private void Awake()
    {

        INSTANCE = this;

        _tunnelPieces = new List<_tunnelPieceLoc>();
        _startPiece.TriggerGeneration(Vector3.forward, 10);

    }

    public void GenerateTunnelFrom(TunnelPieceController piece, Vector3 forceDirection, int forwardBy = 0)
    {

        if(piece.Available.Count == 0)
        {

            return;

        }

        if(forceDirection != Vector3.zero && piece.Available.Contains(forceDirection))
        {

            GenerateTunnel(piece, forceDirection);
            return;

        }

        int x = Random.Range(0, 6);

        if(x == 5)
            x = 2;
        else
            x = 1;

        for(int i = 0; i < x; i++)
        {

            GenerateTunnel(piece, piece.Available[Random.Range(0, piece.Available.Count)]);

        }

    }

    private TunnelPieceController GenerateTunnel(TunnelPieceController piece, Vector3 direction)
    {

        Vector3 position = piece.transform.position + direction * 20;

        if(TryGetPieceAt(position, out TunnelPieceController existingPiece))
        {

            if(existingPiece.Triggered)
            {

                return null;

            }

            piece.ConnectedTunnels.Add(existingPiece);
            existingPiece.ConnectedTunnels.Add(piece);

            Instantiate(TunnelBridge, piece.transform.position, Quaternion.LookRotation(direction));

            piece.RemoveWall(direction);
            existingPiece.RemoveWall(direction * -1);

            return existingPiece;

        }

        TunnelPieceController newPiece = Instantiate(TunnelMiddle, position, Quaternion.identity).GetComponent<TunnelPieceController>();

        Instantiate(TunnelBridge, piece.transform.position, Quaternion.LookRotation(direction));

        piece.RemoveWall(direction);
        piece.ConnectedTunnels.Add(newPiece);
        newPiece.RemoveWall(direction * -1);

        _tunnelPieces.Add(new _tunnelPieceLoc() { Location = new Vector2((int)position.x, (int)position.z), PieceController = newPiece });

        return newPiece;

    }

    private bool TryGetPieceAt(Vector3 position, out TunnelPieceController piece)
    {

        for(int i = 0; i < _tunnelPieces.Count; i++)
        {

            if(_tunnelPieces[i].Location == new Vector2((int)position.x, (int)position.z))
            {

                piece = _tunnelPieces[i].PieceController;
                return true;

            }

        }

        piece = null;
        return false;

    }

}
