using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace BusinessLogic
{
    public class ReaderService : IReaderService
    {
        private IRepository<Reader> ReaderRepository { get; set; }

        public ReaderService(IRepository<Reader> readerRepository)
        {
            ReaderRepository = readerRepository;
        }

        /// <summary>
        /// добавить читателя
        /// </summary>
        /// <param name="name">имя</param>
        /// <param name="address">адрес</param>
        public void AddReader(string name, string address)
        {
            Reader reader = new Reader(name, address);
            ReaderRepository.Add(reader);
        }

        /// <summary>
        /// удалить читателя
        /// </summary>
        /// <param name="readerId">id читателя</param>
        public void DeleteReader(int readerId)
        {
            var readerToDelete = ReaderRepository.ReadById(readerId);

            if (readerToDelete != null)
            {
                ReaderRepository.Delete(readerId);
            }
        }

        /// <summary>
        /// получить всех читателей
        /// </summary>
        /// <returns>список читателей</returns>
        public IEnumerable<Reader> GetAllReaders()
        {
            return ReaderRepository.ReadAll();
        }

        /// <summary>
        /// получить читателя
        /// </summary>
        /// <param name="readerId">id читателя</param>
        /// <returns>читатель</returns>
        public Reader GetReader(int readerId)
        {
            return ReaderRepository.ReadById(readerId);
        }
    }
}
