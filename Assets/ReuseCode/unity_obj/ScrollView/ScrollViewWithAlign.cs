using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;
public class ScrollViewWithAlign : ScrollView
{
    [SerializeField]
    protected AlignType m_AlignType = AlignType.UpLeft;

    protected override Vector2 OnGetPos(int row, int col)
    {
        if (m_AlignType == AlignType.UpLeft)
        {
            return OnGetPos_AlignUpLeft(row, col);
        }
        else if (m_AlignType == AlignType.UpCenter)
        {
            return OnGetPos_AlignUpCenter(row, col);
        }
        else
        {
            return OnGetPos_AlignUpLeft(row, col);
        }
    }

    protected virtual Vector2 OnGetPos_AlignUpLeft(int row, int col)
    {
        int x = col * cellWidth + padding.left;
        int y = row * cellHeight + padding.top;

        return new Vector2(x + cellWidth * 0.5f, -y - cellHeight * 0.5f);
    }

    protected virtual Vector2 OnGetPos_AlignUpCenter(int row, int col)
    {
        if (this.vertical)
        {
            if (colCount <= 0)
            {
                return OnGetPos_AlignUpLeft(row, col);
            }

            int afterNum = cellCount - row * colCount;
            int curRowHasNum = afterNum >= colCount ? colCount : afterNum % colCount;
            float curRowWidth = curRowHasNum * cellWidth;
            float viewWidth = maxWidth;
            if (maxWidth <= 0)
            {
                viewWidth = Mathf.FloorToInt(this.viewport.rect.width);
            }
            float xStart = (viewWidth - padding.left - padding.right - curRowWidth) / 2 + padding.left;
            float x = xStart + col * cellWidth;
            float y = row * cellHeight + padding.top;
            return new Vector2(x + cellWidth * 0.5f, -y - cellHeight * 0.5f);
        }
        else
        {
            return base.OnGetPos(row, col);  // scrollView 并未处理水平方向
        }
    }

    public enum AlignType
    {
        UpLeft,
        UpCenter,
    }
}