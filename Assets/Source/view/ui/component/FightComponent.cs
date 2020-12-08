using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Zby;

public class FightComponent : CnUiComponent
{
    public Button btnSend;
    public InputField iptFight;
    
    public override bool DoInit()
    {
        bool ret = false;
        do
        {
            btnSend = GetElement<Button>("btnSend");
            if ( null == btnSend)
            {
                ZLog.E(Trans, "no btnSend");
                break;
            }

            iptFight = GetElement<InputField>("iptFight");
            if (null == iptFight)
            {
                ZLog.E(Trans, "no iptFight");
                break;
            }

            ret = true;
        } while (false);
        return ret;
    }
}
