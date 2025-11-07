namespace MyLessons.ConverterSQLClass
{
    public class lesson
    {
        public string day {  get; set; }

        public string number { get; set; }

        public string less { get; set; }

        public string teacher { get; set; }

        public string clas { get; set; }

        public string room { get; set; }
        public lesson()
        {

        }
        public lesson(string day, string num, string less, string teacher, string clas, string room)
        {
            this.day = day;
            number = num;
            this.less = less;
            this.teacher = teacher;
            this.clas = clas;
            this.room = room;
        }
    }
}
