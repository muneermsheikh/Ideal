export interface IToDo
{
    id: number;
    enquiryId: number;
    enquiryItemId: number;
    ownerId: number;
    assignedToId: number;
    taskDate: string;
    completeBy: string;
    taskType: string;
    taskDescription: string;
    taskStatus: string;
    taskItems: ITaskItem[];
}

export interface ITaskItem{
    id: number;
    taskId: number;
    transDate: string;
    qntyConcluded: number;
    transactionDetail: string;
    createEMailMessage: boolean;
    remindOn: string;
    itemStatus: string;
}
