namespace QLNS.Models
{
    public class BookImage
    {
        public int Id { get; set; }
        public string Url { get; set; }
        public int BookId { get; set; }
        public Book? Book { get; set; }

    }
}
