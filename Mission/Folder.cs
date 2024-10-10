using TMPro;
using UnityEngine;

public class Mission_Folder : MonoBehaviour
{
    [Header("Mission")]
    public IMission[] missions;
    public bool isHidden;

    [Header("Block")]
    public Mission_Page_Control control;

    public int Index
    {
        set
        {
            index = value;


            if (index >= missions.Length)
            {
                for (int i = 0; i < 3; i++)
                    control.Main_Page.titleTMP[i].color = new Color(249 / 255f, 163 / 255f, 27 / 255f);

                control.Main_Page.block.sprite = Mission_Manager.inst.Clear_Sprite;
                control.Main_Page.Set_Circle(index, missions.Length); ;
                Destroy(gameObject);
            }
            else
                control.SetUp(this, value);
        }
        get
        {
            return index;
        }
    }int index;

    private void Awake()
    {
        missions = GetComponentsInChildren<IMission>();

        if (missions.Length == 0) 
            Destroy(gameObject);
    }

    public void Get_Block(Mission_Page_Control control, int n = 0)
    {
        this.control = control;
        Index = n;
    }

    public IMission Get_Mission()
    {
        return missions[index];
    }
}
