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
        

        //Initialisation de vertexs
        for (int i = 0; i < nbVertex; i++) {
            line = reader.ReadLine();
            split_line = line.Split(" ");
            vertexs[i] =(new Vector3(float.Parse(split_line[0],CultureInfo.InvariantCulture), float.Parse(split_line[1], CultureInfo.InvariantCulture), float.Parse(split_line[2], CultureInfo.InvariantCulture)));
            normalReference.x = Mathf.Max(normalReference.x,(vertexs[i].x));
            normalReference.y = Mathf.Max(normalReference.y, (vertexs[i].y));
            normalReference.z = Mathf.Max(normalReference.z, (vertexs[i].z));

            //Pour ne pas repasser dedans lors du calcul de CoG
            CoG += vertexs[i];
        }
        //Calcul final de CoG
        CoG = CoG / nbVertex;


        //Initialisation de sides
        for (int y = 0; y < nbSides; y++) {
            line = reader.ReadLine();
            split_line = line.Split(" ");


            //Pour récupérer les valeurs et les convertir en Int dans une liste, on Skip 1 car c'est la taille des sides et pas une valeur
            for (var z= 1; z <= int.Parse(split_line[0]); z++) {
                
                triangles[y + z-1] = (int.Parse(split_line[z]));
            }
        }

        Debug.Log(normalReference);
        if (normalReference.x > 0 && normalReference.y > 0 && normalReference.z > 0)
        {
            for (int u = 0; u < vertexs.Length; u++)
            {
                //vertexs[u] = new Vector3(vertexs[u].x / normalReference.x, vertexs[u].y / normalReference.y, vertexs[u].z / normalReference.z);
            }
        }


    }


    public void affiche_struct()
    {
        mesh.Clear();

         



        mesh.vertices = vertexs;
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
