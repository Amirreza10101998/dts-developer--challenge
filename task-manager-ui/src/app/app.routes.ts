import { Routes } from '@angular/router';
import { TaskListComponent } from '../components/task-list/task-list.component';
import { TaskComponent } from '../components/shared/task/task.component';
import { AddTaskComponent } from '../components/add-task/add-task.component';
import { EditTaskComponent } from '../components/edit-task/edit-task.component';

export const routes: Routes = [
  { path: 'tasks', component: TaskListComponent },
  { path: 'tasks/add', component: AddTaskComponent},
  { path: 'tasks/edit/:id', component: EditTaskComponent },
  { path: '', redirectTo: '/tasks', pathMatch: 'full' }

];
