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
        /// <summary>
        /// Обновление данных читателя.
        /// </summary>
        /// <param name="reader">читатель</param>
        void Update(Reader reader);

        /// <summary>
        /// чтение читателя по Id
        /// </summary>
        /// <param name="readerId">Id читателя</param>
        /// <returns></returns>
        Reader GetReader(int readerId);
    }
}
