using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace KémiaCSHARP
{
    public class Elem
    {
        public string ev { get; set; }
        public string elem { get; set; }
        public string vegyjel { get; set; }
        public int rendszam { get; set; }
        public string felfedezo { get; set; }

        public Elem(string ev, string elem, string vegyjel, int rendszam, string felfedezo)
        {
            this.ev = ev;
            this.elem = elem;
            this.vegyjel = vegyjel;
            this.rendszam = rendszam;
            this.felfedezo = felfedezo;
        }
    }

    class Program
    {
        static public List<Elem> lista = beolvas();
        static public List<Elem> beolvas()
        {
            List<Elem> list = new List<Elem>();
            try
            {
                using (StreamReader sr=new StreamReader(new FileStream("felfedezesek.csv",FileMode.Open),Encoding.UTF8))
                {
                    sr.ReadLine();
                    while (!sr.EndOfStream)
                    {
                        var split = sr.ReadLine().Split(';');
                        var o = new Elem(
                                split[0],
                                split[1],
                                split[2],
                                Convert.ToInt32(split[3]),
                                split[4]
                            );
                        list.Add(o);
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("Hiba a beolvasásnál! "+e.Message);
            }
            return list;
        }
        static void Main(string[] args)
        {
            #region 3. feladat
            Console.WriteLine("3. feladat: Felfedezések száma: "+lista.Count());
            #endregion

            #region 4. feladat
            Console.WriteLine("4. feladat: Felfedezések száma az ókorban: "+lista.Where(x=>x.ev.Equals("Ókor")).Count());
            #endregion

            #region 5. feladat
            bool isValid(String bemenet)
            {
                return Regex.IsMatch(bemenet, @"^[a-zA-Z]+$");
            }
            var beker = "";
            do
            {
                Console.Write("5. feladat: Kérek egy vegyjelet:");
                beker = Console.ReadLine();
            } while (!isValid(beker) || beker.Length>2);
            #endregion

            #region 6. feladat
            Console.WriteLine("6. feladat: Keresés");
            var elem = lista.SingleOrDefault(x=>x.vegyjel.ToUpper().Equals(beker.ToUpper()));
            if (elem==null)
            {
                Console.WriteLine("\tNincs ilyen elem az adatforrásban!");
            }
            else
            {
                Console.WriteLine("\tAz elem vegyjele: "+elem.vegyjel);
                Console.WriteLine("\tAz elem neve: "+elem.elem);
                Console.WriteLine("\tRendszáma: "+elem.rendszam);
                Console.WriteLine("\tFelfedezés éve: "+elem.ev);
                Console.WriteLine("\tFelfedező: "+elem.felfedezo);
            }
            #endregion

            #region 7. feladat
            var elozo = lista.Where(x => !x.ev.Equals("Ókor")).OrderBy(y=>y.ev).First().ev;
            var max = 0;
            foreach (var item in lista)
            {
                if (!item.ev.Equals("Ókor"))
                {
                    if (
                        Convert.ToInt32(item.ev) -
                        Convert.ToInt32(elozo)
                        > max
                    )
                    {
                        max = Convert.ToInt32(item.ev) - Convert.ToInt32(elozo);
                        elozo = item.ev;
                    }
                    else
                    {
                        elozo = item.ev;
                    }
                }
            }
            Console.WriteLine("7. feladat: {0} év volt a leghosszabb időszak két elem felfedezése között. ",max);
            #endregion

            #region 8. feladat
            Console.WriteLine("8. feladat: Statisztika");
            var evenkent = lista
                .GroupBy(x=>x.ev)
                .Select(y=>new
                {
                    ev=y.Key,
                    db=y.Count()
                }).ToList();

            evenkent.ForEach(x => {
                if (x.db>3 && !x.ev.Equals("Ókor"))
                {
                    Console.WriteLine("\t{0}: {1} db",x.ev,x.db);
                } 
            }
            );
            #endregion
            Console.ReadKey(); 
        }
    }
}
