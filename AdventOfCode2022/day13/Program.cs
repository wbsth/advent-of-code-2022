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

        var lines = File.ReadAllText(filePath);

        string[] pairs = lines.Split("\n\n");

        List<List<object>> allPacketList = new List<List<object>>();
        
        int count = 0;
        for (var index = 0; index < pairs.Length; index++)
        {
            var pair = pairs[index];
            var packets = pair.Split("\n");

            var pair0 = Packet.ParsePacket(packets[0]);
            var pair1 = Packet.ParsePacket(packets[1]);
        
            allPacketList.Add(pair0);
            allPacketList.Add(pair1);
            
            if (Packet.ComparePackets(pair0, pair1) <= 0)
            {
                count += index+1;
                Console.WriteLine(index+1);
            }
        }
    
        Console.WriteLine($"Sum: {count}");

        var dividerPacket1 = Packet.ParsePacket("[[2]]");
        var dividerPacket2 = Packet.ParsePacket("[[6]]");
        
        allPacketList.Add(dividerPacket1);
        allPacketList.Add(dividerPacket1);
        
        var sorted = BubbleSort(allPacketList.ToArray());
        static List<object>[] BubbleSort(List<object>[] arr)
        {
            int n = arr.Length;
            for (int i = 0; i < n - 1; i++)
            for (int j = 0; j < n - i - 1; j++)
                if (Packet.ComparePackets(arr[j], arr[j+1]) > 0)
                {
                    // swap temp and arr[i]
                    (arr[j], arr[j + 1]) = (arr[j + 1], arr[j]);
                }

            return arr;
        }

        int multiplier = 1;
        for (var index = 0; index < sorted.Length; index++)
        {
            var item = sorted[index];
            if (item == dividerPacket1 || item == dividerPacket2)
            {
                multiplier *= (index+1);
            }
        }
        Console.WriteLine($"Multiplier: {multiplier}");
    }
    
    class Packet
    {
        public static List<object> ParsePacket(string text)
        {
            if (text[0] == '[' && text[^1] == ']')
            {
                text = text[1..^1];
            }

            var tempObjectList = new List<object>();
        
            var tempCharList = new List<char>();

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
                tempObjectList.Add(integer);
            }

            void AddToList(string txt)
            {
                var temp = ParsePacket(txt);
                tempObjectList.Add(temp);
            }

            return tempObjectList;
        }

        public static int ComparePackets(List<object> FirstPacket, List<object> SecondPacket)
        {
            var iterationLength = Math.Min(FirstPacket.Count, SecondPacket.Count);
            
            for (int i = 0; i < iterationLength; i++)
            {
                object objectFirst = FirstPacket[i];
                object objectSecond = SecondPacket[i];
                
                var result = CompareObjects(objectFirst, objectSecond);
                if (result < 0)
                {
                    return -1;
                }

                if (result > 0)
                {
                    return 1;
                }
            }

            return Math.Sign(FirstPacket.Count - SecondPacket.Count);
        }

        public static int CompareObjects(object FirstObject, object SecondObject)
        {
            var result = 0;
            return (FirstObject, SecondObject) switch
            {
                (int x, int y) => Math.Sign(x - y),
                (List<object> x, List<object> y) => ComparePackets(x, y),
                (List<object> x, int y) => ComparePackets(x, new List<object> { y }),
                (int x, List<object> y) => ComparePackets(new List<object> { x }, y),
                _ => result
            };
        }
    }
    
    enum EPacketElementType
    {
        List,
        Int
    }
}