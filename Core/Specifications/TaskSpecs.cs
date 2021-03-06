using System;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class TaskSpecs : BaseSpecification<ToDo>
    {
        
        public TaskSpecs(int ownerId, string taskStatus)
            : base(x => x.OwnerId == ownerId && x.TaskStatus.ToLower() == taskStatus.ToLower())
        {
            AddOrderByDescending(x => x.TaskDate);
        }

        public TaskSpecs(string taskStatus, int assignedToId)
            : base(x => x.AssignedToId == assignedToId && x.TaskStatus.ToLower() == taskStatus.ToLower())
        {
            AddOrderByDescending(x => x.TaskDate);
        }

        public TaskSpecs(string taskType, int ownerId, bool onlyHeaders)
            : base(x => x.OwnerId == ownerId && x.TaskType.ToLower() == taskType.ToLower())
        {
            if (!onlyHeaders) AddInclude(x => x.TaskItems);
            AddOrderByDescending(x => x.TaskDate);
        }

        public TaskSpecs(string taskType, string taskStatus, int ownerId)
            : base(x => x.OwnerId == ownerId && x.TaskStatus.ToLower() == taskStatus.ToLower())
        {
            AddOrderByDescending(x => x.TaskDate);
        }


        public TaskSpecs(int enquiryItemId, string taskType, 
            string taskStatus, bool onlyHeaders, int ownerId)
            : base(x => x.OwnerId == ownerId && x.TaskStatus.ToLower() == taskStatus.ToLower() && 
            x.EnquiryItemId == enquiryItemId)
        {
            if (!onlyHeaders) AddInclude(x => x.TaskItems);
        }

        public TaskSpecs(string taskType, string taskStatus)
            : base(x => x.TaskType.ToLower() == taskType.ToLower() && 
                x.TaskStatus.ToLower() == taskStatus.ToLower())
        {
            AddOrderByDescending(x => x.TaskDate);
        }

        public TaskSpecs(string taskType)
            : base(x => x.TaskType.ToLower() == taskType.ToLower())
        {
            AddOrderByDescending(x => x.TaskDate);
        }

        public TaskSpecs(string taskStatus, string dummy, string dummy2)
            : base(x => x.TaskStatus.ToLower() == taskStatus.ToLower())
        {
            AddOrderByDescending(x => x.TaskDate);
        }

        public TaskSpecs(TaskSpecParams tParams) 
            :  base( x => (
                (string.IsNullOrEmpty(tParams.Search) || 
                    x.Owner.FullName.ToLower().Contains(tParams.Search)) &&
                (!tParams.OwnerId.HasValue || x.OwnerId == tParams.OwnerId) &&
                (!tParams.AssignedToId.HasValue || x.AssignedToId == tParams.AssignedToId) &&
                (x.TaskType.ToLower() == tParams.TaskType.ToLower()) &&
                (x.TaskStatus.ToLower() == tParams.TaskStatus.ToLower())) &&
                ((!tParams.DateRangeFrom.HasValue && !tParams.DateRangeUpto.HasValue) ||
                    x.TaskDate >= tParams.DateRangeFrom && x.TaskDate <= tParams.DateRangeUpto)
            )
        {
            if (tParams.IncludeItems) AddInclude(x => x.TaskItems);

            if (tParams.PageSize != 0)
            {
                ApplyPaging(tParams.PageSize, tParams.PageSize * (tParams.PageIndex-1));

                if (!string.IsNullOrEmpty(tParams.Sort))
                {
                    switch (tParams.Sort)
                    {
                        case "OwnedByAsc":
                            AddOrderBy(x => x.Owner.FullName);
                            break;
                        case "OwnedByDesc":
                            AddOrderByDescending(x => x.Owner.FullName);
                            break;
                        case "AssignedToAsc":
                            AddOrderBy(x => x.AssignedTo.FullName);
                            break;
                        case "AssignedToDesc":
                            AddOrderByDescending(x => x.AssignedTo.FullName);
                            break;
                        case "TaskDateAsc":
                            AddOrderBy(x => x.TaskDate);
                            break;
                        case "TaskDateDesc":
                            AddOrderByDescending(x => x.TaskDate);
                            break;
                        case "TaskTypeAsc":
                            AddOrderBy(x => x.TaskType);
                            break;
                        case "TaskTypeDesc":
                            AddOrderByDescending(x => x.TaskType);
                            break;
                        case "TaskStatusAsc":
                            AddOrderBy(x => x.TaskStatus);
                            break;
                        case "TaskStatusDesc":
                            AddOrderByDescending(x => x.TaskStatus);
                            break;
                        default:
                            AddOrderByDescending(x => x.TaskDate);
                            break;
                    }
                }
            }
        }
        public TaskSpecs(int enquiryItemId, int assignedToId, string taskType)
            : base(x => (x.EnquiryItemId==enquiryItemId && 
                        x.AssignedToId==assignedToId &&
                        x.TaskType.ToLower()==taskType.ToLower()))
        {
        }
        
        public TaskSpecs(int enquiryItemId, int assignedToId, string taskType, 
            string taskStatus)
            : base(x => (x.EnquiryItemId==enquiryItemId && 
                        x.AssignedToId==assignedToId &&
                        x.TaskType.ToLower()==taskType.ToLower() && x.TaskStatus.ToLower()==taskStatus.ToLower()))
        {
        }
    }
}