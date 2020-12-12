using UnityEngine;

namespace duoli
{
    public class BinDataMgr{
        public languageMgr _languageMgr = new languageMgr();
        public effectMgr _effectMgr = new effectMgr();
        public bushouMgr _bushouMgr = new bushouMgr();
        public jiegouMgr _jiegouMgr = new jiegouMgr();

        public bool LoadData( string path)
        {
            bool ret = false;
            do{

                if ( ! _languageMgr.LoadDefault ( path ) ){
                    Debug.Log("languageMgr failed");
                    break;
                }

                if ( ! _effectMgr.LoadDefault ( path ) ){
                    Debug.Log("effectMgr failed");
                    break;
                }

                if ( ! _bushouMgr.LoadDefault ( path ) ){
                    Debug.Log("bushouMgr failed");
                    break;
                }

                if ( ! _jiegouMgr.LoadDefault ( path ) ){
                    Debug.Log("jiegouMgr failed");
                    break;
                }                
                ret = true;
            }while(false);

            return ret;
        }
    } 
}
