using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Model
{
    [Table("Books", Schema = "public")]
    public class Book : IDomainObject
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public string Genre { get; set; }
        public bool IsAvailable { get; set; }

        public int? ReaderId { get; set; }

        public Reader Reader { get; set; }

        public Book(string title, string author, string genre)
        {
            Title = title;
            Author = author;
            Genre = genre;
            IsAvailable = true;
        }
        public Book() { }
        public override string ToString()
        {
            return Title; 
        }

        /// <summary>
        /// изменение статуса(доступна ли книга в фонде библотеки)
        /// </summary>
        /// <param name="newStatus">новый статус</param>
        public void UpdateAvailability(bool newStatus) => IsAvailable = newStatus;
    }
}
