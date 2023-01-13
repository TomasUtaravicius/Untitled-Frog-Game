using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class CheckTerrainTexture : MonoBehaviour
{
    public Transform characterTransform;
    public Terrain t;
    public int posX;
    public int posZ;
    public float[] textureValues;
    public bool movingOnTerrain;
    void Start()
    {
        t = Terrain.activeTerrain;
        characterTransform = gameObject.transform;
    }
    void Update()
    {
        // For better performance, move this out of update 
        // and only call it when you need a footstep.
        GetTerrainTexture();
    }
    public void GetTerrainTexture()
    {
        ConvertPosition(characterTransform.position);
        CheckTexture();
    }
    void ConvertPosition(Vector3 playerPosition)
    {
        Vector3 terrainPosition = playerPosition - t.transform.position;
        
        Vector3 mapPosition = new Vector3
        (terrainPosition.x / t.terrainData.size.x, terrainPosition.y/ t.terrainData.size.y,
        terrainPosition.z / t.terrainData.size.z);
        //Debug.Log("Character above ground: " + (playerPosition.y - Terrain.activeTerrain.SampleHeight(transform.position)));
        if(playerPosition.y - Terrain.activeTerrain.SampleHeight(transform.position)>0.05f)
        {
            movingOnTerrain = false;
        }
        else
        {
            movingOnTerrain = true;
        }
        float xCoord = mapPosition.x * t.terrainData.alphamapWidth;
        float zCoord = mapPosition.z * t.terrainData.alphamapHeight;
        posX = (int)xCoord;
        posZ = (int)zCoord;
    }
    void CheckTexture()
    {
        float[,,] aMap = t.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        textureValues[0] = aMap[0, 0, 0];
        textureValues[1] = aMap[0, 0, 1];
        textureValues[2] = aMap[0, 0, 2];
        textureValues[3] = aMap[0, 0, 3];
    }
    private void OnCollisionStay(Collision collision)
    {
        Debug.Log("Walking on: " + collision.gameObject.name);
    }
    private void OnTriggerStay(Collider other)
    {
        Debug.Log("Walking on: " + other.gameObject.name);
    }
}