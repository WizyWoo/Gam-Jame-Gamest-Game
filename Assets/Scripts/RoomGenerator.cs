using System.Collections;
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEngine;

public class RoomGenerator : MonoBehaviour
{

    public GameObject[] RoomPieces;
    public int[] MaxSpawnsAllowed;
    [SerializeField]
    private TunnelPieceController _startPiece;

}
