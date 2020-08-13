using System;
using System.Collections.Generic;
using Core.Entities.Admin;

namespace API.Dtos
{
    public class DLToForwardToAssociatesDto
    {
        public DateTime DateForwarded {get; set; }
        public int EnqId {get; set; }
        public IReadOnlyList<IdInt> EnqItemIds {get; set;}
        public string mode {get; set;}
        public IReadOnlyList<IdInt> OfficialIds {get; set; }

    }
}