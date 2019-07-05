import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { environment } from '../../environments/environment';
import { Observable } from 'rxjs';
import { ITaskManagerModel } from 'src/app/_models/task-manager.model';
import { ITaskManagerFilterCriteria } from 'src/app/_models/task-manager-filter-criteria.model';

@Injectable({
  providedIn: 'root'
})
export class TaskManagerService {
  controllerRoute: string = environment.baseApiUrl + 'taskmanager';

  constructor(private http: HttpClient) { }

  getTasks(): Observable<ITaskManagerModel[]> {
    return this.http.get<ITaskManagerModel[]>(this.controllerRoute + '/lookupTaskManager');
  }

  filterTaskManagerData(filterCriteria: ITaskManagerFilterCriteria): Observable<ITaskManagerModel[]> {
    return this.http.post<ITaskManagerModel[]>(this.controllerRoute + '/filterTasks', filterCriteria);
  }

  saveTaskManager(taskManagerData: ITaskManagerModel): Observable<string> {
    return this.http.post<string>(this.controllerRoute + '/saveTask', taskManagerData);
  }

  endTask(taskId: number): Observable<string> {
    return this.http.post<string>(this.controllerRoute + '/endTask/' + taskId, taskId);
  }
}
