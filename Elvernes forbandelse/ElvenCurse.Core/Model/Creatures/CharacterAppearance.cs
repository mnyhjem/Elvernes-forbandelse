namespace ElvenCurse.Core.Model.Creatures
{
    public class CharacterAppearance
    {
        public Sex Sex { get; set; }
        public Body Body { get; set; }
        public Eyecolor Eyecolor { get; set; }
        public Nose Nose { get; set; }
        public Ears Ears { get; set; }

        public Facial Facial { get; set; }
        public Hair Hair { get; set; }

        public CharacterAppearance()
        {
            Facial = new Facial();
            Hair = new Hair();
        }
    }

    public class Hair
    {
        public HairType Type { get; set; }
        public HairColor Color { get; set; }

        public enum HairType
        {
            None = 0,
            Bangs = 1,
            Bangslong = 2,
            Bangslong2 = 3,
            Bangsshort = 4,
            Bedhead = 5,
            Bunches = 6,
            Jewfro = 7,
            Long = 8,
            Longhawk = 9,
            Longknot = 10,
            Loose = 11,
            Messy1 = 12,
            Messy2 = 13,
            Mohawk = 14,
            Page = 15,
            Page2 = 16,
            Parted = 17,
            Pixie = 18,
            Plain = 19,
            Ponytail = 20,
            Ponytail2 = 21,
            Princess = 22,
            ShortHawk = 23,
            ShortKnot = 24,
            Shoulderl = 25,
            Shoulderr = 26,
            Swoop = 27,
            Unkempt = 28,
            Xlong = 29,
            Xlongknot = 30
        }
    }

    public class Facial
    {
        public FacialType Type { get; set; }
        public HairColor Color { get; set; }

        public enum FacialType
        {
            None = 0,
            Beard = 1,
            Bigstache = 2,
            Fiveoclock = 3,
            Frenchstache = 4,
            Mustache = 5
        }
    }

    public enum HairColor
    {
        Black,
        Blonde,
        Blonde2,
        Blue,
        Blue2,
        Brown,
        Brown2,
        Brunette,
        Brunette2,
        Dark_blonde,
        Gold,
        Gray,
        Gray2,
        Green,
        Green2,
        Light_blonde,
        Light_blonde2,
        Pink,
        Pink2,
        Purple,
        Raven,
        Raven2,
        Redhead,
        Redhead2,
        Ruby_red,
        White,
        White_blonde,
        White_blonde2,
        White_Cyan
    }

    public enum Ears
    {
        Default = 0,
        Bigears = 1,
        Elvenears = 2
    }

    public enum Nose
    {
        Default = 0,
        Bignose = 1,
        Buttonnose = 2,
        Straightnose = 3
    }

    public enum Eyecolor
    {
        Blue = 0,
        Brown = 1,
        Gray = 2,
        Green = 3,
        Orange = 4,
        Purple = 5,
        Red = 6,
        Yellow = 7
    }

    public enum Sex
    {
        Male = 0,
        Female = 1
    }

    public enum Body
    {
        Dark = 0,
        Dark2 = 1,
        Darkelf = 2,
        Darkelf2 = 3,
        Light = 4,
        Orc = 5,
        Red_orc = 6,
        Tanned = 7,
        tanned2 = 8
    }
}
