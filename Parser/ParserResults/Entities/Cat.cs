namespace Parser.ParserResults.Entities
{
    public class Cat
    {
        public string Name { get; set; }

        public string Date { get; set; }

        public string Breed { get; set; }

        public Owner Owner { get; set; }

        public string Color { get; set; }

        public LinkedList<Cat> Friends { get; set; }
    }
}