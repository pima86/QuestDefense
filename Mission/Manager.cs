using System.Collections;
using TMPro;
using UnityEngine;
public class Mission_Manager : MonoBehaviour
{
    public static Mission_Manager inst;
    private void Awake() => inst = this;


    [Header("Canvas")]
    [SerializeField] GameObject Canvas;

    [Header("Page")]
    [SerializeField] Transform Content_Transform;
    [SerializeField] GameObject Mission_Prefab;
    public Sprite Clear_Sprite;

    [Header("Mission")]
    public TextMeshProUGUI Missions_percent;
    public Mission_Folder[] Folders;
    public float missions_count;
    public float missions_clear;
    public float percent;

    [Header("Hide")]
    [SerializeField] GameObject hidden_block;
    public TextMeshProUGUI Hiddens_percent;
    public float Hiddens_count;
    public float Hiddens_clear;

    private void Start()
    {
        Page_Create();

        StartCoroutine(Update_Search());
    }

    #region 성공 여부
    IEnumerator Update_Search()
    {
        while (true)
        {
            for (int i = 0; i < Folders.Length; i++)
            {
                IMission m = Folders[i].Get_Mission();
                if (m.Search())
                {
                    if (!Canvas.activeSelf)
                    {
                        SoundManager.Inst.SFXPlay(SoundManager.Inst.Clear, 0.5f);

                        Canvas.SetActive(true);
                        RuleManager.inst.Main_Page.SetUp(m.Origin_Mission, false);

                        if(Folders[i].isHidden) 
                            Hiddens_clear++;
                        else
                            missions_clear++;

                        m.Origin_Mission.Reward();
                    }

                    yield return new WaitForSeconds(0.1f);

                    Folders = GetComponentsInChildren<Mission_Folder>();

                    percent = (missions_clear + Hiddens_clear) / (missions_count + Hiddens_count);
                    Missions_percent.text = (percent * 100f).ToString("N2") + "%";
                    Hiddens_percent.text = ((Hiddens_clear / Hiddens_count) * 100f).ToString("N2") + "%";
                }
            }

            yield return new WaitForSeconds(0.1f);
        }
    }
    #endregion

    #region 페이지에 생성
    public void Page_Create()
    {
        Folders = GetComponentsInChildren<Mission_Folder>();

        int hidden_block_index = 1;
        for (int i = 0; i < Folders.Length; i++)
        {
            var obj = Instantiate(Mission_Prefab, Content_Transform);

            Mission_Page_Control control = obj.GetComponent<Mission_Page_Control>();
            Folders[i].Get_Block(control);

            if (Folders[i].isHidden)
            {
                Hiddens_count++;
                obj.SetActive(false);
            }
            else
            {
                missions_count += Folders[i].missions.Length;
                hidden_block_index++;
            }
        }

        hidden_block.transform.SetSiblingIndex(hidden_block_index);
    }
    #endregion

}
