using System;

namespace RConceptXP.Views
{
    public class TabDeletedEventArgs : EventArgs
    {
        public string Header { get; }

        public TabDeletedEventArgs(string header)
        {
            Header = header;
        }
    }
}
