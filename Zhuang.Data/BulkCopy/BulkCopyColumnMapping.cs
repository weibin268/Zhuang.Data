using System;
using System.Collections.Generic;
using System.Text;

namespace Zhuang.Data.BulkCopy
{
    public class BulkCopyColumnMapping
    {

        internal string _destinationColumnName;
        internal int _destinationColumnOrdinal;
        internal string _sourceColumnName;
        internal int _sourceColumnOrdinal;

        // devnote: we don't want the user to detect the columnordinal after WriteToServer call.
        // _sourceColumnOrdinal(s) will be copied to _internalSourceColumnOrdinal when WriteToServer executes.
        internal int _internalDestinationColumnOrdinal;
        internal int _internalSourceColumnOrdinal;   // -1 indicates an undetermined value

        public string DestinationColumn
        {
            get
            {
                if (_destinationColumnName != null)
                {
                    return _destinationColumnName;
                }
                return string.Empty;
            }
            set
            {
                _destinationColumnOrdinal = _internalDestinationColumnOrdinal = -1;
                _destinationColumnName = value;
            }
        }

        public int DestinationOrdinal
        {
            get
            {
                return _destinationColumnOrdinal;
            }
            set
            {
                if (value >= 0)
                {
                    _destinationColumnName = null;
                    _destinationColumnOrdinal = _internalDestinationColumnOrdinal = value;
                }
                else {
                    throw new Exception(string.Format("IndexOutOfRange({0}", value));
                }
            }
        }

        public string SourceColumn
        {
            get
            {
                if (_sourceColumnName != null)
                {
                    return _sourceColumnName;
                }
                return string.Empty;
            }
            set
            {
                _sourceColumnOrdinal = _internalSourceColumnOrdinal = -1;
                _sourceColumnName = value;
            }
        }

        public int SourceOrdinal
        {
            get
            {
                return _sourceColumnOrdinal;
            }
            set
            {
                if (value >= 0)
                {
                    _sourceColumnName = null;
                    _sourceColumnOrdinal = _internalSourceColumnOrdinal = value;
                }
                else {
                    throw new Exception(string.Format("IndexOutOfRange({0}", value));
                }
            }
        }

        public BulkCopyColumnMapping()
        {
            _internalSourceColumnOrdinal = -1;
        }

        public BulkCopyColumnMapping(string sourceColumn, string destinationColumn)
        {
            SourceColumn = sourceColumn;
            DestinationColumn = destinationColumn;
        }

        public BulkCopyColumnMapping(int sourceColumnOrdinal, string destinationColumn)
        {
            SourceOrdinal = sourceColumnOrdinal;
            DestinationColumn = destinationColumn;
        }

        public BulkCopyColumnMapping(string sourceColumn, int destinationOrdinal)
        {
            SourceColumn = sourceColumn;
            DestinationOrdinal = destinationOrdinal;
        }

        public BulkCopyColumnMapping(int sourceColumnOrdinal, int destinationOrdinal)
        {
            SourceOrdinal = sourceColumnOrdinal;
            DestinationOrdinal = destinationOrdinal;
        }
    }
}
