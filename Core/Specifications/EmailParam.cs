using System;

namespace Core.Specifications
{
    public class EmailParam
    {
        public int? Id {get; set; }
        
        public string Subject
        { 
            get => _subject; 
            set => _subject = value.ToLower(); 
        }
        public string SenderEmailAddress 
        { 
            get => _senderEmailAddress; 
            set => _senderEmailAddress = value.ToLower(); 
        }
        public string ToEmailAddress 
        { 
            get => _toEmailAddress; 
            set => _toEmailAddress = value.ToLower(); 
        }
        
        public string MessageType 
        { 
            get => _messageType; 
            set => _messageType = value.ToLower(); 
        }
        public DateTime? DateSent {get; set; }

        private string _messageType;
        private string _senderEmailAddress;
        private string _subject;
        private string _toEmailAddress;
        
        private const int MaxmPageSize = 50;
       
        public int PageIndex { get; set; } = 1;

        private int _pageSize = 10;

        public int PageSize
        {
            get => _pageSize;
            set => _pageSize = (value > MaxmPageSize) ? MaxmPageSize : value;
        }
        public string Sort { get; set; }

    }
}