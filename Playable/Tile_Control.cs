using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UIElements;

public class Tile_Control : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public bool Box_Target;

    public Unit_Control Unit
    {
        get
        {
            return unit;
        }
        set
        {
            unit = value;

            if (value == null) tile_renderer.color = tile_color[0];
            else
            {
                tile_renderer.color = tile_color[(int)unit.rate + 1];

                if(Box_Target)
                    value.info.search.layer = LayerMask.GetMask("Box");
                else
                    value.info.search.layer = LayerMask.GetMask("Mob");
            }
        }
    }Unit_Control unit;
    SpriteRenderer tile_renderer;

    Color[] tile_color = new Color[5]
    {
        new Color(1,1,1,20/255f),
        new Color(120/255f,    200/255f,   120/255f,    50/255f),
        new Color(120/255f,    200/255f,   255/255f,    50/255f),
        new Color(120/255f,    80/255f,    255/255f,    50/255f),
        new Color(255/255f,    80/255f,    80/255f,     50/255f)
    };

    GameObject copy;
    Camera cam;
    LayerMask layerMask;

    void Start()
    {
        cam = Camera.main;
        tile_renderer = GetComponent<SpriteRenderer>();

        layerMask = LayerMask.GetMask("Tile");
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (Unit == null || GameManager.inst.state != GameManager.State.Wave) return;

        copy = Instantiate(Unit.gameObject);

        Unit.GetComponent<Unit_Control>().Drag_Set_Origin(false);

        Unit_Control u = copy.GetComponent<Unit_Control>();
        u.Drag_Set_Layer();
        u.state = Char_Manager.State.Drag;

        int price = (int)Unit.rate == 0 ? Unit.Count * 10 : (int)Mathf.Pow(6, (int)Unit.rate) * Unit.Count * 10;
        Char_Manager.inst.Sell_And_Buy(this, true, price);
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (Unit == null || GameManager.inst.state != GameManager.State.Wave) return;

        copy.transform.position = cam.ScreenToWorldPoint(Input.mousePosition) - new Vector3(0,0,-10);
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Unit == null || GameManager.inst.state != GameManager.State.Wave) return;

        #region 판매
        //Debug.Log(Camera.main.ScreenToWorldPoint(Input.mousePosition));
        //ㅡㅡㅡㅡSell and Buy
        if (Camera.main.ScreenToWorldPoint(Input.mousePosition).y < -2.8f)
        {
            Sell();

            Destroy(Unit.gameObject);
            Unit = null;
        }
        else
            Unit.GetComponent<Unit_Control>().Drag_Set_Origin(true);
        #endregion
        Char_Manager.inst.Sell_And_Buy(this, false);
        Destroy(copy);

        //ㅡㅡㅡㅡRaycast
        Vector3 pos = cam.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, transform.forward, float.MaxValue, layerMask);
        if (hit)
        {
            Tile_Control tile = hit.collider.GetComponent<Tile_Control>();

            if (tile.Unit != null)
            {
                //합치기
                if (tile.Unit.name == Unit.name && tile.Unit.Count != 3 && tile != this)
                    Sum(tile);

                //합성
                else if (tile.unit.name == Unit.name
                      && tile.Unit.Count == 3 
                      && Unit.Count == 3 
                      && tile != this)
                    Evolution(tile);

                //교체
                else if (tile != this)
                    Change(tile);
            }
            else
            {
                tile.Unit = Unit;
                Unit = null;

                tile.Unit.transform.position = tile.transform.position;
            }
        }
    }

    public void Exit_Drag()
    {
        Unit.GetComponent<Unit_Control>().Drag_Set_Origin(true);
        Destroy(copy);
    }

    void Sell()
    {
        int price = (int)Unit.rate == 0 ? Unit.Count * 10 : (int)Mathf.Pow(6, (int)Unit.rate) * Unit.Count * 10;
        RuleManager.inst.Sell_Count += Unit.Count;

        int r = Random.Range(0, 10);
        if (r <= 6) RuleManager.inst.Ticket += price;
    }

    void Evolution(Tile_Control tile)
    {
        var obj = Char_Manager.inst.Evolution(tile.Unit.rate);

        if (obj == null)
        {
            Change(tile);
        }
        else
        {
            SoundManager.Inst.SFXPlay(SoundManager.Inst.Evolution, 0.3f);

            Destroy(tile.Unit.gameObject);
            Destroy(Unit.gameObject);

            obj.transform.position = tile.transform.position;
            tile.Unit = obj.GetComponent<Unit_Control>();
            Unit = null;

            RuleManager.inst.Evolution_Count++;
        }
    }

    void Sum(Tile_Control tile)
    {
        int move_count = 3 - tile.Unit.Count;

        if (move_count >= Unit.Count)
        {
            tile.Unit.Count += Unit.Count;

            Destroy(Unit.gameObject);
            Unit = null;
        }
        else
        {
            tile.Unit.Count += move_count;
            Unit.Count -= move_count;
        }
    }

    void Change(Tile_Control tile)
    {
        Unit_Control temp = tile.Unit;
        tile.Unit = Unit;
        Unit = temp;

        Unit.transform.position = transform.position;
        tile.Unit.transform.position = tile.transform.position;
    }
}
