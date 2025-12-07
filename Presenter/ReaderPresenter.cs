using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BusinessLogic;
using Model;
using Shared;

namespace Presenter
{
    internal class ReaderPresenter
    {
        private IReaderService ReaderLogic;
        private IReaderView View;

        public ReaderPresenter(IReaderService readerLogic, IReaderView view)
        {
            this.ReaderLogic = readerLogic;
            this.View = view;

            readerLogic.DataChanged += OnModelDataChanged;

            view.AddDataEvent += OnAddData;
            view.DeleteDataEvent += readerLogic.Delete;
            view.UpdateDataEvent += OnUpdateData;
            view.ReadByIdEvent += OnReadById;
        }

        private void OnModelDataChanged(IEnumerable<Reader> readers)
        {
            List<ReaderEventArgs> args = new List<ReaderEventArgs>();
            foreach (Reader reader in readers)
            {
                args.Add(new ReaderEventArgs()
                {
                    Name = reader.Name,
                    Address = reader.Address,
                });
            }
            View.Redraw(args);
        }

        private void OnAddData(EventArgs data)
        {
            ReaderEventArgs args = data as ReaderEventArgs;
            Reader reader = new Reader();
            reader.Id = args.Id;
            reader.Name = args.Name;
            reader.Address = args.Address;
            ReaderLogic.Add(reader);
        }

        private void OnUpdateData(EventArgs data)
        {
            ReaderEventArgs args = data as ReaderEventArgs;
            Reader reader = new Reader();
            reader.Id = args.Id;
            reader.Name = args.Name;
            reader.Address = args.Address;
            ReaderLogic.Update(reader);
        }

        private void OnReadById(int id)
        {
            Reader reader = ReaderLogic.GetReader(id);
            ReaderEventArgs args = new ReaderEventArgs()
            {
                Id = id,
                Name = reader.Name,
                Address = reader.Address
            };
            View.ShowReaderProfile(args);
        }
    }
}
