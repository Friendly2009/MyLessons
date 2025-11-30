namespace MyLessons.ConverterSQLClass
{
    public class Data
    {
        public int Id { get; set; }
        public string text { get; set; }
        public string teacher { get; set; }
		public Data(int id)
        {
            Id = id;
            teacher = "";
            text = "";
		}
    }
}
