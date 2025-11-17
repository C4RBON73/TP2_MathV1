using System.Globalization;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;


public class Loader : MonoBehaviour
{
    private Mesh mesh;
    [SerializeField] string struct_path;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private string extension;
    private int nbVertex;
    private int nbSides;
    private int nbRidges;
    private Vector3[] vertexs;
    private int[] triangles;
    private Vector3 CoG;
    private Vector3[] normals;
    
    //La coordonné la plus éloigner du centre de gravité
    private Vector3 normalReference = new Vector3(0f,0f,0f); 

   


    public void loadStructure(string path)
    {
        //Initialisation du reader
        StreamReader reader = new StreamReader(path);
        extension = reader.ReadLine();

        //vérification de l'extension
        if (extension == "OFF")
        {
            Debug.Log("Bonne extension");
        }

        //initialisation des valeurs
        var line = reader.ReadLine();
        var split_line = line.Split(" ");
        nbVertex = int.Parse(split_line[0]);
        nbSides = int.Parse(split_line[1]);
        nbRidges = int.Parse(split_line[2]);
        vertexs = new Vector3[nbVertex];
        triangles = new int[nbSides *3];
        normals = new Vector3[nbVertex];

        

        //Initialisation de vertexs
        for (int i = 0; i < nbVertex; i++) {
            line = reader.ReadLine();
            split_line = line.Split(" ");
            vertexs[i] =(new Vector3(float.Parse(split_line[0],CultureInfo.InvariantCulture), float.Parse(split_line[1], CultureInfo.InvariantCulture), float.Parse(split_line[2], CultureInfo.InvariantCulture)));
            normalReference.x = Mathf.Max(normalReference.x, Mathf.Abs(vertexs[i].x));
            normalReference.y = Mathf.Max(normalReference.y, Mathf.Abs(vertexs[i].y));
            normalReference.z = Mathf.Max(normalReference.z, Mathf.Abs(vertexs[i].z));

            //Pour ne pas repasser dedans lors du calcul de CoG
            CoG += vertexs[i];

        }
        //Calcul final de CoG
        CoG = CoG / nbVertex;


        //Initialisation de sides
        int t = 0;
        for (int y = 0; y < nbSides; y++) {
            line = reader.ReadLine();
            split_line = line.Split(" ");


            //Pour récupérer les valeurs et les convertir en Int dans une liste, on Skip 0 car c'est la taille des sides et pas une valeur
            for (var z= 1; z <= int.Parse(split_line[0]); z++) {
                
                triangles[t] = (int.Parse(split_line[z]));
                t++;
            }
            
        }


        if (normalReference.x > 0 && normalReference.y > 0 && normalReference.z > 0)
        {
            var max = Mathf.Max(normalReference.x,normalReference.y,normalReference.z);
            for (int u = 0; u < vertexs.Length; u++)
            {
                vertexs[u] = new Vector3(vertexs[u].x / max, vertexs[u].y / max, vertexs[u].z / max);
            }
        }


        //Calcul des normales
        int s = 0;
        for ( int v =0; v < normals.Length; v++)
        {
            Vector3 p1 = vertexs[triangles[v + s]];
            Vector3 p2 = vertexs[triangles[v + ++s]];
            Vector3 p3 = vertexs[triangles[v + ++s]];

            Vector3 v1 = p2 - p1;
            Vector3 v2 = p3 - p1;

            normals[v] = Vector3.Cross(v1, v2).normalized;
            
        }


    }


    public void affiche_struct()
    {
        mesh.Clear();

         


        
        mesh.vertices = vertexs;
        mesh.normals = normals;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

    }

    void Start()
    {
        mesh = GetComponent<MeshFilter>().mesh;
        loadStructure(struct_path);

        affiche_struct();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
