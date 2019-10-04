using UnityEngine;

// Add this interface to the prefab you want to spawn by ObjectPooler
public interface IPooledObject
{
    // Called after spawn by ObjectPooler
    void OnObjectSpawn();
}
