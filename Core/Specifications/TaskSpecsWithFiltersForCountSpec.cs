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
                    x.Owner.FullName.ToLower().Contains(tParams.Search)) &&
                (!tParams.OwnerId.HasValue || x.OwnerId == tParams.OwnerId) &&
                (!tParams.AssignedToId.HasValue || x.AssignedToId == tParams.AssignedToId) &&
                (x.TaskType.ToLower() == tParams.TaskType.ToLower()) &&
                (x.TaskStatus.ToLower() == tParams.TaskStatus.ToLower())) &&
                ((!tParams.DateRangeFrom.HasValue && !tParams.DateRangeUpto.HasValue) ||
                    x.TaskDate >= tParams.DateRangeFrom && x.TaskDate <= tParams.DateRangeUpto)
            )
        {
        }

        public TaskSpecsWithFiltersForCountSpec(int ownerId, string taskStatus) 
            :  base( x => x.OwnerId == ownerId && x.TaskStatus.ToLower() == taskStatus.ToLower())
        {
        }

        public TaskSpecsWithFiltersForCountSpec(string taskStatus, int assignedToId) 
            :  base( x => x.AssignedToId == assignedToId && x.TaskStatus.ToLower() == taskStatus.ToLower())
        {
        }
    }
}