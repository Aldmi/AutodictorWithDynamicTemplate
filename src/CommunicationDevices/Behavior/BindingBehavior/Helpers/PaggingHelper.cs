using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reactive.Subjects;
using System.Timers;
using CommunicationDevices.DataProviders;
using CommunicationDevices.Devices;

namespace CommunicationDevices.Behavior.BindingBehavior.Helpers
{
    public class PaggingHelper : IDisposable
    {
        private readonly Timer _timer;
        private int _currentPage = 0;



        public int CountPaging { get; }                                    // кол-во страниц.      
        public List<UniversalInputType> PagingBuffer { get; set; } = new List<UniversalInputType>();



        public ISubject<PagingList> PagingListSend { get; } = new Subject<PagingList>();



        public PaggingHelper(int timeDispalayPage, int countPaging)
        {
            CountPaging = countPaging;

            _timer = new Timer(timeDispalayPage);
            _timer.Elapsed += OnTimedEvent;
            _timer.Enabled = true;
        }





        private void OnTimedEvent(object sender, ElapsedEventArgs e)
        {
            var pagingList= new PagingList();
            var inData = new UniversalInputType { TableData = PagingBuffer };

            if (CountPaging >= PagingBuffer.Count)
            {
                pagingList.CurrentPage = 0;
                pagingList.List = PagingBuffer;
                PagingListSend.OnNext(pagingList);
                return;
            }

            var numberOfPage = PagingBuffer.Count / CountPaging;
            if (_currentPage < numberOfPage)
            {
                var page = PagingBuffer.Skip(_currentPage * CountPaging).Take(CountPaging).ToList();
                pagingList.List = page;
            }
            else
            {
                var remainingElem = PagingBuffer.Count - (_currentPage * CountPaging);
                var page = PagingBuffer.Skip(_currentPage * CountPaging).Take(remainingElem).ToList();
                pagingList.List = page;
            }


            pagingList.CurrentPage = _currentPage;
            PagingListSend.OnNext(pagingList);


            if (++_currentPage > numberOfPage)
                _currentPage = 0;
        }




        #region Disposable

        public void Dispose()
        {
            _timer?.Dispose();
        }

        #endregion
    }




    public class PagingList
    {
        public List<UniversalInputType> List { get; set; }
        public int CurrentPage { get; set; }
    }
}