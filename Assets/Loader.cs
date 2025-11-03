using System.IO;
using UnityEngine;

public class Loader : MonoBehaviour
{
    [SerializeField] string path;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    public string extension;
    public int nbVertex;
    public int nbSides;
    public int nbRidges;
    public Vertex[] Vertexs;

    public class Vertex
    {
        public float x;
        public float y;
        public float z;

        public Vertex(float x, float y, float z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

    }
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
        StreamReader reader = new StreamReader(path);
        extension = reader.ReadLine();
        if (extension == "OFF")
        {
            Debug.Log("Bonne extension");
        }
    }



    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
