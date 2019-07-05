import { Component, OnInit } from '@angular/core';
import { FormBuilder, Validators, FormGroup } from '@angular/forms';
import { ITaskManagerModel } from 'src/app/_models/task-manager.model';
import { TaskManagerService } from 'src/app/_services/task-manager.service';
import { ITaskManagerFilterCriteria } from 'src/app/_models/task-manager-filter-criteria.model';
import { BsDatepickerConfig } from 'ngx-bootstrap/datepicker';
import { TASK_MANAGER_CONSTANTS } from 'src/app/_constants/task-manager.constant';
import { TASK_MANAGER_MESSAGES } from 'src/app/_messages/task-manager.message';

@Component({
  selector: 'app-task-manager',
  templateUrl: './task-manager.component.html',
  styleUrls: ['./task-manager.component.css']
})
export class TaskManagerComponent implements OnInit {
  taskManagerForm: FormGroup;
  taskManagerModel: ITaskManagerModel;
  filteredTasks: ITaskManagerModel[] = null;

  taskManagerLookUp: ITaskManagerModel[];
  parentTasksLookUp: ITaskManagerModel[];

  showAddTaskSection = false;
  showViewTaskSection = false;

  sectionButtonText = TASK_MANAGER_CONSTANTS.CONST_BTN_ADD_TASK;

  taskManagerId: number;
  taskManagerParentId: number;

  taskManagerFilterCriteria: ITaskManagerFilterCriteria = {
    ParentTaskId: null,
    TaskName: null,
    StartDate: null,
    EndDate: null,
    PriorityFrom: null,
    PriorityTo: null,
    IsTaskComplete: null
  };

  datePickerConfig: Partial<BsDatepickerConfig>;

  rangeSliderConfig = { min: 0, max: 30, value: 0, step: 1 };
  rangeSliderTickMarks: number[] = [];

  constructor(
    private fb: FormBuilder,
    private taskManagerService: TaskManagerService
  ) {
    this.datePickerConfig = Object.assign({}, {
      showWeekNumbers: false,
      dateInputFormat: 'DD-MMM-YYYY'
    });

    for (let index = this.rangeSliderConfig.min; index <= this.rangeSliderConfig.max; index++) {
      this.rangeSliderTickMarks.push(index);
    }
  }

  ngOnInit() {
    this.buildForm();

    this.taskManagerForm.get('Priority').valueChanges.subscribe(val => {
      if (val != null) { this.setValueToPrioritySlider(val); }
    });

    this.getAllTasks();

    this.displaySection(TASK_MANAGER_CONSTANTS.CONST_VIEW_TASK);
  }

  buildForm(): void {
    this.taskManagerForm = this.fb.group({
      TaskName: [null, Validators.required],
      Priority: [null],
      ParentTaskId: [null],
      StartDate: [null, Validators.required],
      EndDate: [null, Validators.required]
    });
  }

  get TaskManagerForm() {
    return this.taskManagerForm.controls;
  }

  getAllTasks(): void {
    this.taskManagerService.getTasks()
      .subscribe(response => {
        this.taskManagerLookUp = response;
      },
        (error) => { alert(error); },
        () => { this.setDefaults(); });
  }

  submitTaskManagerForm(taskManagerForm: FormGroup): void {
    if (this.validateTaskManagerForm(taskManagerForm)) {
      const taskManagerRequest = this.taskManagerForm.getRawValue();

      if (this.taskManagerModel != null && this.taskManagerModel.Id != null && this.taskManagerModel.Id > 0) {
        taskManagerRequest.Id = this.taskManagerModel.Id;
      }

      this.taskManagerService.saveTaskManager(taskManagerRequest)
        .subscribe(response => {
          alert(response);
          this.displaySection(TASK_MANAGER_CONSTANTS.CONST_VIEW_TASK);
        },
          (error) => {
            alert(error.error.Message);
          },
          () => { this.getAllTasks(); });
    }
  }

