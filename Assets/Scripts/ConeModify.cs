using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConeModify : MonoBehaviour
{

#pragma warning
    [Header("Fill Settings")]
    [SerializeField]
    private float bottomRadiusSize = .16f;
    [SerializeField]
    private float topRadiusSize = .2f;
    [SerializeField]
    private float coneHeight = .65f;
    [SerializeField]
    private int numIncreases = 1000;
    [SerializeField]
    private Shader shader;
    [SerializeField]
    private int alphaSetting = 150;

    private Texture texture;
    private Color color;

    private float midRadiusSize;
    private float midRadiusIncrease;
    private float midHeight;
    private float midHeightIncrease;
    private float increasesSoFar;

    public float maxAlpha = .8f;
    public float alphaStep = .05f;

    public PourFromCup pourScript;

    private Material scriptedMaterial;

    private MeshRenderer rend;

    private void Start()
    {
        midRadiusSize = bottomRadiusSize;
        rend = GetComponent<MeshRenderer>();
        rend.material = new Material(shader);
        rend.material.color = new Color(0, 0, 0, alphaSetting);
        rend.material.SetFloat("_Mode", 3);
        rend.material.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.One);
        rend.material.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        rend.material.SetInt("_ZWrite", 0);
        rend.material.DisableKeyword("_ALPHATEST_ON");
        rend.material.DisableKeyword("_ALPHABLEND_ON");
        rend.material.EnableKeyword("_ALPHAPREMULTIPLY_ON");
        rend.material.renderQueue = 2999;
    }

    private void Update()
    {
        rend.material.SetFloat("_Mode", 3);
        //print(Time.deltaTime);
    }

    private void ModifyCone()
    {
        midRadiusIncrease = topRadiusSize / numIncreases;
        midHeightIncrease = coneHeight / numIncreases;

        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        Mesh mesh = filter.mesh;
        mesh.Clear();

        float height = midHeight + midHeightIncrease;
        float bottomRadius = bottomRadiusSize;
        float topRadius = midRadiusSize + midRadiusIncrease;
        int nbSides = 18;
        int nbHeightSeg = 1; // Not implemented yet

        int nbVerticesCap = nbSides + 1;
        #region Vertices

        // bottom + top + sides
        Vector3[] vertices = new Vector3[nbVerticesCap + nbVerticesCap + nbSides * nbHeightSeg * 2 + 2];
        int vert = 0;
        float _2pi = Mathf.PI * 2f;

        // Bottom cap
        vertices[vert++] = new Vector3(0f, 0f, 0f);
        while (vert <= nbSides)
        {
            float rad = (float)vert / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0f, Mathf.Sin(rad) * bottomRadius);
            vert++;
        }

        // Top cap
        vertices[vert++] = new Vector3(0f, height, 0f);
        while (vert <= nbSides * 2 + 1)
        {
            float rad = (float)(vert - nbSides - 1) / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
            vert++;
        }

        // Sides
        int v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            vertices[vert] = new Vector3(Mathf.Cos(rad) * topRadius, height, Mathf.Sin(rad) * topRadius);
            vertices[vert + 1] = new Vector3(Mathf.Cos(rad) * bottomRadius, 0, Mathf.Sin(rad) * bottomRadius);
            vert += 2;
            v++;
        }
        vertices[vert] = vertices[nbSides * 2 + 2];
        vertices[vert + 1] = vertices[nbSides * 2 + 3];
        #endregion

        #region Normales

        // bottom + top + sides
        Vector3[] normales = new Vector3[vertices.Length];
        vert = 0;

        // Bottom cap
        while (vert <= nbSides)
        {
            normales[vert++] = Vector3.down;
        }

        // Top cap
        while (vert <= nbSides * 2 + 1)
        {
            normales[vert++] = Vector3.up;
        }

        // Sides
        v = 0;
        while (vert <= vertices.Length - 4)
        {
            float rad = (float)v / nbSides * _2pi;
            float cos = Mathf.Cos(rad);
            float sin = Mathf.Sin(rad);

            normales[vert] = new Vector3(cos, 0f, sin);
            normales[vert + 1] = normales[vert];

            vert += 2;
            v++;
        }
        normales[vert] = normales[nbSides * 2 + 2];
        normales[vert + 1] = normales[nbSides * 2 + 3];
        #endregion

        #region UVs
        Vector2[] uvs = new Vector2[vertices.Length];

        // Bottom cap
        int u = 0;
        uvs[u++] = new Vector2(0.5f, 0.5f);
        while (u <= nbSides)
        {
            float rad = (float)u / nbSides * _2pi;
            uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            u++;
        }

        // Top cap
        uvs[u++] = new Vector2(0.5f, 0.5f);
        while (u <= nbSides * 2 + 1)
        {
            float rad = (float)u / nbSides * _2pi;
            uvs[u] = new Vector2(Mathf.Cos(rad) * .5f + .5f, Mathf.Sin(rad) * .5f + .5f);
            u++;
        }

        // Sides
        int u_sides = 0;
        while (u <= uvs.Length - 4)
        {
            float t = (float)u_sides / nbSides;
            uvs[u] = new Vector3(t, 1f);
            uvs[u + 1] = new Vector3(t, 0f);
            u += 2;
            u_sides++;
        }
        uvs[u] = new Vector2(1f, 1f);
        uvs[u + 1] = new Vector2(1f, 0f);
        #endregion

        #region Triangles
        int nbTriangles = nbSides + nbSides + nbSides * 2;
        int[] triangles = new int[nbTriangles * 3 + 3];

        // Bottom cap
        int tri = 0;
        int i = 0;
        while (tri < nbSides - 1)
        {
            triangles[i] = 0;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = tri + 2;
            tri++;
            i += 3;
        }
        triangles[i] = 0;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = 1;
        tri++;
        i += 3;

        // Top cap
        //tri++;
        while (tri < nbSides * 2)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = nbVerticesCap;
            tri++;
            i += 3;
        }

        triangles[i] = nbVerticesCap + 1;
        triangles[i + 1] = tri + 1;
        triangles[i + 2] = nbVerticesCap;
        tri++;
        i += 3;
        tri++;

        // Sides
        while (tri <= nbTriangles)
        {
            triangles[i] = tri + 2;
            triangles[i + 1] = tri + 1;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;

            triangles[i] = tri + 1;
            triangles[i + 1] = tri + 2;
            triangles[i + 2] = tri + 0;
            tri++;
            i += 3;
        }
        #endregion

        mesh.vertices = vertices;
        mesh.normals = normales;
        mesh.uv = uvs;
        mesh.triangles = triangles;

        mesh.RecalculateBounds();
        mesh.Optimize();

        midRadiusSize = topRadius;
        midHeight = height;
    }

    public void ChangeFill(GameObject other)
    {
        if (other.gameObject.tag == "Water" || other.gameObject.tag == "Alcohol")
        {
            if (increasesSoFar < numIncreases)
            {
                Color otherColor = other.gameObject.GetComponent<TrailRenderer>().material.color;
                if (increasesSoFar == 0)
                {
                    rend.material.color = otherColor;
                }
                else if (rend.material.color != otherColor)
                {
                    rend.material.color = Color.Lerp(rend.material.color, otherColor, Time.deltaTime * 0.1f);
                    // rend.material.color += new Color(otherColor.r, otherColor.g, otherColor.b, 0) / 256;
                }

                increasesSoFar += 1;
                ModifyCone();
            }
            Destroy(other.gameObject);
        } else if (other == null)
        {
            if (increasesSoFar > 0)
            {
                // Hardcoded values
                increasesSoFar = increasesSoFar > 9 ? increasesSoFar - 10: increasesSoFar = 0;
                ModifyCone();
            }
        }

        /*else
        {
            print(rend.material.color);
            rend.material.color = new Color(rend.material.color.r, rend.material.color.g, rend.material.color.b, 150);
        }*/
    }

    public void DecreaseFill() {
        if (increasesSoFar >= 0)
        {
            increasesSoFar--;
            ModifyCone();
            pourScript.Fill(rend.material);
        }
        else {
            increasesSoFar = 0;
            ModifyCone();
            pourScript.Empty();
        }
    }

    public void MakeOpaque()
    {
        Color oldColor = rend.material.color;
        if (oldColor.a < maxAlpha)
        {
            Color newColor = new Color(oldColor.r, oldColor.g, oldColor.b, oldColor.a + alphaStep);
            rend.material.color = newColor;
        }
    }
}
