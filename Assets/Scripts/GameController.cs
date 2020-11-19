using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public enum Tip { AsInima, AsInima2, AsPica, AsPica2, AsRomb, AsRomb2, AsTrefla, AsTrefla2, None };

    [System.Serializable]
    public class Carte
    {
        public GameObject obiect;
        public Tip tip;
        public bool isHidden = true;
        public int order = 0;
    }

    public Carte[] carti;
    public GameObject CardBack;
    public int score = 0;

    bool delay = false;
    private LinkedList<Tip> carti_rezolvate = new LinkedList<Tip>();
    private LinkedList<Tip> carti_nerezolvate = new LinkedList<Tip>();

    private UnityEngine.Object[] clones = null;
    private UnityEngine.Object[] clonesBack = null;

    private float cardOffset_x = -6.0f;
    private float cardOffset_y = -2.5f;

    Carte getCarte(Tip t)
    {
        for(int i=0; i < carti.Length; i++)
        {
            if(carti[i].tip==t)
            {
                return carti[i];
            }
        }

        return null;
    }

    // Start is called before the first frame update
    void Start()
    {
        clones = new UnityEngine.Object[carti.Length];
        clonesBack = new UnityEngine.Object[carti.Length];
        System.Random rand = new System.Random();
        LinkedList<int> randNrs = new LinkedList<int>();

        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 4; j++) 
            {
                int num = 0;
                while(randNrs.Contains(num=rand.Next(carti.Length)))
                {

                }

                randNrs.AddLast(num);

                clones[j + 4 * i] = Instantiate(carti[num].obiect, new Vector2(j * 4 + cardOffset_x, i * 4 + cardOffset_y), Quaternion.identity);
                clonesBack[j + 4 * i] = Instantiate(CardBack, new Vector2(j * 4 + cardOffset_x, i * 4 + cardOffset_y), Quaternion.identity);

                clones[j + 4 * i].name = carti[num].tip.ToString();
                clonesBack[j + 4 * i].name = "B" + carti[num].tip.ToString();

                carti[num].order = j + 4 * i;
            }
    }

    void OnGUI()
    {
        GUIStyle myStyle = new GUIStyle(GUI.skin.GetStyle("label"));
        myStyle.fontSize = 25;
        GUI.Label(new Rect(25, 25, 100, 50), "Score:" + score.ToString(), myStyle);
    }

    // Update is called once per frame
    void Update()
    {
        checkCards();
    }

    void checkPair()
    {
        if(carti_nerezolvate.Contains(Tip.AsInima) && carti_nerezolvate.Contains(Tip.AsInima2))
        {
            carti_rezolvate.AddLast(Tip.AsInima);
            carti_rezolvate.AddLast(Tip.AsInima2);
            carti_nerezolvate.Clear();
            score++;
            return;
        }

        if (carti_nerezolvate.Contains(Tip.AsPica) && carti_nerezolvate.Contains(Tip.AsPica2))
        {
            carti_rezolvate.AddLast(Tip.AsPica);
            carti_rezolvate.AddLast(Tip.AsPica2);
            carti_nerezolvate.Clear();
            score++;
            return;
        }

        if (carti_nerezolvate.Contains(Tip.AsRomb) && carti_nerezolvate.Contains(Tip.AsRomb2))
        {
            carti_rezolvate.AddLast(Tip.AsRomb);
            carti_rezolvate.AddLast(Tip.AsRomb2);
            carti_nerezolvate.Clear();
            score++;
            return;
        }

        if (carti_nerezolvate.Contains(Tip.AsTrefla) && carti_nerezolvate.Contains(Tip.AsTrefla2))
        {
            carti_rezolvate.AddLast(Tip.AsTrefla);
            carti_rezolvate.AddLast(Tip.AsTrefla2);
            carti_nerezolvate.Clear();
            score++;
            return;
        }

        delay = true;
        StartCoroutine("Delay", 2.0f);
    }

    IEnumerator Delay(float Count)
    {
        yield return new WaitForSeconds(Count);

        getCarte(carti_nerezolvate.First.Value).isHidden = true;
        getCarte(carti_nerezolvate.Last.Value).isHidden = true;
        GameObject.Find("B" + carti_nerezolvate.First.Value.ToString()).GetComponent<Renderer>().enabled = true;
        GameObject.Find("B" + carti_nerezolvate.Last.Value.ToString()).GetComponent<Renderer>().enabled = true;

        carti_nerezolvate.Clear();
        delay = false;
        yield return null;
    }

    void checkCards()
    {
        if(Input.GetMouseButtonDown(0) && !delay)
        {
            Vector3 mousePos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            Vector2 mousePos2D = new Vector2(mousePos.x, mousePos.y);

            RaycastHit2D hit = Physics2D.Raycast(mousePos2D, Vector2.zero);

            if (hit.collider != null)
            {
                string name = hit.collider.gameObject.name;

                if(getCarte(nameToType(name)).isHidden==true && !carti_nerezolvate.Contains(nameToType(name)))
                {
                    GameObject s = GameObject.Find("B" + name);

                    if (s != null)
                    {
                        s.GetComponent<Renderer>().enabled = false;
                    }

                    getCarte(nameToType(name)).isHidden = false;
                    carti_nerezolvate.AddLast(nameToType(name));
                }
            }

            if (carti_nerezolvate.Count == 2) 
            {
                checkPair();
            }
        }
    }

    static Tip nameToType(string s)
    {
        if (s == "AsInima")
            return Tip.AsInima;

        if (s == "AsInima2")
            return Tip.AsInima2;

        if (s == "AsPica")
            return Tip.AsPica;

        if (s == "AsPica2")
            return Tip.AsPica2;

        if (s == "AsRomb")
            return Tip.AsRomb;

        if (s == "AsRomb2")
            return Tip.AsRomb2;

        if (s == "AsTrefla")
            return Tip.AsTrefla;

        if (s == "AsTrefla2")
            return Tip.AsTrefla2;

        return Tip.None;
    }

    public void reset()
    {
        System.Random rand = new System.Random();
        LinkedList<int> randNrs = new LinkedList<int>();

        for (int i = 0; i < 2; i++)
            for (int j = 0; j < 4; j++)
            {
                int num = 0;
                while (randNrs.Contains(num = rand.Next(carti.Length)))
                {

                }

                randNrs.AddLast(num);

                Destroy(clones[j + 4 * i]);
                Destroy(clonesBack[j + 4 * i]);

                clones[j + 4 * i] = Instantiate(carti[num].obiect, new Vector2(j * 4 + cardOffset_x, i * 4 + cardOffset_y), Quaternion.identity);
                clonesBack[j + 4 * i] = Instantiate(CardBack, new Vector2(j * 4 + cardOffset_x, i * 4 + cardOffset_y), Quaternion.identity);

                clones[j + 4 * i].name = carti[num].tip.ToString();
                clonesBack[j + 4 * i].name = "B" + carti[num].tip.ToString();

                carti[num].order = j + 4 * i;
            }

        for (int i=0;i<carti.Length;i++)
        {
            string name = "B" + carti[i].tip.ToString();
            GameObject s = GameObject.Find(name);
            if (s != null)
            {
                s.GetComponent<Renderer>().enabled = true;
            }

            carti[i].isHidden = true;
        }

        score = 0;
        carti_nerezolvate.Clear();
        carti_rezolvate.Clear();
    }
}
