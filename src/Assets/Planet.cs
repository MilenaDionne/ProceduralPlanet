using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Planet : MonoBehaviour {

    [Range(2,256)]
    public int resolution = 10;

    [SerializeField, HideInInspector]
    MeshFilter[] meshFilters;
    TerrainFace[] terrainFaces;

    [Tooltip("Spin: Yes or No")]
    public bool spin;
    [Tooltip("Spin the parent object instead of the object this script is attached to")]
    public bool spinParent;
    public float speed = 10f;

    [HideInInspector]
    public bool clockwise = true;
    [HideInInspector]
    public float direction = 1f;
    [HideInInspector]
    public float directionChangeSpeed = 2f;

    // Update is called once per frame
    void Update()
    {
        if (direction < 1f)
        {
            direction += Time.deltaTime / (directionChangeSpeed / 2);
        }

        if (spin)
        {
            if (clockwise)
            {
                if (spinParent)
                    transform.parent.transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
                else
                    transform.Rotate(Vector3.up, (speed * direction) * Time.deltaTime);
            }
            else
            {
                if (spinParent)
                    transform.parent.transform.Rotate(-Vector3.up, (speed * direction) * Time.deltaTime);
                else
                    transform.Rotate(-Vector3.up, (speed * direction) * Time.deltaTime);
            }
        }
    }

    private void OnValidate()
	{
        Initialize();
        GenerateMesh();
	}

	void Initialize()
    {
        if (meshFilters == null || meshFilters.Length == 0)
        {
            meshFilters = new MeshFilter[6];
        }
        terrainFaces = new TerrainFace[6];

        Vector3[] directions = { Vector3.up, Vector3.down, Vector3.left, Vector3.right, Vector3.forward, Vector3.back };

        for (int i = 0; i < 6; i++)
        {
            if (meshFilters[i] == null)
            {
                GameObject meshObj = new GameObject("mesh");
                meshObj.transform.parent = transform;

                meshObj.AddComponent<MeshRenderer>().sharedMaterial = new Material(Shader.Find("Standard"));
                meshFilters[i] = meshObj.AddComponent<MeshFilter>();
                meshFilters[i].sharedMesh = new Mesh();
            }

            terrainFaces[i] = new TerrainFace(meshFilters[i].sharedMesh, resolution, directions[i]);
        }
    }

    void GenerateMesh()
    {
        foreach (TerrainFace face in terrainFaces)
        {
            face.ConstructMesh();
        }
    }
}
