using System;
using System.IO;
using System.Collections.Generic;
using Zby;

namespace duoli
{ 
    public  class  jiegou
    {
        public int	index; //索引
        public int	add; //追加结构伤害段数
        public string	xing; //名字
        public string	parent_name; //大类名称

        public void ReadFromBuff(Zby.ByteBuffer rd)
        {
            index = rd.ReadInt();
            add = rd.ReadInt();
            xing = rd.ReadString();
            parent_name = rd.ReadString();
        } 
    }

    public  class  jiegouMgr
    {
        public static string BinFile = "jiegou.bin";
        public Dictionary<int,jiegou> dataMap = new Dictionary<int,jiegou>(); 

        public bool LoadDefault( string path)
        {
            return LoadFromFile( path + BinFile);
        }
        
        public bool LoadFromFile( string fileName)
        {
            dataMap.Clear();
            using (var stream = new FileStream(fileName, FileMode.Open))
            {
                Zby.ByteBuffer readBuff = new Zby.ByteBuffer(stream);
                do{
                    jiegou newData = new jiegou();
                    try{
                        newData.ReadFromBuff(readBuff);
                        dataMap.Add( newData.index, newData);
                    }catch(Exception e){
                        break;
                    }
                }while(true);
            }
            return dataMap.Count > 0;
        }

        public jiegou FindData(int key ){
            jiegou ret = null;
            if ( dataMap.ContainsKey(key)){
                ret = dataMap[key];
            }
            return ret;
        }
    }
}
