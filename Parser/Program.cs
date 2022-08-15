
using Parser.ParserResults.ClientGens;
using Parser.ParserResults.Entities;
using Parser.MainParser;

namespace Parser
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var dirControllersPath = 
                "C://Users//volok//IdeaProjects//demo//src//main//java//com//example//demo//Controllers";
            var dirEntitiesPath =
                "C://Users//volok//IdeaProjects//demo//src//main//java//com//example//demo//Entities";
            var dirToGensPath = "C://Users//volok//RiderProjects//ServerParsing//Parser//ParserResults//ClientGens";
            var dirToEntitiesPath = "C://Users//volok//RiderProjects//ServerParsing//Parser//ParserResults//Entities";
            MainParser.MainParser mainParser = new MainParser.MainParser();
            mainParser.Distribute(dirToGensPath, dirControllersPath);
            mainParser.Distribute(dirToEntitiesPath, dirEntitiesPath);
            Cat cat1 = CatClient.createCat("bars", "01.10.2002", "British", "grey").Result;
            Cat cat2 = CatClient.createCat("barsik", "01.10.2002", "British", "black").Result;
            //cat1.Friends.AddLast(cat2);
            foreach (Cat cat in CatClient.getAll().Result)
            {
                Console.WriteLine(cat.Name);
                Console.WriteLine(cat.Date);
                Console.WriteLine(cat.Breed);
                Console.WriteLine(cat.Color);
            }

            Console.WriteLine("-----------");
            Cat cat3 = CatClient.deleteCat(cat1).Result;
            foreach (Cat cat in CatClient.getAll().Result)
            {
                Console.WriteLine(cat.Name);
                Console.WriteLine(cat.Date);
                Console.WriteLine(cat.Breed);
                Console.WriteLine(cat.Color);
            }
            /*foreach (Cat cat in CatClient.getFriends(cat1).Result)
            {
                Console.WriteLine(cat.Name);
                Console.WriteLine(cat.Date);
                Console.WriteLine(cat.Breed);
                Console.WriteLine(cat.Color);
            }*/
        }
    }
}
