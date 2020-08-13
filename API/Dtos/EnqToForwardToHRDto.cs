using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace API.Dtos
{
    public class EnqToForwardToHRDto
    {
        [Required]
        public int enquiryId {get; set; }
        [Required]
        public EnqHeaderToForwardDto enqToForwardDto {get; set; }
        public IReadOnlyList<EnqItemForwardDto> EnqItemsToFwdDto { get; set; }
    }
}