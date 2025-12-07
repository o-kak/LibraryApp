using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Model;

namespace BusinessLogic
{
    public interface IReaderService : IModel<Reader>
    {
        void Update(Reader reader);
        Reader GetReader(int readerId);
    }
}
