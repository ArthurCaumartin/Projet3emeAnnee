using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Serialization;
using Random = UnityEngine.Random;

public class SpawnMobileMob : MonoBehaviour
{
    public Transform _playerPos;
    public Camera _camera;
    public Mob _mobPrefab;

    [Tooltip("When spawning an enemy, the max distance to have a NavMesh close enough to spawn it")]
    public float _maxDistanceNavMeshSpawn = 2f;

    [Tooltip("Timer between spawn of enemies")]
    public float _cooldownSpawn = 1f;

    [Tooltip("Distance between camera limit and outer vision to spawn enemies")]
    public float _distanceSpawnMobs = 10f;

    [Tooltip("Range to despawn enemis if too far")]
    public float _despawnRadius = 70f;

    private float _cooldown;
    private bool _canSpawnMobs = true;
    private List<GameObject> _allMobs = new List<GameObject>();

    private void Update()
    {
        if (!_canSpawnMobs) return;

        _cooldown += Time.deltaTime;

        if (_cooldown >= _cooldownSpawn)
        {
            DeleteFarMobs();
            CameraSpawn();
            _cooldown = 0;
        }

    }

    //Function that get a random position outside of player's view and few meters
    private void GetRandomPos(Vector3 bottomLeft, Vector3 topLeft, Vector3 topRight, Vector3 bottomRight)
    {
        Vector3 bottomLeftMax = new Vector3(bottomLeft.x + _distanceSpawnMobs, 0, bottomLeft.z - _distanceSpawnMobs);
        Vector3 topLeftMax = new Vector3(topLeft.x - _distanceSpawnMobs, 0, topLeft.z - _distanceSpawnMobs);
        Vector3 topRightMax = new Vector3(topRight.x - _distanceSpawnMobs, 0, topRight.z + _distanceSpawnMobs);
        Vector3 bottomRightMax = new Vector3(bottomRight.x + _distanceSpawnMobs, 0, bottomRight.z + _distanceSpawnMobs);

        float zPosSpawn = Random.Range(topLeftMax.z, topRightMax.z);
        float xPosSpawn = Random.Range(topLeftMax.x, bottomRightMax.x);

        while (!(
                  zPosSpawn <= topLeft.z ||
                  zPosSpawn >= topRight.z ||
                  xPosSpawn <= topLeft.x ||
                  xPosSpawn >= bottomRight.x
                  ))
        {
            zPosSpawn = Random.Range(bottomLeftMax.z, topRightMax.z);
            xPosSpawn = Random.Range(topLeftMax.x, bottomRightMax.x);
        }

        Vector3 mobSpawnPos = new Vector3(xPosSpawn, 0.75f, zPosSpawn);

        //Verify if the position is on a non NavMesh surface
        NavMeshHit hit;
        if (NavMesh.SamplePosition(mobSpawnPos, out hit, _maxDistanceNavMeshSpawn, NavMesh.AllAreas))
        {
            SpawnMob(mobSpawnPos);
        }
        else
        {
            GetRandomPos(bottomLeft, topLeft, topRight, bottomRight);
        }
    }

    //Delete all mobs that are too far from the player
    public void DeleteFarMobs()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_playerPos.position, _despawnRadius);

        List<GameObject> hitObjects = new List<GameObject>();

        for (int i = 0; i < hitColliders.Length; i++)
        {
            hitObjects.Add(hitColliders[i].gameObject);
        }

        for (int i = 0; i < _allMobs.Count; i++)
        {
            if (!hitObjects.Contains(_allMobs[i]))
            {
                GameObject tempGameobject = _allMobs[i];
                _allMobs.Remove(tempGameobject);
                Destroy(tempGameobject);
            }
        }
    }
    // private void OnDrawGizmos()
    // {
    //     Gizmos.color = Color.green;
    //     Gizmos.DrawSphere(_playerPos.position, _despawnRadius);
    // }

    public void Nuke()
    {
        _canSpawnMobs = false;

        for (int i = 0; i < _allMobs.Count; i++)
        {
            GameObject tempGameobject = _allMobs[i];
            Destroy(tempGameobject);
        }
        _allMobs.Clear();
    }

    public void DeleteAMob(GameObject mobToDelete)
    {
        _allMobs.Remove(mobToDelete);
        Destroy(mobToDelete);
    }

    private void SpawnMob(Vector3 spawnPoistion)
    {
        Mob intantiatedMob = Instantiate(_mobPrefab, spawnPoistion, Quaternion.identity, transform);
        intantiatedMob.Initialize(_playerPos);
        
        
        _allMobs.Add(intantiatedMob.gameObject);
    }

    //Raycast to all corners of the camera and get the max positions of field of player's view
    private void CameraSpawn()
    {
        Ray r1 = _camera.ScreenPointToRay(new Vector3(0, 0, 0));
        Ray r2 = _camera.ScreenPointToRay(new Vector3(0, _camera.pixelHeight, 0));
        Ray r3 = _camera.ScreenPointToRay(new Vector3(_camera.pixelWidth, _camera.pixelHeight, 0));
        Ray r4 = _camera.ScreenPointToRay(new Vector3(_camera.pixelWidth, 0, 0));

        Physics.Raycast(r1.origin, r1.direction, out RaycastHit hit1, LayerMask.GetMask("Ground"));
        Vector3 posHit1 = hit1.point;
        Physics.Raycast(r2.origin, r2.direction, out RaycastHit hit2, LayerMask.GetMask("Ground"));
        Vector3 posHit2 = hit2.point;
        Physics.Raycast(r3.origin, r3.direction, out RaycastHit hit3, LayerMask.GetMask("Ground"));
        Vector3 posHit3 = hit3.point;
        Physics.Raycast(r4.origin, r4.direction, out RaycastHit hit4, LayerMask.GetMask("Ground"));
        Vector3 posHit4 = hit4.point;

        GetRandomPos(posHit1, posHit2, posHit3, posHit4);

        // Debug.DrawRay(r1.origin, r1.direction * 100000, Color.red);
        // Debug.DrawRay(r2.origin, r2.direction * 100000, Color.red);
        // Debug.DrawRay(r3.origin, r3.direction * 100000, Color.red);
        // Debug.DrawRay(r4.origin, r4.direction * 100000, Color.red);
    }

}