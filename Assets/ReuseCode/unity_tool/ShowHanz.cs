using System.IO;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using Unity.VectorGraphics;

public class ShowHanz
{
    protected WrapHanzSvg wrpsvg;
    protected Sprite      sprite;
    protected VectorUtils.TessellationOptions tessOptions;
    int _initPathCount;

    public int PathCount
    {
        get { return _initPathCount; }
    }
    public ShowHanz()
    {
        wrpsvg = new WrapHanzSvg();
        tessOptions = new VectorUtils.TessellationOptions()
        {
            StepDistance = 100.0f,
            MaxCordDeviation = 0.5f,
            MaxTanAngleDeviation = 0.1f,
            SamplingStepSize = 0.01f
        };
    }

    public  Sprite Build()
    {
        if (sprite != null)
        {
            Object.Destroy(sprite);
        }
        sprite = wrpsvg.BuildSprite( 100.0f, VectorUtils.Alignment.Center, Vector2.zero, 32, true);
        return sprite;
    }
    public void FillMesh(Mesh mesh)
    {
        wrpsvg.FillMesh(mesh, 100.0f, true);
    }
    public bool LoadFromPath(string path)
    {
        bool ret = false;
        StreamReader reader = System.IO.File.OpenText(path);
        do
        {
            if (!wrpsvg.ImportSVG(reader))
            {
                Debug.LogError(string.Format("laod svg file {0} err", path));
                break;
            }
            wrpsvg.BuildGeoms(tessOptions);
            _initPathCount = wrpsvg._geoms.Count;
            ret = true;
        } while (false);
        return ret;
    }
    public bool LoadTextAsset(string path)
    {
        bool ret = false;
       
        do
        {
            TextAsset ta = Resources.Load<TextAsset>(path);
            if ( ta == null)
            {
                Debug.LogError(string.Format("load svg asset file {0} err", path));
                break;
            }
            StringReader reader = new StringReader(ta.text);
            if (!wrpsvg.ImportSVG(reader))
            {
                Debug.LogError(string.Format("laod svg file {0} err", path));
                break;
            }
            wrpsvg.BuildGeoms(tessOptions);
            _initPathCount = wrpsvg._geoms.Count;
            ret = true;
        } while (false);
        return ret;
    }
    public void ShowBihua(int index)
    {
        ShowBihua(index, Color.blue);
    }

    public void ShowBihua(int index, Color clr)
    {
        wrpsvg.SetPathColor(index, clr);
    }

    public void ShowAllColor( Color clr)
    {
        foreach(var geo in wrpsvg._geoms)
        {
            geo.Color = clr;
        }
    }
}

public class ShowHanzEx : ShowHanz
{
    NodeScene _node = new NodeScene();
    List<float> list_initValue = new List<float>();
    bool redraw = false;
    UnityAction on_complete;

    int _index = 0;
    bool stopped = true;

    public float deOffsetSpeed { set; get; }

    public UnityAction OnComplete
    {
        set { on_complete = value; }
    }

    public Sprite CurSprite
    {
        get { return sprite; }
    }

    public bool DrawFinish
    {
        get { return stopped; }
    }

    public ShowHanzEx(float speed = 20)
    {
        deOffsetSpeed = speed;
    }

    public bool LoadAndDraw(string path)
    {
        bool ret = false;
        do
        {
            if (!LoadFromPath(path))
            {
                break;
            }
            ret = StartDraw();
        } while (false);
        return ret;
    }
    public bool ReDraw()
    {
        wrpsvg.Reset(list_initValue);
        return StartDraw();
    }
    public bool StartDraw()
    {
        bool ret = false;
        do
        {
            _index = 0;
            if (!_node.SetNode(wrpsvg.GetPathIndex(_index)))
            {
                break;
            }

            if (!redraw)
            {          
                list_initValue.Add(_node.InitOffset);
            }
           
            _node.Update(wrpsvg);
            stopped = false;
            ret = true;
        } while (false);
        return ret;
    }

    public Sprite Update()
    {
        bool build = true;
        do
        {
            if (stopped)
            {
                build = false;
                break;
            }
            bool goon = _node.Minus(deOffsetSpeed);
            _node.Update(wrpsvg);
            if (goon)
            {
                break;
            }
            _index++;
            if (!_node.SetNode(wrpsvg.GetPathIndex(_index)))
            {
                stopped = true;
                if (on_complete != null)
                {
                    if (!redraw)
                    {
                        redraw = true;
                    }
                    on_complete();
                }
            }
            else
            {
                if (!redraw)
                {
                    list_initValue.Add(_node.InitOffset);
                }
            }
        } while (false);

        if (build)
        {
            Build();
        }
        return sprite;
    }
}