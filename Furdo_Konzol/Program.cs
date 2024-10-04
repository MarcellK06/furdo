using System;
using System.IO;
namespace Furdo_Konzol {
    class Data {
        public int id, sId;
        public bool inS;
        public TimeOnly time;
        public Data(int _id, int _sId, int _inS, int h, int m, int s) {
            id = _id;
            sId = _sId;
            inS = _inS == 1;
            time = new TimeOnly(h, m, s);
        }
        
    }
class Program {
    static void Main(string[] args) {

        var lines = File.ReadAllLines("furdoadat.txt");
        List<Data> data = new List<Data>();
        for(var k = 0; k < lines.Length; k++) {
            var line = lines[k].Split(" ");
            data.Add(new Data(int.Parse(line[0]), int.Parse(line[1]), int.Parse(line[2]),int.Parse(line[3]), int.Parse(line[4]), int.Parse(line[5])));
        }

        var sorted = data.OrderBy(a => a.time).ToList();
        var ids = data.Select(i => i.id).ToList();
        Console.WriteLine("Az első és az utolsó vendég kilépésének ideje:"); //Sovlve this
        Console.WriteLine(sorted.First(i => i.id == sorted.Min(i => i.id)).time.ToString());
        Console.WriteLine(sorted.First(i => i.id == sorted.Max(i => i.id)).time.ToString());

        var n = ids.Distinct().ToList();
        Console.WriteLine(n.Count);
        var enter = new Data(999,999,999,23,59,59);
        var exit = new Data(0,0,0,0,0,0);
        foreach(var id in n) {
            foreach(var d in data) {
                if (d.id == id && d.inS && d.time < enter.time) {
                    enter = d;
                }
                if (d.id == id && !d.inS && d.time > exit.time)
                    exit = d;
            }
            if (enter.id != exit.id){
                enter = new Data(999,999,999,23,59,59);
                exit = new Data(0,0,0,0,0,0);
                }
        }
        Console.WriteLine($"{enter.id} {(enter.time - exit.time).ToString()}");

        Dictionary<int, int> stats = new Dictionary<int, int>{
            {0, 0}, {1, 0}, {2, 0}
        };

        foreach(var d in data) {
                if (d.time > new TimeOnly(6,0,0) && d.time < new TimeOnly(8, 59, 59))
                    stats[0]++;
                if (d.time > new TimeOnly(9,0,0) && d.time < new TimeOnly(15, 59, 59))
                    stats[1]++;
                if (d.time > new TimeOnly(16,0,0) && d.time < new TimeOnly(19, 59, 59))
                    stats[2]++;
        }

        Console.WriteLine($"6:00:00 és 8:59:59 között {stats[0]}db | 9:00:00 és 15:59:59 között {stats[1]}db | 16:00:00 és 19:59:59 között {stats[2]}db");

        Dictionary<int, int> places = new Dictionary<int, int>{
            {0, 0}, {1, 0}, {2, 0}, {3, 0}, {4, 0}
        };

        for(var k = 0; k < data.Count; k++)
            if (!data[k].inS)
                places[data[k].sId]++;

        Console.WriteLine($"Uszoda: {places[1]}");
        Console.WriteLine($"Szauna: {places[2]}");
        Console.WriteLine($"Gyógyvizes Medence: {places[3]}");
        Console.WriteLine($"Strand: {places[4]}");
    }
}
}