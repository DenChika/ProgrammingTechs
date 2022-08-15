namespace Parser.ParserResults.Entities
{
    public class Owner
    {
        public string Name { get; set; }

        public string Date { get; set; }

        public LinkedList<Cat> _cats { get; set; }
    }
}