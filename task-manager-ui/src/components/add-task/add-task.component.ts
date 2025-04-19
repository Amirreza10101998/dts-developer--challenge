import { Component } from '@angular/core';
import { AppComponent } from "../../app/app.component";
import { TaskComponent } from '../shared/task/task.component';

@Component({
  selector: 'app-add-task',
  imports: [TaskComponent],
  templateUrl: './add-task.component.html',
  styleUrl: './add-task.component.css'
})
export class AddTaskComponent {

}
