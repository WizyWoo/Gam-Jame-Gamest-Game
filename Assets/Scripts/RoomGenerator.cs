using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using Unity.IO.LowLevel.Unsafe;
using UnityEditor.PackageManager;
using UnityEngine;
using UnityEngine.AI;

public class RoomGenerator : MonoBehaviour
{

    public List<GameObject> RoomPieces;
    public List<int> MaxSpawnsAllowed;
    [SerializeField]
    private TunnelPieceController _startPiece;
    private struct _roomLoc
    {

        public Vector2 Location;
        public TunnelPieceController PieceController;

    }
    private List<_roomLoc> _rooms;
    [SerializeField]
    private List<TunnelPieceController> _unProcessedRooms;
    [SerializeField]
    private List<TunnelPieceController> _generatedRooms;

    private void Awake()
    {

        _rooms = new List<_roomLoc>();
        _unProcessedRooms = new List<TunnelPieceController>();
        _generatedRooms = new List<TunnelPieceController>();
        _unProcessedRooms.Add(_startPiece);
        _generatedRooms.Add(_startPiece);
        _rooms.Add(new _roomLoc() { Location = new Vector2((int)_startPiece.transform.position.x, (int)_startPiece.transform.position.z), PieceController = _startPiece });

        int max = 50, x = 0;
        bool allRoomsProcessed = false;
        while(!allRoomsProcessed)
        {

            if(x > max)
            {

                Debug.LogError("Max rooms reached");
                break;

            }
            x++;

            if(RoomPieces.Count == 0)
            {

                allRoomsProcessed = true;
                break;

            }

            ProcessRoom(_unProcessedRooms[0]);

            if(_unProcessedRooms.Count == 0)
            {

                allRoomsProcessed = true;
                break;

            }

        }

        List<GameObject> floors = new List<GameObject>();

        foreach(TunnelPieceController room in _generatedRooms)
        {

            floors.Add(room.gameObject.transform.GetChild(0).gameObject);

        }

        GameObject navmeshObj = new GameObject("NavMesh");

        foreach(GameObject floor in floors)
        {

            floor.transform.parent = navmeshObj.transform;

        }

        NavMeshSurface navMesh = navmeshObj.AddComponent<NavMeshSurface>();
        navmeshObj.transform.position = Vector3.up * 100 + Vector3.right * 100;
        navMesh.BuildNavMesh();
        navmeshObj.transform.position = Vector3.zero;


    }

    private void ProcessRoom(TunnelPieceController room)
    {

        if(room.Available.Count == 0)
        {

            _unProcessedRooms.Remove(room);
            return;

        }

        int x = room.MaxBranches;

        for(int i = 0; i < x; i++)
        {

            if(room.Available.Count == 0)
            {

                break;

            }

            if(RoomPieces.Count == 0)
            {

                break;

            }

            int y = Random.Range(0, room.Available.Count);

            Vector3 testDir = room.Available[y];
            ModAvailableDir(ref testDir, room);

            if(CheckLoc(room.transform.position + (testDir * 15)))
            {

                continue;

            }
            GenerateRoom(room, room.Available[y]);
            room.RemoveWall(room.Available[y]);

        }

        _unProcessedRooms.Remove(room);

    }

    private bool CheckLoc(Vector3 location)
    {

        foreach(_roomLoc room in _rooms)
        {

            if(room.Location == new Vector2((int)location.x, (int)location.z))
            {

                return true;

            }

        }

        return false;

    }

    private void GenerateRoom(TunnelPieceController currentRoom, Vector3 availableDir)
    {

        bool roomSelected = false;
        GameObject selectedRoomType = null;
        while(!roomSelected)
        {

            int roomIndex = Random.Range(0, RoomPieces.Count);

            if(MaxSpawnsAllowed[roomIndex] > 0)
            {

                selectedRoomType = RoomPieces[roomIndex];
                MaxSpawnsAllowed[roomIndex]--;
                roomSelected = true;

                if(MaxSpawnsAllowed[roomIndex] == 0)
                {

                    RoomPieces.RemoveAt(roomIndex);
                    MaxSpawnsAllowed.RemoveAt(roomIndex);

                }

            }

        }

        ModAvailableDir(ref availableDir, currentRoom);

        TunnelPieceController newRoom = Instantiate(selectedRoomType, currentRoom.transform.position + (availableDir * 15), Quaternion.identity).GetComponent<TunnelPieceController>();
        newRoom.transform.LookAt(currentRoom.transform.position);

        newRoom.RemoveWall(new Vector3(0,0,1));

        currentRoom.ConnectedRooms.Add(newRoom);

        _rooms.Add(new _roomLoc() { Location = new Vector2((int)newRoom.transform.position.x, (int)newRoom.transform.position.z), PieceController = newRoom });
        _unProcessedRooms.Add(newRoom);
        _generatedRooms.Add(newRoom);

    }

    private void ModAvailableDir(ref Vector3 availableDir, TunnelPieceController currentRoom)
    {
        //Debug.Log("Before " + availableDir);
        if(availableDir == Vector3.forward)
        {

            availableDir = currentRoom.transform.forward;

        }
        else if(availableDir == Vector3.back)
        {

            availableDir = -currentRoom.transform.forward;

        }
        else if(availableDir == Vector3.right)
        {

            availableDir = currentRoom.transform.right;

        }
        else if(availableDir == Vector3.left)
        {

            availableDir = -currentRoom.transform.right;

        }
        //Debug.Log("After " + availableDir);
    }

}
