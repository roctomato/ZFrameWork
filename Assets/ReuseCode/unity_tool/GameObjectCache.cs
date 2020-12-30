﻿using UnityEngine;
using System.Collections.Generic;

public class GameObjectCache
{
    List<GameObject> m_CacheList = new List<GameObject>();

    Transform m_Parent;
    GameObject m_Template;

    bool m_IsInited;
    bool m_AutoActive = true;
    bool m_AutoUnactive = true;
    bool m_SafeCheck = true;
    int m_MaxCount = 256;

    // 获取时是否自动SetActive(true)
    public bool autoActive { set { m_AutoActive = value; } }
    // 释放时是否自动SetActive(false)
    public bool autoUnactive { set { m_AutoUnactive = value; } }

    // 是否执行安全检测，执行安全检测会损失一定的性能
    // 一旦关闭安全检测，需要使用方保证对象安全
    public bool safeCheck { set { m_SafeCheck = value; } }

    public int count
    {
        get { return m_CacheList.Count; }
    }

    public int maxCount
    {
        get { return m_MaxCount; }
        set
        {
            m_MaxCount = value;
            if (m_MaxCount > 0 && m_MaxCount < m_CacheList.Count)
            {
                for (int i = m_MaxCount; i < m_CacheList.Count; ++i)
                {
                    if (m_CacheList[i] != null)
                    {
                        GameObject.Destroy(m_CacheList[i]);
                    }
                }

                m_CacheList.RemoveRange(m_MaxCount, m_CacheList.Count - m_MaxCount);
            }
        }
    }

    public void Init(Transform parent, GameObject template)
    {
        if (null == parent || null == template)
        {
            return;
        }

        m_Parent = parent;
        m_Template = template;

        m_IsInited = true;
    }

    public GameObject Get(Transform parent = null)
    {
        if (!m_IsInited)
        {
            Debug.LogErrorFormat(null,"the object has't inited.");
            return null;
        }

        GameObject obj = null;

        if (m_CacheList.Count > 0)
        {
            if (m_SafeCheck)
            {
                for (int i = m_CacheList.Count - 1; i >= 0; --i)
                {
                    if (m_CacheList[i] == null)
                    {
                        m_CacheList.RemoveAt(i);
                    }
                    else
                    {
                        obj = m_CacheList[i];
                        m_CacheList.RemoveAt(i);
                        break;
                    }
                }
            }
            else
            {
                int index = m_CacheList.Count - 1;
                obj = m_CacheList[index];
                m_CacheList.RemoveAt(index);
            }
        }

        if (obj == null)
        {
            obj = GameObject.Instantiate(m_Template.gameObject) as GameObject;
        }

        if (obj != null)
        {
            if (parent != null)
            {
                obj.transform.SetParent(parent, false);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = m_Template.transform.localRotation;
                obj.transform.localScale = m_Template.transform.localScale;
            }
            else
            {
                obj.transform.SetParent(m_Parent, false);
                obj.transform.localPosition = Vector3.zero;
                obj.transform.localRotation = m_Template.transform.localRotation;
                obj.transform.localScale = m_Template.transform.localScale;
            }

            if (m_AutoActive)
            {
                obj.SetActive(true);
            }
        }

        return obj;
    }

    public void Release(GameObject obj)
    {
        if (!m_IsInited)
        {
            Debug.LogErrorFormat(null,"the object has't inited.");
            return;
        }

        if (obj == null)
        {
            return;
        }

        if (m_SafeCheck && m_CacheList.Contains(obj))
        {
            return;
        }

        if (m_MaxCount > 0 && m_CacheList.Count >= m_MaxCount)
        {
            GameObject.Destroy(obj);
            return;
        }

        obj.transform.SetParent(m_Parent, false);

        if (m_AutoUnactive)
        {
            obj.SetActive(false);
        }
        else
        {
            obj.transform.localScale = Vector3.zero;
        }

        m_CacheList.Add(obj);
    }

    public void DestroyAll()
    {
        if (!m_IsInited)
        {
            Debug.LogErrorFormat(null,"the object has't inited.");
            return;
        }

        for (int i = m_CacheList.Count - 1; i >= 0; --i)
        {
            GameObject obj = m_CacheList[i];
            if (obj != null)
            {
                GameObject.Destroy(obj);
            }
        }

        m_CacheList.Clear();
    }

    public void Destroy(int capacity)
    {
        if (capacity <= 0)
        {
            DestroyAll();
            return;
        }

        if (capacity < m_CacheList.Count)
        {
            int removeCount = m_CacheList.Count - capacity;
            for (int i = 0; i < removeCount; ++i)
            {
                if (m_CacheList[i] != null)
                {
                    GameObject.Destroy(m_CacheList[i]);
                }
            }

            m_CacheList.RemoveRange(0, removeCount);
        }
    }
}