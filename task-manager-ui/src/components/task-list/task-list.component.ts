import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { TaskService } from '../../services/task.service';
import { TaskItem } from '../../models/taskItem';

@Component({
  selector: 'app-task-list',
  imports: [MatCardModule],
  standalone: true,
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent {

  constructor(private taskService: TaskService) {}
  tasks: TaskItem[] = [];
  isLoading: boolean = false;

  ngOnInit() {
    this.loadTaskItems();
  };

  

  loadTaskItems() {
    this.isLoading = true;
    this.taskService.getAll().subscribe({
      next: (data) => {
        this.tasks = data;
        console.log('Tasks loaded:', this.tasks);
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading tasks:', error);
        this.isLoading = false;
      }
    });
  };
}
