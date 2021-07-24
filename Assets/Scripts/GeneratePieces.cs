using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GeneratePieces : MonoBehaviour
{
    private List<int> p = new List<int>(){4,3,2,1,0,2,3,4};
    private List<string> n = new List<string>(){"BK","BQ","BB","BN","BR","BP","WK","WQ","WB","WN","WR","WP"};
    public Dictionary<GameObject, List<int>> allMoves = new Dictionary<GameObject, List<int>>();
    // Start is called before the first frame update
    void Start()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (j < 2 || j > 5)
                {
                    var P = new GameObject();
                    var SR = P.AddComponent<SpriteRenderer>();
                    switch (j)
                    {
                        case 0:
                            P.name = n[p[i]+6];
                            SR.sprite = Resources.LoadAll<Sprite>("cp")[p[i]+6];
                            break;
                        case 1:
                            P.name = n[11];
                            SR.sprite = Resources.LoadAll<Sprite>("cp")[11];
                            break;
                        case 6:
                            P.name = n[5];
                            SR.sprite = Resources.LoadAll<Sprite>("cp")[5];
                            break;
                        case 7:
                            P.name = n[p[i]];
                            SR.sprite = Resources.LoadAll<Sprite>("cp")[p[i]];
                            break;
                    }
                    SR.drawMode = SpriteDrawMode.Sliced;
                    SR.size = new Vector2(2,2);
                    P.transform.parent = transform;
                    Vector3 spawn = transform.position + new Vector3(
                        i*SR.size.x,
                        j*SR.size.y,
                        0
                    );
                    P.transform.position = spawn;
                    P.AddComponent<BoxCollider2D>();
                    P.AddComponent<Moves>();
                    if (P.name.Contains("W"))
                    {
                        P.tag = "White";
                    } else {
                        P.tag = "Black";
                    }
                    allMoves.Add(P, new List<int>());
                }
            }
        }
    }
}
