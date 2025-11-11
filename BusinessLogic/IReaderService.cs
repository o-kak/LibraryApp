using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BusinessLogic
{
    public interface IReaderService
    {
        void AddReader(string name, string address);
        void DeleteReader(int readerId);
        IEnumerable<Reader> GetAllReaders();
        Reader GetReader(int readerId);
    }
}
