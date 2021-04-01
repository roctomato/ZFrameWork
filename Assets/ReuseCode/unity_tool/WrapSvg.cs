using UnityEngine;
using Unity.VectorGraphics;
using static Unity.VectorGraphics.VectorUtils;

using System;
using System.IO;
using System.Collections.Generic;


public class NodeScene
{
    Scene nodeScene = new Scene();
    Geometry _geometry;
    Stroke _stroke;
    int _nodeIndex;
    float _initOffset;
    float _curValue;

    public float InitOffset { get { return _initOffset; } }

    public bool SetNode(SceneNode nd)
    {
        bool ret = false;
       
        if (null != nd)
        {
            foreach (var shape in nd.Shapes)
            {
                if (shape.PathProps.Stroke != null)
                {
                    nodeScene.Root = nd;
                    _stroke = shape.PathProps.Stroke;
                    _initOffset = _curValue = shape.PathProps.Stroke.PatternOffset;
                    _nodeIndex = -1;
                    ret = true;
                    break;

                }
            }
        }
        return ret;
    }

    public bool Update(WrapHanzSvg whs)
    {
        return Update(whs._geoms, whs._option);
    }
    public bool Update(List<Geometry> geoms, VectorUtils.TessellationOptions tessOptions)
    {
        bool ret = false;
        var genos = VectorUtils.TessellateScene(nodeScene, tessOptions);
        if (genos.Count > 0)
        {
            ret = true;
            genos[0].WorldTransform.m11 = -1;
            if ( -1 == _nodeIndex)
            {
                geoms.Add(genos[0]);
                _nodeIndex = geoms.Count - 1;
            }
            else
            {
                geoms[_nodeIndex] = genos[0];
            }  
        }
        return ret;
    }
    public bool Minus(float offset)
    {
        bool ret = false;
        do
        {
            if (nodeScene.Root == null)
            {
                break;
            }

            if (_curValue <= 0)
            {
                break;
            }

            _curValue -= offset;
            if (_curValue <= 0)
            {
                _curValue = 0;
            }
            else
            {
                ret = true;
            }
            _stroke.PatternOffset = _curValue;
        } while (false);
        return ret;
    }
}

public class WrapHanzSvg
{
    public SVGParser.SceneInfo             _scene;
    public List<Geometry>                  _geoms;
    public VectorUtils.TessellationOptions _option;

    public bool ImportSVG(TextReader textReader, float dpi = 0, float pixelsPerUnit = 1, int windowWidth = 0, int windowHeight = 0, bool clipViewport = false)
    {
        bool ret = false;
        try
        {
            _scene = SVGParser.ImportSVG(textReader, dpi, pixelsPerUnit, windowWidth, windowHeight, clipViewport);
            ret = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return ret;
    }

    public bool ImportSVG(TextReader textReader, ViewportOptions viewportOptions, float dpi = 0, float pixelsPerUnit = 1, int windowWidth = 0, int windowHeight = 0)
    {
        bool ret = false;
        try
        {
            _scene = SVGParser.ImportSVG(textReader, viewportOptions, dpi, pixelsPerUnit, windowWidth, windowHeight);
            ret = true;
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
        return ret;
    }
    public Sprite BuildSprite( Rect rect, float svgPixelsPerUnit, Alignment alignment, Vector2 customPivot, ushort gradientResolution, bool flipYAxis = false)
    {
        return VectorUtils.BuildSprite(_geoms, rect, svgPixelsPerUnit, alignment, customPivot, gradientResolution, flipYAxis);
    }

    public Sprite BuildSprite( float svgPixelsPerUnit, Alignment alignment, Vector2 customPivot, ushort gradientResolution, bool flipYAxis = false)
    {
        return VectorUtils.BuildSprite(_geoms, svgPixelsPerUnit, alignment, customPivot, gradientResolution, flipYAxis);
    }
    public  void FillMesh(
       Mesh mesh,               // The mesh to fill, which will be cleared before filling
       float svgPixelsPerUnit,  // How many "SVG units" should fit in a "Unity unit"
       bool flipYAxis = false) // If true, the Y-axis will point downward
    {
        VectorUtils.FillMesh(mesh, _geoms, svgPixelsPerUnit, flipYAxis);
    }
    public void Reset(List<float> list_initValue)
    {
        for(int i = 0; i < list_initValue.Count; i ++)
        {
            SceneNode nd = GetPathIndex(i);
            if (null != nd)
            {
                foreach (var shape in nd.Shapes)
                {
                    if (shape.PathProps.Stroke != null)
                    {
                        shape.PathProps.Stroke.PatternOffset = list_initValue[i];
                    }
                }
            }

        }
        _geoms = VectorUtils.TessellateScene(_scene.Scene, _option);
    }
    public List<Geometry> BuildGeoms(VectorUtils.TessellationOptions tessOptions)
    {
        _option = tessOptions;
        _geoms = VectorUtils.TessellateScene(_scene.Scene, tessOptions);
        return _geoms;
    }

    public SceneNode GetPathIndex(int index)
    {
        string pathId = string.Format("make-me-a-hanzi-animation-{0}", index);
        if (_scene.NodeIDs.ContainsKey(pathId))
        {
            return _scene.NodeIDs[pathId];
        }
        return null;
    }

    public Color SetPathColor(int index, Color clr)
    {
        Color ret = clr;
        if (index >=0 && index < _geoms.Count)
        {
            ret = _geoms[index].Color;
            _geoms[index].Color = clr;
        }
        return ret;
    }
   
}

