using UnityEngine;
using UnityEngine.Rendering;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class CubeField : MonoBehaviour {

    [SerializeField] private Vector2Int size;
    [SerializeField] private int seed;
    [SerializeField, Range(0, 10)] private float randomness;

    [SerializeField] private Material material;

    private System.Random random;

    GameObject[] cubes = null;

    public void Generate() {

        random = new(seed);

        var meshFilter = GetComponent<MeshFilter>();
        meshFilter.sharedMesh.Clear();

        if (cubes != null) {
            foreach (var c in cubes)
                DestroyImmediate(c);
            cubes = null;
        }

        cubes = new GameObject[size.x * size.y];

        for (int i = 0; i < cubes.Length; i++) {
            var cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
            cube.transform.parent = transform;

            var randY = 1 + random.Next(0, 10) / 10.0f * randomness;

            var position = new Vector3(i % size.x, randY / 2.0f, i / size.x) + transform.position;
            var scale = new Vector3(1.0f, randY, 1.0f);
            cube.transform.localScale = scale;
            cube.transform.position = position;

            cube.name = i.ToString();

            cubes[i] = cube;
        }

        CombineMeshes();
    }

    public void CombineMeshes() {
        if (cubes == null) return;

        var meshFilters = GetComponentsInChildren<MeshFilter>();
        var combine = new CombineInstance[meshFilters.Length];

        for (int i = 0; i < meshFilters.Length; i++) {
            combine[i] = new CombineInstance() {
                mesh = meshFilters[i].sharedMesh,
                transform = meshFilters[i].transform.localToWorldMatrix
            };

            meshFilters[i].gameObject.SetActive(false);
        }

        Mesh combinedMesh = new Mesh();
        combinedMesh.indexFormat = IndexFormat.UInt32;
        combinedMesh.CombineMeshes(combine);
        var meshFilter = gameObject.GetComponent<MeshFilter>();
        meshFilter.sharedMesh = combinedMesh;
        //meshFilter.sharedMesh.indexFormat = IndexFormat.UInt32;

        var renderer = GetComponent<MeshRenderer>();
        renderer.material = material;

        foreach (var c in cubes)
            DestroyImmediate(c);

/*        var col = GetComponent<MeshCollider>();
        if (col != null)
            DestroyImmediate(col);

        var newCol = gameObject.AddComponent<MeshCollider>();
        newCol.sharedMesh = combinedMesh;*/

        gameObject.SetActive(true);
    }

    void Start() {

    }

    void Update() {

    }
}
