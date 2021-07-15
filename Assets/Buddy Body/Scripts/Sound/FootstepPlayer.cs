using MyBox;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepPlayer : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private Rigidbody playerRB;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private bool useTerrain;
    [ConditionalField("useTerrain")] [SerializeField] private Terrain terrain;

    [Header("Audio")]
    [SerializeField] private AudioSource footstepSource;
    [SerializeField] private AudioClip[] walkingStoneClips;
    [SerializeField] private AudioClip[] walkingDirtClips;
    [SerializeField] private AudioClip[] walkingSandClips;
    [SerializeField] private AudioClip[] walkingGrassClips;
    [SerializeField] private AudioClip[] runningStoneClips;
    [SerializeField] private AudioClip[] runningDirtClips;
    [SerializeField] private AudioClip[] runningSandClips;
    [SerializeField] private AudioClip[] runningGrassClips;
    [SerializeField] private AudioClip[] walkingWoodBridgeClips;
    [SerializeField] private AudioClip[] runningWoodBridgeClips;
    [SerializeField] private AudioClip[] pavementWalkingClips;
    [SerializeField] private AudioClip[] pavementRunningClips;

    [Header("Debugger")]
    [ReadOnly] [SerializeField] private float[] textureValues;
    [ReadOnly] [SerializeField] Vector3 terrainPosition;
    [ReadOnly] [SerializeField] Vector3 mapPosition;
    [ReadOnly] [SerializeField] float xCoord;
    [ReadOnly] [SerializeField] float zCoord;
    [ReadOnly] [SerializeField] int posX;
    [ReadOnly] [SerializeField] int posZ;
    [ReadOnly] [SerializeField] AudioClip selectedClip;
    [ReadOnly] [SerializeField] AudioClip previousClip;

    private void OnEnable()
    {
        footstepSource.volume = GameManager.Instance.soundSystem.GetSetSFXVolume;
        GameManager.Instance.soundSystem.onSFXVolumeChange += ChangeVolume;
    }

    private void OnDisable()
    {
        GameManager.Instance.soundSystem.onSFXVolumeChange -= ChangeVolume;
    }

    private void ChangeVolume(object sender, EventArgs e)
    {
        footstepSource.volume = GameManager.Instance.soundSystem.GetSetSFXVolume;
    }

    public void GetTerrainTexture()
    {
        ConvertPosition(playerRB.position);
    }

    private void ConvertPosition(Vector3 playerPosition)
    {
        terrainPosition = playerPosition - terrain.transform.position;
        mapPosition = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0,
            terrainPosition.z / terrain.terrainData.size.z);

        xCoord = mapPosition.x * terrain.terrainData.alphamapWidth;
        zCoord = mapPosition.z * terrain.terrainData.alphamapHeight;

        posX = (int)xCoord;
        posZ = (int)zCoord;

        CheckTexture();
    }

    private void CheckTexture()
    {
        float[,,] aMap = terrain.terrainData.GetAlphamaps(posX, posZ, 1, 1);

        textureValues[0] = aMap[0, 0, 0];
        textureValues[1] = aMap[0, 0, 1];
        textureValues[2] = aMap[0, 0, 2];
        textureValues[3] = aMap[0, 0, 3];
    }


    public void PlayFootstep()
    {
        if (useTerrain)
            GetTerrainTexture();

        //  Walking
        if (GameManager.Instance.direction.magnitude > 0f && GameManager.Instance.direction.magnitude <= 0.5f)
        {
            if (playerMovement.TagGround == "TerrainGround")
            {
                if (textureValues[0] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(walkingDirtClips), textureValues[0]);
                }
                if (textureValues[1] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(walkingGrassClips), textureValues[1]);
                }
                if (textureValues[2] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(walkingDirtClips), textureValues[2]);
                }
                if (textureValues[3] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(walkingStoneClips), textureValues[3]);
                }
            }
            else if (playerMovement.TagGround == "WoodBridgeGround")
            {
                footstepSource.PlayOneShot(GetClip(walkingWoodBridgeClips));
            }
            else if (playerMovement.TagGround == "StoneGround")
            {
                footstepSource.PlayOneShot(GetClip(walkingStoneClips));
            }
            else if (playerMovement.TagGround == "DirtGround")
            {
                footstepSource.PlayOneShot(GetClip(walkingDirtClips));
            }
            else if (playerMovement.TagGround == "GrassGround")
            {
                footstepSource.PlayOneShot(GetClip(walkingGrassClips));
            }
        }
        //  Running
        else if (GameManager.Instance.direction.magnitude > 0.5f && GameManager.Instance.direction.magnitude <= 1.5f)
        {
            if (playerMovement.TagGround == "TerrainGround")
            {
                if (textureValues[0] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(runningDirtClips), textureValues[0]);
                }
                if (textureValues[1] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(runningGrassClips), textureValues[1]);
                }
                if (textureValues[2] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(runningDirtClips), textureValues[2]);
                }
                if (textureValues[3] > 0)
                {
                    footstepSource.PlayOneShot(GetClip(runningStoneClips), textureValues[3]);
                }
            }
            else if (playerMovement.TagGround == "WoodBridgeGround")
            {
                footstepSource.PlayOneShot(GetClip(runningWoodBridgeClips));
            }
            else if (playerMovement.TagGround == "StoneGround")
            {
                footstepSource.PlayOneShot(GetClip(runningStoneClips));
            }
            else if (playerMovement.TagGround == "DirtGround")
            {
                footstepSource.PlayOneShot(GetClip(runningDirtClips));
            }
            else if (playerMovement.TagGround == "GrassGround")
            {
                footstepSource.PlayOneShot(GetClip(runningGrassClips));
            }
        }
    }

    AudioClip GetClip(AudioClip[] clipArray)
    {
        int attempts = 3;
        selectedClip = clipArray[UnityEngine.Random.Range(0, clipArray.Length - 1)];

        while (selectedClip == previousClip && attempts > 0)
        {
            selectedClip =
            clipArray[UnityEngine.Random.Range(0, clipArray.Length - 1)];

            attempts--;
        }

        previousClip = selectedClip;
        return selectedClip;
    }
}