  validateTaskManagerForm(form: FormGroup): boolean {

    if (form.invalid) {
      Object.keys(this.TaskManagerForm).forEach(key => {
        this.TaskManagerForm[key].markAsTouched();
      });
      alert(TASK_MANAGER_MESSAGES.MSG_INVALID_FORM);
      return false;
    }

    const taskStartDate = form.get('StartDate').value;
    const taskEndDate = form.get('EndDate').value;
    if (taskStartDate > taskEndDate) {
      alert(TASK_MANAGER_MESSAGES.MSG_DATE_VALIDATION);
      return false;
    }

    return true;
  }

  filterTasks(): void {
    if (this.taskManagerFilterCriteria != null) {
      this.taskManagerService.filterTaskManagerData(this.taskManagerFilterCriteria)
        .subscribe(response => {
          this.filteredTasks = response;
        },
          error => {
            alert(error);
          });
    } else {
      alert(TASK_MANAGER_MESSAGES.MSG_EMPTY_FILTER_CRITERIA);
    }
  }

  editTask(record: ITaskManagerModel): void {
    this.setDefaults();

    this.displaySection(TASK_MANAGER_CONSTANTS.CONST_ADD_TASK);

    this.sectionButtonText = TASK_MANAGER_CONSTANTS.CONST_BTN_UPDATE_TASK;

    this.parentTasksLookUp = [];
    this.taskManagerLookUp.forEach(task => {
      if (task.Id != record.Id && (this.checkParentTaskReference(task, record.Id) == true)) {
        this.parentTasksLookUp.push(task);
      }
    });

    this.taskManagerModel = record;

    this.setValueToPrioritySlider(record.Priority);

    this.taskManagerForm.patchValue({
      TaskName: record.TaskName,
      Priority: record.Priority,
      ParentTaskId: record.ParentTaskId,
      StartDate: new Date(record.StartDate),
      EndDate: new Date(record.EndDate)
    });
  }

  checkParentTaskReference(task: ITaskManagerModel, recordId: number): boolean {
    while (task != null) {
      if (task.Id == recordId) {
        return false;
      }

      return this.checkParentTaskReference(task.ParentTask, recordId);
    }

    return true;
  }

  endTask(taskId: number): void {
    if (confirm(TASK_MANAGER_MESSAGES.MSG_END_TASK_CONFIRMATION)) {
      this.taskManagerService.endTask(taskId)
        .subscribe(response => {
          alert(response);
          this.filterTasks();
        },
          error => {
            alert(error);
          });
    }
  }

  resetTaskManagerFilterCriteria(): void {
    this.filteredTasks = null;
    this.taskManagerFilterCriteria = {
      ParentTaskId: null,
      TaskName: null,
      StartDate: null,
      EndDate: null,
      PriorityFrom: null,
      PriorityTo: null,
      IsTaskComplete: null
    };
  }

  displaySection(sectionName: string): void {
    if (sectionName == TASK_MANAGER_CONSTANTS.CONST_ADD_TASK) {
      this.showAddTaskSection = true;
      this.showViewTaskSection = false;
    }

    if (sectionName == TASK_MANAGER_CONSTANTS.CONST_VIEW_TASK) {
      this.setDefaults();
      this.showAddTaskSection = false;
      this.showViewTaskSection = true;
    }
  }

  setDefaults(): void {
    this.taskManagerForm.reset();
    this.resetTaskManagerFilterCriteria();
    this.setValueToPrioritySlider(0);
    this.sectionButtonText = TASK_MANAGER_CONSTANTS.CONST_BTN_ADD_TASK;

    this.parentTasksLookUp = [];
    this.parentTasksLookUp = this.taskManagerLookUp;

    this.taskManagerModel = {
      Id: null,
      ParentTaskId: null,
      TaskName: null,
      StartDate: null,
      EndDate: null,
      Priority: null,
      IsTaskComplete: null,
      ParentTask: null
    };
  }

  setValueToPrioritySlider(val: number): void {
    this.rangeSliderConfig.value = val;
  }

}
