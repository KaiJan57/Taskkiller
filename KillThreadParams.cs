namespace Taskkiller
{
    class KillThreadParams
    {
        public bool Troll;
        public bool KillCompletely;
        public int Time;
        public string Name;

        public KillThreadParams(bool Troll, bool KillCompletely, int Time, string Name)
        {
            this.Troll = Troll;
            this.KillCompletely = KillCompletely;
            this.Time = Time;
            this.Name = Name;
        }
    }
}
