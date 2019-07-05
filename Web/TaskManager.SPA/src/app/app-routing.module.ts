import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { ROUTE_PATH } from './_constants/route-names.constant';
import { TaskManagerComponent } from './_components/task-manager/task-manager.component';

const routes: Routes = [
  { path: ROUTE_PATH.TASKMANAGER, component: TaskManagerComponent }
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule]
})
export class AppRoutingModule { }
