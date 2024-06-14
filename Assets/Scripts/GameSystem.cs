using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameSystem : MonoBehaviour
{
    public Transform StartPosition;
    public GameObject[] points = new GameObject[11];
    public int[] fastest = new int[10];
    public int[] curFastest = new int[10];
    public int[] allTimeFastest = new int[10];

    public TextMeshProUGUI disText;
    public TextMeshProUGUI seqText;
    public TextMeshProUGUI genText;

    public int curGen = 0;
    public int fastestGen = 0;
    public float fastestDis = float.MaxValue;
    public float allTimeFastestDis = float.MaxValue;
    public bool draw = false;
    public bool useDraw = true;

    // Start is called before the first frame update
    void Start()
    {
        int[] nums = { 4, 0, 8, 1, 7, 3, 2, 6, 9, 5 };
        for(int i=0; i<10; i++)
        {
            fastest[i] = nums[i];
            curFastest[i] = nums[i];
            allTimeFastest[i] = nums[i];
        }
        fastestDis = MutDis(fastest);
        Invoke("FirstDraw", 2.5f);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyUp(KeyCode.Space))
        {
            curGen++;
            Mutation();
        }

        if (Input.GetKeyUp(KeyCode.A))
        {
            for (int i = 0; i < 5; i++)
            {
                curGen++;
                Mutation();
            }
        }

        if (Input.GetKeyUp(KeyCode.D))
        {
            draw = true;
        }

        if (Input.GetKeyUp(KeyCode.B))
        {
            curGen = fastestGen;
            fastestDis = allTimeFastestDis;
            curFastest = allTimeFastest;
        }

        genText.text = "Generation: " + curGen.ToString();
        disText.text = "Distance: " + fastestDis.ToString();
        string s = "[ ";
        for(int i=0; i<10; i++)
        {
            s = s + curFastest[i].ToString() + ", ";
        }
        s += "]";
        seqText.text = s;
    }

    void Mutation()
    {
        int[][] mut = new int[5][];
        float dis = float.MaxValue;
        int disNum = -1;
        for(int i=0; i<5; i++)
        {
            mut[i] = new int[10];
            for(int j=0; j<10; j++)
            {
                mut[i][j] = curFastest[j];
            }
            int a = Random.Range(0, 10);
            int b = Random.Range(0, 10);
            (mut[i][a], mut[i][b]) = (mut[i][b], mut[i][a]);
            float curDis = MutDis(mut[i]);
            if(curDis < dis)
            {
                dis = curDis;
                disNum = i;
            }
        }

        curFastest = mut[disNum];
        fastestDis = dis;
        if (dis < fastestDis)
        {
            fastestDis = dis;
            fastest = mut[disNum];
        }

        if (fastestDis < allTimeFastestDis)
        {
            allTimeFastestDis = fastestDis;
            allTimeFastest = fastest;
            fastestGen = curGen;
        }
        
    }

    float MutDis(int[] a)
    {
        float result = 0f;
        result += Vector2.Distance(StartPosition.position, 
            points[a[0]].transform.position);
        for(int i=0; i<9; i++)
        {
            result += Vector2.Distance(points[a[i]].transform.position, 
                points[a[i + 1]].transform.position);
        }
        result += Vector2.Distance(points[a[9]].transform.position, 
            StartPosition.position);
        return result;
    }

    void FirstDraw()
    {
        draw = true;
    }
}
