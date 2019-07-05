export interface ITaskManagerModel {
    Id: number;
    ParentTaskId: number;
    TaskName: string;
    StartDate: Date;
    EndDate: Date;
    Priority: number;
    IsTaskComplete: boolean;
    ParentTask: ITaskManagerModel;
}
