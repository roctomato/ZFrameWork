using UnityEngine;
using System.Collections;
using System;
using UnityEngine.Profiling;

public class ShowFPS : MonoBehaviour {

    [SerializeField]
    public Texture background;
    [SerializeField]
    public float UpdateInterval = 0.5F;

    private float lastInterval;

    private int frames = 0;

    private float fps;

    int initGCCount = 0;
    int totalCGCount = 0;
    int minuteGCCount = 0;
    int lastMinuteGCCount = 0;

    float m_LastUpdateTime = 0;

    long maxMemSize;

    void Start()
    {
        //Application.targetFrameRate=60;

        lastInterval = Time.realtimeSinceStartup;

        frames = 0;

        initGCCount = GC.CollectionCount(0);
        totalCGCount = 0;
        minuteGCCount = 0;
        lastMinuteGCCount = 0;

        m_LastUpdateTime = Time.time;
    }

    void OnGUI()
    {
        GUI.backgroundColor = Color.yellow;
        GUI.skin.label.fontSize = 32;
        GUI.color = Color.red;

        GUI.Label(new Rect(50, 50, 400, 200),string.Format("FPS:{0:f2}\nGC:{1} {2}\n{3}\n{4}", 
            fps, totalCGCount, lastMinuteGCCount, GetMonoStr(), GetMemoryStr()));
    }

    public string GetMemoryStr()
    {
        long memSize = Profiler.GetTotalAllocatedMemoryLong();
        if(maxMemSize < memSize)
        {
            maxMemSize = memSize;
        }

        return string.Format("Momery: {0} {1}", memSize / 1024 / 1024, maxMemSize / 1024 / 1024);
#if UNITY_ANDROID && !UNITY_EDITOR
            //return AndroidNative.GetUsedMemory();
#elif UNITY_IOS && !UNITY_EDITOR
            //return IOSNative.GetUsedMemory();
#else
        //return Profiler.GetTotalAllocatedMemoryLong();
#endif
    }

    string GetMonoStr()
    {
        return string.Format("Mono: {0:N1} {1}",
            Profiler.GetMonoUsedSizeLong() / 1024 / 1024.0f,
            Profiler.GetMonoHeapSizeLong() / 1024 / 1024);
    }
    
    void Update() 
    {
        ++frames;

        if (Time.realtimeSinceStartup > lastInterval + UpdateInterval) 
        {
            fps = frames / (Time.realtimeSinceStartup - lastInterval);

            frames = 0;

            lastInterval = Time.realtimeSinceStartup;
        }

        totalCGCount = GC.CollectionCount(0) - initGCCount;

        if (Time.time - m_LastUpdateTime > 60f)
        {
            minuteGCCount = totalCGCount - lastMinuteGCCount;
            lastMinuteGCCount = totalCGCount;
            m_LastUpdateTime = Time.time;
        }
    }
}