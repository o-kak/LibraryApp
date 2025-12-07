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
        public event Action<IEnumerable<Reader>> DataChanged;
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
        public void Add(Reader reader)
        {
            ReaderRepository.Add(reader);
            InvokeDataChanged();
        }

        /// <summary>
        /// удалить читателя
        /// </summary>
        /// <param name="readerId">id читателя</param>
        public void Delete(int readerId)
        {
            var readerToDelete = ReaderRepository.ReadById(readerId);

            if (readerToDelete != null)
            {
                ReaderRepository.Delete(readerId);
            }
            InvokeDataChanged();
        }

        public void Update(Reader reader)
        {
            ReaderRepository.Update(reader);
            InvokeDataChanged();
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

        public void InvokeDataChanged()
        {
            DataChanged?.Invoke(new List<Reader>(ReaderRepository.ReadAll()));
        }
    }
}
