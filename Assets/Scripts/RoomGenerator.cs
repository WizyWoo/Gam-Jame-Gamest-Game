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
    private GameObject _ghost, _portal;
    [SerializeField]
    private TunnelPieceController _startPiece;
    [SerializeField]
    private List<Vector2> _rooms;
    [SerializeField]
    private List<TunnelPieceController> _unProcessedRooms;
    [SerializeField]
    private List<TunnelPieceController> _generatedRooms;
    private List<TunnelPieceController> _portalRooms;

    private void Awake()
    {

        _rooms = new List<Vector2>();
        _unProcessedRooms = new List<TunnelPieceController>();
        _generatedRooms = new List<TunnelPieceController>();
        _unProcessedRooms.Add(_startPiece);
        _generatedRooms.Add(_startPiece);
        _rooms.Add(new Vector2((int)_startPiece.transform.position.x, (int)_startPiece.transform.position.z));

        bool allRoomsProcessed = false;
        while(!allRoomsProcessed)
        {

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

        _portalRooms = _generatedRooms;

        int portalsSpawned = 0;
        bool allPortalsSpawned = false;

        for(int i = 0; i < _portalRooms.Count; i++)
        {

            if(_portalRooms[i].ConnectedRooms.Count == 0 && _portalRooms[i].PortalPos != null)
            {

                Instantiate(_portal, _portalRooms[i].PortalPos.position, Quaternion.identity);
                _portalRooms.RemoveAt(i);
                portalsSpawned++;

            }

            if(portalsSpawned == 4)
            {

                allPortalsSpawned = true;
                break;

            }

        }

        while(!allPortalsSpawned)
        {

            int x = Random.Range(0, _portalRooms.Count);

            if(portalsSpawned == 4)
                break;

            if(Vector3.Distance(_portalRooms[x].transform.position, Vector3.zero) > 15 && _portalRooms[x].PortalPos != null)
            {

                Instantiate(_portal, _portalRooms[x].PortalPos.position, Quaternion.identity);
                _portalRooms.RemoveAt(x);
                portalsSpawned++;

            }

            if(portalsSpawned == 4)
                break;

        }

        Instantiate(_ghost, _generatedRooms[_generatedRooms.Count - 1].transform.position + Vector3.up * 2, Quaternion.identity);

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

            Debug.Log("On Check Dir " + testDir);

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

        Debug.Log("inside the Check Dir " + location);
        foreach(Vector2 room in _rooms)
        {
            
            if(Mathf.RoundToInt(room.x) == Mathf.RoundToInt(location.x) && Mathf.RoundToInt(room.y) == Mathf.RoundToInt(location.z))
            {

                Debug.LogError("Room already exists at " + location);
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

        Debug.Log("Rooms Spawn loc " + newRoom.transform.position);
        newRoom.RemoveWall(new Vector3(0,0,1));

        currentRoom.ConnectedRooms.Add(newRoom);

        _rooms.Add(new Vector2(Mathf.RoundToInt(newRoom.transform.position.x), Mathf.RoundToInt(newRoom.transform.position.z)));
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
