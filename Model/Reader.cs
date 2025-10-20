using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations.Schema;

namespace Model
{
    [Table("Readers", Schema = "public")]
    public class Reader : IDomainObject
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public int Id { get; set; }

        public Reader(string name, string address)
        {
            Name = name;
            Address = address;
        }

        public Reader() { }

        /// <summary>
        /// изменение имени
        /// </summary>
        /// <param name="name">новое имя</param>
        public void UpdateName(string name) => Name = name;

        /// <summary>
        /// изменение адреса
        /// </summary>
        /// <param name="address">новый адрес</param>
        public void UpdateAddress(string address) => Address = address;

        
    }
}
