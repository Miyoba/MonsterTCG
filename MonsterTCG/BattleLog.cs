using System;

namespace MonsterTCG
{
    public class BattleLog
    {
        public BattleLog()
        {
            Log = "";
        }

        public String Log { get; set; }
        public User Winner { get; set; }

        public void AddRoundText(string text)
        {
            Log += text+"\n";
        }
    }
}