using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmashOnCollision : MonoBehaviour
{

    public void Smash()
    {
        StartCoroutine(SplitMesh(true));
    }


    public IEnumerator SplitMesh(bool destroy)
    {

        //Debug.Log("SPLIT");
        //Debug.Log("Glass Sound");

        if (GetComponent<MeshFilter>() == null || GetComponent<SkinnedMeshRenderer>() == null)
        {
            yield return null;
        }

        if (GetComponent<Collider>())
        {
            GetComponent<Collider>().enabled = false;
        }

        Mesh M = new Mesh();
        if (GetComponent<MeshFilter>())
        {
            Debug.Log("FOUND");
            M = GetComponent<MeshFilter>().mesh;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            M = GetComponent<SkinnedMeshRenderer>().sharedMesh;
        }

        Material[] materials = new Material[0];
        if (GetComponent<MeshRenderer>())
        {
            materials = GetComponent<MeshRenderer>().materials;
        }
        else if (GetComponent<SkinnedMeshRenderer>())
        {
            materials = GetComponent<SkinnedMeshRenderer>().materials;
        }

        Vector3[] verts = M.vertices;
        Vector3[] normals = M.normals;
        Vector2[] uvs = M.uv;
        for (int submesh = 0; submesh < M.subMeshCount; submesh++)
        {

            int[] indices = M.GetTriangles(submesh);
            print("Num indices: " + indices.Length);
            for (int i = 0; i < indices.Length; i += 12) // i += x -> x should be a multiple of 3 or there could be wierd triangles
            // i += x -> x = 3 normal amt of triangles, x = 6 half amt of triangles, x = 9 one third amt of triangles, etc
            {
                Vector3[] newVerts = new Vector3[3];
                Vector3[] newNormals = new Vector3[3];
                Vector2[] newUvs = new Vector2[3];
                for (int n = 0; n < 3; n++)
                {
                    int index = indices[i + n];
                    newVerts[n] = verts[index];
                    newUvs[n] = uvs[index];
                    newNormals[n] = normals[index];
                }

                Mesh mesh = new Mesh();
                mesh.vertices = newVerts;
                mesh.normals = newNormals;
                mesh.uv = newUvs;

                mesh.triangles = new int[] { 0, 1, 2, 2, 1, 0 };

                GameObject GO = new GameObject("Triangle " + (i / 3));
                //GO.layer = LayerMask.NameToLayer("Particle");
                GO.transform.position = transform.position;
                GO.transform.rotation = transform.rotation;
                GO.AddComponent<MeshRenderer>().material = materials[submesh];
                GO.AddComponent<MeshFilter>().mesh = mesh;
                GO.AddComponent<BoxCollider>();
                //GO.GetComponent<BoxCollider>().isTrigger = true;
                Vector3 explosionPos = new Vector3(transform.position.x + Random.Range(-0.5f, 0.5f), transform.position.y + Random.Range(0f, 0.5f), transform.position.z + Random.Range(-0.5f, 0.5f));
                GO.AddComponent<Rigidbody>().AddExplosionForce(Random.Range(500, 1000), explosionPos, 1) ;
                Destroy(GO, Random.Range(1.0f, 3.0f));
            }
        }

        GetComponent<Renderer>().enabled = false;

        yield return new WaitForSeconds(0.1f);
        if (destroy == true)
        {
            Destroy(gameObject);
        }

    }
}
