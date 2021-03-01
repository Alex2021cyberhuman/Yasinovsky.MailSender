namespace Yasinovsky.MailSender.Services.Test
{
    public class Person
    {
        public Person()
        {
            
        }

        public static Person Parse(string csvString, string sep = ",")
        {
            var data = csvString.Split(sep);
            Person person = new();
            person.Id = int.Parse(data[0]);
            person.Name = data[1];
            return person;
        }

        public int Id { get; set; }

        public string Name { get; set; }

        public string ToString(string sep) => $"{Id}{sep}{Name}";
    }
}