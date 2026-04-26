using UnityEngine;

public class LinearSpawnPointStrategy : ISpawnPointStrategy {
    private int index = 0;
    Transform[] spawnPoints;

    public LinearSpawnPointStrategy(Transform[] spawnPoints) {
        this.spawnPoints = spawnPoints;
    }
    
    public Transform NextSpawnPoint() {
        Transform result = spawnPoints[index];
        index = (index++) % spawnPoints.Length;
        return result;
    }
}