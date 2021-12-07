namespace MartinFowler.Refactoring.Theatre.v9
{
    public class Performance
    {
        public string playId;
        public int audience;
        
        public Play play;
        public int amount;
        public int volumeCredits;
        
        public Performance Clone()
        {
            return new Performance
            {
                playId = playId,
                audience = audience,
                play = play,
                amount = amount,
                volumeCredits = volumeCredits
            };
        }
    }
}