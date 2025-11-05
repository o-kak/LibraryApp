using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DataAccessLayer;
using Model;

namespace BusinessLogic
{
    public class ReaderService
    {
        private IRepository<Reader> ReaderRepository { get; set; }

        public ReaderService(IRepository<Reader> readerRepository)
        {
            ReaderRepository = readerRepository;
        }

        public void AddReader(string name, string address)
        {
            Reader reader = new Reader(name, address);
            ReaderRepository.Add(reader);
        }

        public void DeleteReader(int readerId)
        {
            var readerToDelete = ReaderRepository.ReadById(readerId);

            if (readerToDelete != null)
            {
                ReaderRepository.Delete(readerId);
            }
        }

        public IEnumerable<Reader> GetAllReaders()
        {
            return ReaderRepository.ReadAll();
        }

        public Reader GetReader(int readerId)
        {
            return ReaderRepository.ReadById(readerId);
        }


    }
}
