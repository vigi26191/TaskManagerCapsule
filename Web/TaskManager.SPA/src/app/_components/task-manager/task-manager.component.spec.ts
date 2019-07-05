import { ComponentFixture, TestBed } from '@angular/core/testing';
import { RouterTestingModule } from '@angular/router/testing';


import { ITaskManagerModel } from 'src/app/_models/task-manager.model';
import { ReactiveFormsModule, FormsModule } from '@angular/forms';
import { BsDatepickerModule } from 'ngx-bootstrap/datepicker';
import { HttpClientModule } from '@angular/common/http';
import { TASK_MANAGER_CONSTANTS } from 'src/app/_constants/task-manager.constant';

import { TaskManagerComponent } from './task-manager.component';

describe('TaskManagerComponent', () => {
  let component: TaskManagerComponent;
  let fixture: ComponentFixture<TaskManagerComponent>;
  let mockTaskManagerService;

  beforeEach(() => {
    TestBed.configureTestingModule({
      imports: [RouterTestingModule, ReactiveFormsModule, FormsModule, BsDatepickerModule,
        HttpClientModule],
      declarations: [TaskManagerComponent],
    });

    fixture = TestBed.createComponent(TaskManagerComponent);

    mockTaskManagerService = jasmine.createSpyObj(['getTasks', 'filterTaskManagerData',
      'saveTaskManager', 'endTask']);

    component = fixture.componentInstance;
    component.ngOnInit();

  });

  it('form invalid when empty', () => {
    expect(component.TaskManagerForm.valid).toBeFalsy();
  });

  it('should set add section to true when sectionname parameter is passed as CONST_ADD_TASK',
    () => {
      component.displaySection(TASK_MANAGER_CONSTANTS.CONST_ADD_TASK);
      expect(component.showAddTaskSection).toBe(true);
      expect(component.showViewTaskSection).toBe(false);
    });

  it('should set view section to true when sectionname parameter is passed as CONST_VIEW_TASK',
    () => {
      component.displaySection(TASK_MANAGER_CONSTANTS.CONST_VIEW_TASK);
      expect(component.showAddTaskSection).toBe(false);
      expect(component.showViewTaskSection).toBe(true);
    });

  it('should set range config value based on parameter passed', () => {
    const testValue = 3;
    component.setValueToPrioritySlider(testValue);
    expect(component.rangeSliderConfig.value).toBe(testValue);
  });

  it('should display update section and bind model when a task is to be edited', () => {
    const record: ITaskManagerModel = {
      Id: 1, TaskName: 'Task1', Priority: 1, StartDate: new Date(), EndDate: new Date(),
      ParentTask: null, ParentTaskId: null, IsTaskComplete: false
    };

    component.taskManagerLookUp = [record];

    component.editTask(record);

    expect(component.showAddTaskSection).toBe(true);
    expect(component.showViewTaskSection).toBe(false);
    expect(component.taskManagerModel).toBe(record);
    expect(component.sectionButtonText).toBe(TASK_MANAGER_CONSTANTS.CONST_BTN_UPDATE_TASK);
    expect(component.taskManagerForm.get('TaskName').value).toEqual(record.TaskName);
    expect(component.taskManagerForm.get('Priority').value).toEqual(record.Priority);
    expect(component.taskManagerForm.get('StartDate').value).toEqual(record.StartDate);
    expect(component.taskManagerForm.get('EndDate').value).toEqual(record.EndDate);
    expect(component.taskManagerForm.get('ParentTaskId').value).toEqual(record.ParentTaskId);
  });

});
