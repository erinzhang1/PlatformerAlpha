using script.User_Control;
using UnityEngine;

namespace script.Level_Items_Script
{
    public class BaseFanScript : BaseLevelItemScript
    {
        public override void RemoveMe(UserControl uc)
        {
            uc.LevelItemList.Remove(this);
            uc.fanCount -= 1;
            Destroy(this.gameObject);
        }
    }
}