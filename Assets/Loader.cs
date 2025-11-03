using System.Globalization;
using System.IO;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;


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
    private Sides[] sides;

   
    public class Sides
        {
            public int size;
            public int[] list;

            public Sides(int size)
            {
                this.size = size;
                list = new int[size];
            }

            public void addList(int[] list)
            {
                this.list = list;
            }
    }

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
        sides = new Sides[nbSides];

        //Initialisation de vertexs
        for (int i = 0; i < nbVertex; i++) {
            line = reader.ReadLine();
            split_line = line.Split(" ");
            vertexs.Append(new Vector3(float.Parse(split_line[0],CultureInfo.InvariantCulture), float.Parse(split_line[1], CultureInfo.InvariantCulture), float.Parse(split_line[2], CultureInfo.InvariantCulture)));

        }
        Debug.Log(nbVertex);
        Debug.Log(nbSides); 
        //Initialisation de sides
        for (int y = 0; y < nbSides; y++) {
            line = reader.ReadLine();
            split_line = line.Split(" ");

            //Nécessaire afin de convertir les strings du fichiers
            Sides temp = new Sides(int.Parse(split_line[0]));
            int[] temp2 = new int[int.Parse(split_line[0])];
            
            //Pour récupérer les valeurs et les convertir en Int dans une liste, on Skip 1 car c'est la taille des sides et pas une valeur
            for (var z =0; z < int.Parse(split_line[0]); z++) {
                temp2.Append(int.Parse(split_line[z]));
            }
            temp.addList(temp2);
            sides.Append(temp);
        }


    }


    public void affiche_struct()
    {
        mesh.Clear();

         

        int[] triangles = new int[nbSides * 3];

        int t = 0;
        for (int i = 0; i < nbSides; i++) {
            triangles[t] = sides[i].list[0];
            triangles[t + 1] = sides[i].list[1];
            triangles[t + 2] = sides[i].list[2];
            t += 3;
        }

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
