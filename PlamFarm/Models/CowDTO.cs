namespace PlamFarm.Models
{
    public class CowDTO
    {
        public Cow Cow { get; set; }
        public List<Cow> Cows { get; set; }
        public List<Pregnancy> Pregnancies { get; set; }
        public CowDTO(Cow cow, List<Pregnancy> pregnancies)
        {
            Cow = cow;
            Pregnancies = pregnancies;
        }

        public CowDTO(Cow cow, List<Cow> cows, List<Pregnancy> pregnancies)
        {
            Cow = cow;
            Cows = cows;
            Pregnancies = pregnancies;
        }

    }

    public class CowSoldDTO
    {
        public List<Cow> Cows { get; set; }
        public List<Sold> Solds { get; set; }
        public CowSoldDTO(List<Cow> cows, List<Sold> solds)
        {
            Cows = cows;
            Solds = solds;
        }
    }
}
