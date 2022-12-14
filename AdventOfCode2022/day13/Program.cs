class Program
{
    public static void Main(string[] args)
    {
        const string filePath = @"./input2.txt";

        if (!File.Exists(filePath))
        {
            Console.WriteLine("no file found");
            return;
        }

        var lines = File.ReadLines(filePath);

        var packetPairList = new List<PacketPair>();
        Packet? tempPacket = null;
        
        foreach (var line in lines)
        {
            if (line.Length > 0)
            {
                var newPacket = new Packet()
                {
                    Data = line
                };

                if (tempPacket != null)
                {
                    packetPairList.Add(new PacketPair()
                    {
                        FirstPacket = tempPacket,
                        SecondPacket = newPacket
                    });
                    tempPacket = null;
                }
                else
                {
                    tempPacket = newPacket;
                }
            }
        }

        ;
    }
    
    class PacketPair
    {
        public Packet FirstPacket;
        public Packet SecondPacket;
    }

    class Packet
    {
        public string Data { get; set; }
        private List<PacketElement> PacketElements = new List<PacketElement>();
        
        public void PopulateElementsList()
        {
           var rawDataWithoutBraces = Data[1..^1]; 
        }
    }

    class PacketElement
    {
        public EPacketElementType Type;
        public List<PacketElement> ChildElements;
    }

    enum EPacketElementType
    {
        List,
        Int
    }
}