using System.Collections;
using System.Collections.Generic;
using Unity.VectorGraphics;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

using XLua;

[LuaCallCSharp]
public class SvgAnimation : MonoBehaviour
{
    enum showMothed
    {
        NO_SPRITE,
        USE_SPRITE,
        USE_IMG,
        USE_NORMAL,
    }

    SpriteRenderer spriteRd;
    SVGImage svgUI;
    Image img;
    ShowHanzEx shHanz;

    showMothed showMethod;

    float accTime = 0.02f;
    float spanTime = 0.03f;

    public void SetSprite(SpriteRenderer sr)
    {
        showMethod = showMothed.USE_SPRITE;
        spriteRd = sr;
    }

    public void SetImg(SVGImage sr)
    {
        showMethod = showMothed.USE_IMG;
        svgUI = sr;
    }

    public void SetNormalImg(Image sr)
    {
        showMethod = showMothed.USE_NORMAL;
        img = sr;
        img.useSpriteMesh = true;
    }
    public bool SetPath(string path)
    {
        return  shHanz.LoadFromPath(path);
    }

    public bool LoadFromTextAsset(string path)
    {
        return shHanz.LoadTextAsset(path);
    }
    public void ShowPath(int index)
    {
        shHanz.ShowBihua(index);
    }
    public void SetAllColor(Color cl)
    {
        shHanz.ShowAllColor(cl);
    }
    public void ShowBihua(int index, Color clr)
    {
        shHanz.ShowBihua(index, clr);
    }

    void Show(Sprite sp)
    {
        switch(showMethod)
        {
            case showMothed.USE_SPRITE:
                spriteRd.sprite = sp;
                break;
            case showMothed.USE_IMG:
                svgUI.sprite = sp;
                break;
            case showMothed.USE_NORMAL:
                img.sprite = sp;
                break;
        }
    }
    public void Show()
    {
        Show( shHanz.Build());
    }

    public void StartDraw(float speed = 20, UnityAction action = null)
    {
        shHanz.deOffsetSpeed = speed;
        shHanz.OnComplete = action;
        shHanz.StartDraw();
    }
    public void Redraw()
    {
        shHanz.ReDraw();
    }
    public bool UseSpriteRender()
    {
        Init();
        SpriteRenderer sr = this.GetComponent<SpriteRenderer>();
        if ( sr != null)
        {
            this.SetSprite(sr);
        }
        return sr != null;
    }
    public bool UseSvgImg()
    {
        Init();
        SVGImage sr = this.GetComponent<SVGImage>();
        if (sr != null)
        {
            this.SetImg(sr);
        }
        return sr != null;
    }
    public bool UseNormalImg()
    {
        Init();
        Image sr = this.GetComponent<Image>();
        if (sr != null)
        {
            this.SetNormalImg(sr);
        }
        return sr != null;
    }

    void Init()
    {
        if ( null == shHanz)
        {
            shHanz = new ShowHanzEx();
            showMethod = showMothed.NO_SPRITE;
        }
    }
    /// <summary>
    /// /////////////////////////////////////////////////
    /// </summary>
    void Awake()
    {
        Init();
        //shHanz = new ShowHanzEx();
        //showMethod = showMothed.NO_SPRITE;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
         if ( showMethod == showMothed.NO_SPRITE || shHanz.DrawFinish)
        {
            return;
        }
        accTime += Time.deltaTime;

        if (accTime < spanTime)
        {
            return;
        }

        Show( shHanz.Update());
        accTime = 0;
    }
}
