using System;
using System.Linq.Expressions;
using Core.Entities.Admin;
using Core.Enumerations;

namespace Core.Specifications
{
    public class TaskSpecsWithFiltersForCountSpec : BaseSpecification<ToDo>

    {
        public TaskSpecsWithFiltersForCountSpec(TaskSpecParams tParams) 
            :  base( x => (
                (string.IsNullOrEmpty(tParams.Search) || 
                    x.Owner.Person.FullName.ToLower().Contains(tParams.Search)) &&
                (!tParams.OwnerId.HasValue || x.OwnerId == tParams.OwnerId) &&
                (!tParams.AssignedToId.HasValue || x.AssignedToId == tParams.AssignedToId) &&
                (!tParams.TaskType.HasValue || x.TaskType == tParams.TaskType) &&
                (!tParams.TaskStatus.HasValue || x.TaskStatus == tParams.TaskStatus)) &&
                ((!tParams.DateRangeFrom.HasValue && !tParams.DateRangeUpto.HasValue) ||
                    x.TaskDate >= tParams.DateRangeFrom && x.TaskDate <= tParams.DateRangeUpto)
            )
        {
        }

        public TaskSpecsWithFiltersForCountSpec(int ownerId, enumTaskStatus taskStatus) 
            :  base( x => x.OwnerId == ownerId && x.TaskStatus == taskStatus)
        {
        }

        public TaskSpecsWithFiltersForCountSpec(enumTaskStatus taskStatus, int assignedToId) 
            :  base( x => x.AssignedToId == assignedToId && x.TaskStatus == taskStatus)
        {
        }
    }
}