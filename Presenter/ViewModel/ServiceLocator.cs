using BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Presenter.ViewModel
{
    public static class ServiceLocator
    {
        public static IBookService BookService { get; private set; }
        public static IReaderService ReaderService { get; private set; }
        public static ILoan LoanService { get; private set; }



        public static void Initialize(
            IBookService bookService,
            IReaderService readerService,
            ILoan loanService)
        {
            BookService = bookService;
            ReaderService = readerService;
            LoanService = loanService;

        }
    }
}
