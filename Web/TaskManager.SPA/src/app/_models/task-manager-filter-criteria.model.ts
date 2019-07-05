export interface ITaskManagerFilterCriteria {
    ParentTaskId: number;
    TaskName: string;
    StartDate: string;
    EndDate: string;
    PriorityFrom: number;
    PriorityTo: number;
    IsTaskComplete: boolean;
}
