import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { TaskService } from '../../services/task.service';
import { TaskItem } from '../../models/taskItem';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';

@Component({
  selector: 'app-task-list',
  imports: [MatCardModule, MatFormFieldModule, MatInputModule, MatIconModule, CommonModule, MatProgressSpinnerModule ],
  standalone: true,
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent {

  constructor(private taskService: TaskService) {}
  tasks: TaskItem[] = [];
  displayedColumns: string[] = ['status', 'title', 'dueDate'];
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

  navigateToAddTask() {
    window.location.href = '/tasks/add';
  }

  naviageteToEditTask(taskId: number) {
    window.location.href = `/tasks/edit/${taskId}`;
  } 
}
