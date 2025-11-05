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
        private IRepository<Reader> _readerRepository { get; set; }

        public ReaderService(IRepository<Reader> readerRepository)
        {
            _readerRepository = readerRepository;
        }

        public void AddReader(string name, string address)
        {
            Reader reader = new Reader(name, address);
            _readerRepository.Add(reader);
        }

        public void DeleteReader(int readerId)
        {
            var readerToDelete = _readerRepository.ReadById(readerId);

            if (readerToDelete != null)
            {
                _readerRepository.Delete(readerId);
            }
        }

        public IEnumerable<Reader> GetAllReaders()
        {
            return _readerRepository.ReadAll();
        }

        public Reader GetReader(int readerId)
        {
            return _readerRepository.ReadById(readerId);
        }

    }
}
