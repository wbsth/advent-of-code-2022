class Program
{
    public static void Main(string[] args)
    {
        const string filePath = @"./input.txt";

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
                var newPacket = new Packet(line);
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

        var indexSum = 0;
        for (var index = 0; index < packetPairList.Count; index++)
        {
            
            var packetPair = packetPairList[index];
            var a = Packet.ComparePackets(packetPair.FirstPacket, packetPair.SecondPacket);
            if (a == 1)
            {
                indexSum += index+1;
                Console.WriteLine($"{index}");
            }
        }
        Console.WriteLine($"Sum: {indexSum}");
    }
    class PacketPair
    {
        public Packet FirstPacket;
        public Packet SecondPacket;
    }

    class Packet
    {
        public Packet(string data)
        {
            ParsePacket(data);
            Data = data;
        }

        public readonly string Data;

        private List<int> Integers { get; set; } = new List<int>();
        private List<Packet> ChildPackets { get; set; } = new List<Packet>();

        private readonly Dictionary<int, (EPacketElementType, int)> _orderDictionary = new();

        public int LengthOverall => Integers.Count + ChildPackets.Count;
        
        public void ParsePacket(string text)
        {
            var cnt = 0;
            

            if (text[0] == '[' && text[^1] == ']')
            {
                text = text[1..^1];
            }
            
            List<int> integers = new List<int>();
            List<string> lists = new();
        
            List<char> tempCharList = new List<char>();

            var insideBracketCount = 0;
            for (var index = 0; index < text.Length; index++)
            {
                var character = text[index];
                if ((character == ',') && insideBracketCount == 0 && tempCharList.Count > 0)
                {
                    var integer = int.Parse(string.Join("", tempCharList));
                    AddInt(integer);
                    tempCharList.Clear();
                }
                else if (character == '[')
                {
                    tempCharList.Add('[');
                    insideBracketCount += 1;
                }
                else if (character == ']')
                {
                    tempCharList.Add(']');
                    insideBracketCount -= 1;

                    if (insideBracketCount == 0 && tempCharList.Any())
                    {
                        AddToList(string.Join("", tempCharList));
                        tempCharList.Clear();
                    }
                }
                else if (!(insideBracketCount == 0 && character == ','))
                {
                    tempCharList.Add(character);
                    if (index == text.Length - 1)
                    {
                        var integer = int.Parse(string.Join("", tempCharList));
                        AddInt(integer);
                        tempCharList.Clear();
                    }
                }
            }

            void AddInt(int integer)
            {
                integers.Add(integer);
                _orderDictionary.Add(cnt, (EPacketElementType.Int, integers.Count - 1));
                cnt += 1;
            }

            void AddToList(string txt)
            {
                lists.Add(txt);
                _orderDictionary.Add(cnt, (EPacketElementType.List, lists.Count - 1));
                cnt += 1;
            }
            
            Integers = integers;
            ChildPackets = lists.Select(x=>new Packet(x)).ToList();
        }

        public static int ComparePackets(Packet FirstPacket, Packet SecondPacket)
        {
            //Console.WriteLine($"Comparing {FirstPacket.Data} vs {SecondPacket.Data}");
            foreach (var pair in FirstPacket._orderDictionary)
            {
                var index = pair.Key;
                
                (EPacketElementType firstElementType, var firstTypeIndex) = pair.Value;
                
                // try to get item from the other packet
                if (true)//SecondPacket._orderDictionary.ContainsKey(index)
                {
                    (EPacketElementType secondElementType, var secondTypeIndex) = SecondPacket._orderDictionary[index];
                    
                    // if both values are ints
                    if (firstElementType == EPacketElementType.Int && secondElementType == EPacketElementType.Int)
                    {
                        //Console.WriteLine($"Comparing {FirstPacket.Integers[firstTypeIndex]} vs {SecondPacket.Integers[secondTypeIndex]}");
                        if (FirstPacket.Integers[firstTypeIndex] < SecondPacket.Integers[secondTypeIndex])
                        {
                            //Console.WriteLine("Left side smaller - OK");
                            return 1;
                        }
                        
                        if (FirstPacket.Integers[firstTypeIndex] > SecondPacket.Integers[secondTypeIndex])
                        {
                            //Console.WriteLine("Right side smaller - BAD");
                            return -1;
                        }
                    }

                    else
                    {
                        var tempFirstPacket = firstElementType == EPacketElementType.Int ?
                                              new Packet(FirstPacket.Integers[firstTypeIndex].ToString()) : FirstPacket;
                        
                        var tempSecondPacket = secondElementType == EPacketElementType.Int ?
                            new Packet(SecondPacket.Integers[secondTypeIndex].ToString()) : SecondPacket;

                        var retVal = ComparePackets(tempFirstPacket, tempSecondPacket);
                        if (retVal != 0)
                            return retVal;

                        if (tempFirstPacket.LengthOverall < tempSecondPacket.LengthOverall)
                            return 1;
                        if (tempFirstPacket.LengthOverall > tempSecondPacket.LengthOverall)
                            return -1;
                        return 0;
                    }
                    
                    // // if both values are packets
                    // else if (firstElementType == EPacketElementType.List && secondElementType == EPacketElementType.List)
                    // {
                    //     var retVal = ComparePackets(FirstPacket.ChildPackets[firstTypeIndex],
                    //         SecondPacket.ChildPackets[secondTypeIndex]);
                    //     
                    //     if(retVal != 0)
                    //         return retVal;
                    // }
                    //
                    // // if conversion is needed
                    // else if(firstElementType != secondElementType)
                    // {
                    //     if (firstElementType == EPacketElementType.Int)
                    //     {
                    //         Packet tempFirstPacket = new Packet(FirstPacket.Integers[firstTypeIndex].ToString());
                    //         var retVal = ComparePackets(tempFirstPacket, SecondPacket.ChildPackets[secondTypeIndex]);
                    //         if (retVal != 0)
                    //         {
                    //             return retVal;
                    //         }
                    //     }
                    //     
                    //     if (secondElementType == EPacketElementType.Int)
                    //     {
                    //         Packet tempSecondPacket = new Packet(SecondPacket.Integers[secondTypeIndex].ToString());
                    //         var retVal = ComparePackets(FirstPacket.ChildPackets[firstTypeIndex], tempSecondPacket);
                    //         if (retVal != 0)
                    //         {
                    //             return retVal;
                    //         }
                    //     }
                    // }
                }
                // else
                // {
                //     //Console.WriteLine("Right side run out of items");
                //     return -1; // right side run out of items, inputs not in right order
                // }
                    
            }

            return 0;
            // if (FirstPacket.ChildPackets.Count == 0 && SecondPacket.ChildPackets.Count == 0 &&
            //     FirstPacket.Integers.Count > 0 && SecondPacket.Integers.Count > 0)
            // {
            //     //Console.WriteLine("Elements equal");
            //     return 0;
            // }
            //
            // if (FirstPacket.ChildPackets.Count == 0 && SecondPacket.ChildPackets.Count == 0 &&
            //     FirstPacket.Integers.Count == 0 && SecondPacket.Integers.Count == 0)
            // {
            //     //Console.WriteLine("Elements equal");
            //     return 0;
            // }
            //
            // else 
            // {
            //     //Console.WriteLine("Left side run out of items");
            //     return 1;
            // }
        }
    }


    public static void ParsePacket(string text, out List<int> intList, out List<string> stringList)
    {
        var rawDataWithoutBraces = text[1..^1];
        List<int> integers = new List<int>();
        List<string> lists = new();
        
        List<char> tempCharList = new List<char>();

        var insideBracketCount = 0;
        for (var index = 0; index < rawDataWithoutBraces.Length; index++)
        {
            var character = rawDataWithoutBraces[index];
            if ((character == ',') && insideBracketCount == 0 && tempCharList.Count > 0)
            {
                var integer = int.Parse(string.Join("", tempCharList));
                integers.Add(integer);
                tempCharList.Clear();
            }
            else if (character == '[')
            {
                insideBracketCount += 1;
            }
            else if (character == ']')
            {
                insideBracketCount -= 1;

                if (insideBracketCount == 0 && tempCharList.Any())
                {
                    lists.Add(string.Join("", tempCharList));
                    tempCharList.Clear();
                }
            }
            else if (!(insideBracketCount == 0 && character == ','))
            {
                tempCharList.Add(character);
                if (index == rawDataWithoutBraces.Length - 1)
                {
                    var integer = int.Parse(string.Join("", tempCharList));
                    integers.Add(integer);
                    tempCharList.Clear();
                }
            }
        }

        intList = integers;
        stringList = lists;

    }
        
    enum EPacketElementType
    {
        List,
        Int
    }
}