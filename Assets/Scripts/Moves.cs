using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Moves : MonoBehaviour
{
    private Camera Cam;
    private string n;
    private List<Transform> BPs = new List<Transform>();
    public List<Transform> possibleMoves = new List<Transform>();
    private List<int> BlackLocations = new List<int>();
    private List<int> WhiteLocations = new List<int>();
    public Dictionary<GameObject, List<int>> allMoves = new Dictionary<GameObject, List<int>>();
    public List<int> Moved = new List<int>();

    // Start is called before the first frame update
    void Start()
    {
        BPs = GameObject.Find("Board").GetComponent<GenerateBoard>().BPs;
        n = gameObject.name;
        Cam = Camera.main;
        allMoves = GameObject.Find("Pieces").GetComponent<GeneratePieces>().allMoves;
        Moved = allMoves[gameObject];
        Moved.Add(BPs.IndexOf(GetClosest(BPs)));
        GenerateMoves();
    }
    void OnMouseDown(){
        GetLocations();
        possibleMoves.Clear();
        GenerateMoves();
        foreach (Transform BP in possibleMoves)
        {
            BP.GetComponent<SpriteRenderer>().color = Color.red;
        }
    }
    void OnMouseDrag() {
        transform.GetComponent<SpriteRenderer>().size = new Vector2(2.1f,2.1f);
        transform.position = new Vector3(
            Cam.ScreenToWorldPoint(Input.mousePosition).x,
            Cam.ScreenToWorldPoint(Input.mousePosition).y,
            -1.1f
        );
    }
    void OnMouseUp() {
        foreach (Transform BP in BPs)
        {
            BP.GetComponent<SpriteRenderer>().color = Color.white;
        }
        transform.position = GetClosest(possibleMoves).position - new Vector3(0,0,1);
        transform.GetComponent<SpriteRenderer>().size = new Vector2(2,2);
        if (BPs.IndexOf(GetClosest(BPs)) != Moved[Moved.Count-1])
        {
            Moved.Add(BPs.IndexOf(GetClosest(BPs)));
        }
        foreach (KeyValuePair<GameObject, List<int>> kvp in allMoves)
        {
            if (kvp.Value[kvp.Value.Count-1] == Moved[Moved.Count-1] && kvp.Key != gameObject)
            {
                allMoves.Remove(kvp.Key);
                GameObject.Find("Pieces").GetComponent<GeneratePieces>().allMoves.Remove(kvp.Key);
                Destroy(kvp.Key);
            }
        }
    }

     public Transform GetClosest(List<Transform> BPs)
    {
        Transform T = null;
        float minDist = Mathf.Infinity;
        foreach (Transform tr in BPs)
        {
            float dist = Vector3.Distance(tr.position, transform.position);
            if (dist < minDist)
            {
                T = tr;
                minDist = dist;
            }
        }
        return T;
    }
    void GetLocations(){
        BlackLocations.Clear();
        WhiteLocations.Clear();
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("Black"))
        {
            BlackLocations.Add(BPs.IndexOf(GO.GetComponent<Moves>().GetClosest(BPs)));
        }
        foreach (GameObject GO in GameObject.FindGameObjectsWithTag("White"))
        {
            WhiteLocations.Add(BPs.IndexOf(GO.GetComponent<Moves>().GetClosest(BPs)));
        }
    }
    void GenerateMoves(){
        int i = BPs.IndexOf(GetClosest(BPs));
        if (n.Contains("K"))       
        {
            switch (i % 8)
            {
                case 0:
                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+1]);
                        possibleMoves.Add(BPs[i+8]);
                        possibleMoves.Add(BPs[i+9]);
                    } else if (56 <= i && i <= 63){
                        possibleMoves.Add(BPs[i+1]);
                        possibleMoves.Add(BPs[i-8]);
                        possibleMoves.Add(BPs[i-7]);
                    } else {
                        possibleMoves.Add(BPs[i+1]);
                        possibleMoves.Add(BPs[i+8]);
                        possibleMoves.Add(BPs[i-8]);
                        possibleMoves.Add(BPs[i-7]);
                        possibleMoves.Add(BPs[i+9]);
                    }
                    break;
                case 7:
                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+8]);
                        possibleMoves.Add(BPs[i-1]);
                        possibleMoves.Add(BPs[i+7]);
                    } else if (56 <= i && i <= 63){
                        possibleMoves.Add(BPs[i-8]);
                        possibleMoves.Add(BPs[i-9]);
                        possibleMoves.Add(BPs[i-1]); 
                    } else {
                        possibleMoves.Add(BPs[i+8]);
                        possibleMoves.Add(BPs[i-8]);
                        possibleMoves.Add(BPs[i-9]);
                        possibleMoves.Add(BPs[i-1]);
                        possibleMoves.Add(BPs[i+7]);
                    }
                    
                    break;
                default:
                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+1]);
                        possibleMoves.Add(BPs[i+9]);
                        possibleMoves.Add(BPs[i+8]);
                        possibleMoves.Add(BPs[i-1]);
                        possibleMoves.Add(BPs[i+7]);
                    } else if (56 <= i && i <= 63){
                        possibleMoves.Add(BPs[i-7]);
                        possibleMoves.Add(BPs[i+1]);
                        possibleMoves.Add(BPs[i-8]);
                        possibleMoves.Add(BPs[i-9]);
                        possibleMoves.Add(BPs[i-1]);
                    } else {
                        possibleMoves.Add(BPs[i-7]);
                        possibleMoves.Add(BPs[i+1]);
                        possibleMoves.Add(BPs[i+9]);
                        possibleMoves.Add(BPs[i-8]);
                        possibleMoves.Add(BPs[i+8]);
                        possibleMoves.Add(BPs[i-9]);
                        possibleMoves.Add(BPs[i-1]);
                        possibleMoves.Add(BPs[i+7]);
                    }
                    break;
            }
        }
        else if (n == "BB" || n == "WB")
        {
            int j = i;
            while (j % 8 != 7 && j < 56)
            {
                j += 9;
                if ((n == "BB" && BlackLocations.Contains(j)) || (n == "WB" && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 0 && j < 56)
            {
                j += 7;
                if ((n == "BB" && BlackLocations.Contains(j)) || (n == "WB" && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 0 && j > 7)
            {
                j -= 9;
                if ((n == "BB" && BlackLocations.Contains(j)) || (n == "WB" && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 7 && j > 7)
            {
                j -= 7;
                if ((n == "BB" && BlackLocations.Contains(j)) || (n == "WB" && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
        }
        else if (n.Contains("N"))
        {
            switch (i % 8)
            {
                case 0:
                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                    } else if (8 <= i && i <= 15){
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                    } else if (56 <= i && i <= 63)
                    {
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i-15]);
                    } else if (48 <= i && i <= 55)
                    {
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i-15]);
                    } else {
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                        possibleMoves.Add(BPs[i-15]);
                    }
                    break;
                case 1:
                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                        possibleMoves.Add(BPs[i+15]);
                    } else if (8 <= i && i <= 15){
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                        possibleMoves.Add(BPs[i+15]);
                    } else if (56 <= i && i <= 63)
                    {
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-17]);
                    } else if (48 <= i && i <= 55)
                    {
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-17]);
                    } else {
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i+15]);
                    }
                    break;
                case 6:
                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i+15]);
                        possibleMoves.Add(BPs[i+17]);
                    } else if (8 <= i && i <= 15){
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i+15]);
                        possibleMoves.Add(BPs[i+17]);
                    } else if (56 <= i && i <= 63)
                    {
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-17]);
                    } else if (48 <= i && i <= 55)
                    {
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i+6]);
                    } else {
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i+15]);
                        possibleMoves.Add(BPs[i+17]);
                    }
                    break;
                case 7:
                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i+15]);
                    } else if (8 <= i && i <= 15){
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i+15]);
                    } else if (56 <= i && i <= 63)
                    {
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-17]);
                    } else if (48 <= i && i <= 55)
                    {
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i+6]);
                    } else {
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i+15]);
                    }
                    break;
                default:

                    if (0 <= i && i <= 7)
                    {
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                        possibleMoves.Add(BPs[i+15]);
                        possibleMoves.Add(BPs[i+6]);
                    } else if (8 <= i && i <= 15){
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                        possibleMoves.Add(BPs[i+15]);
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-6]);
                    } else if (56 <= i && i <= 63)
                    {
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-6]);
                    } else if (48 <= i && i <= 55)
                    {
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-6]);
                        possibleMoves.Add(BPs[i+10]);
                    } else {
                        possibleMoves.Add(BPs[i+10]);
                        possibleMoves.Add(BPs[i+17]);
                        possibleMoves.Add(BPs[i+15]);
                        possibleMoves.Add(BPs[i+6]);
                        possibleMoves.Add(BPs[i-10]);
                        possibleMoves.Add(BPs[i-17]);
                        possibleMoves.Add(BPs[i-15]);
                        possibleMoves.Add(BPs[i-6]);
                    }
                    break;
                
            }
        }
        else if (n.Contains("R"))
        {
            int j = i;
            while (j % 8 != 7)
            {
                j += 1;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j > 7)
            {
                j -= 8;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 0)
            {
                j -= 1;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j < 56)
            {
                j += 8;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
        }
        else if (n.Contains("P"))
        {
            //double moves
            if (i % 8 == 1 && n.Contains("W") && !BlackLocations.Contains(i+1) && !WhiteLocations.Contains(i+1))
            {
                possibleMoves.Add(BPs[i+1]);
                if (!BlackLocations.Contains(i+2) && !WhiteLocations.Contains(i+2))
                {
                    possibleMoves.Add(BPs[i+2]);
                }
                
            }
            else if (i % 8 == 6 && n.Contains("B") && !BlackLocations.Contains(i-1) && !WhiteLocations.Contains(i-1))
            {
                possibleMoves.Add(BPs[i-1]);
                if (!BlackLocations.Contains(i-2) && !WhiteLocations.Contains(i-2))
                {
                    possibleMoves.Add(BPs[i-2]);
                }
            }

            //simlpe moves
            if(i % 8 != 1 && n.Contains("W") && i % 8 != 7 && !BlackLocations.Contains(i+1) && !WhiteLocations.Contains(i+1))
            {
                possibleMoves.Add(BPs[i+1]);
        
            }
            if(i % 8 != 6 && n.Contains("B") && i % 8 != 0 && !BlackLocations.Contains(i-1) && !WhiteLocations.Contains(i-1))
            {
                possibleMoves.Add(BPs[i-1]);        
            }

            //taking enemy pieces
            if (BlackLocations.Contains(i-7) && n.Contains("W")) //enemy left 4 white
            {
                possibleMoves.Add(BPs[i-7]);
            }
            if (BlackLocations.Contains(i+9) && n.Contains("W")) //enemy right 4 white
            {
                possibleMoves.Add(BPs[i+9]);
            }
            if (WhiteLocations.Contains(i-9) && n.Contains("B")) //enemy left 4 black
            {
                possibleMoves.Add(BPs[i-9]);
            }
            if (WhiteLocations.Contains(i+7) && n.Contains("B")) //enemy right 4 black
            {
                possibleMoves.Add(BPs[i+7]);
            }

            //in
            if (i % 8 == 7 && n.Contains("W"))
            {
                
            }
            else if (i % 8 == 0 && n.Contains("B"))
            {
                
            }
        }
        else
        {
            int j = i;
            while (j % 8 != 7 && j < 56)
            {
                j += 9;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 0 && j < 56)
            {
                j += 7;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 0 && j > 7)
            {
                j -= 9;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 7 && j > 7)
            {
                j -= 7;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 7)
            {
                j += 1;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j > 7)
            {
                j -= 8;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j % 8 != 0)
            {
                j -= 1;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
            j = i;
            while (j < 56)
            {
                j += 8;
                if ((n.Contains("B") && BlackLocations.Contains(j)) || (n.Contains("W") && WhiteLocations.Contains(j)))
                {
                    break;
                }
                possibleMoves.Add(BPs[j]);
                if (BlackLocations.Contains(j) || WhiteLocations.Contains(j))
                {
                    break;
                }
            }
        }
        goto Remove;
        Remove:
            foreach (Transform m in possibleMoves)
            {
                if (!n.Contains("P") && !n.Contains("R") && n != "BB" && n != "WB" && !n.Contains("Q"))
                {
                    if (BlackLocations.Contains(BPs.IndexOf(m)) && n.Contains("B"))
                    {
                        possibleMoves.Remove(m);
                        goto Remove;
                    }
                    if (WhiteLocations.Contains(BPs.IndexOf(m)) && n.Contains("W"))
                    {
                        possibleMoves.Remove(m);
                        goto Remove;
                    }
                }
            }
        possibleMoves.Add(BPs[i]);
    }
}
//   -14, -6, +2, +10, +18
//   -15, -7, +1,  +9, +17
//   -16, -8,  0,  +8, +16
//   -17, -9, -1,  +7, +15 
//   -18, -10, -2, +6, +14
