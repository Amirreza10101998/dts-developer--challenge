import { Component } from '@angular/core';
import { MatCardModule } from '@angular/material/card';
import { TaskService } from '../../services/task.service';
import { TaskItem } from '../../models/taskItem';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { CommonModule } from '@angular/common';
import {MatProgressSpinnerModule} from '@angular/material/progress-spinner';
import {MatSlideToggleModule} from '@angular/material/slide-toggle';

@Component({
  selector: 'app-task-list',
  imports: [MatCardModule, MatFormFieldModule, MatInputModule, MatIconModule, CommonModule, MatProgressSpinnerModule, MatSlideToggleModule],
  standalone: true,
  templateUrl: './task-list.component.html',
  styleUrl: './task-list.component.css'
})
export class TaskListComponent{

  constructor(private taskService: TaskService) {}
  tasks: TaskItem[] = [];
  completedTasks: TaskItem[] = [];
  displayedColumns: string[] = ['status', 'title', 'dueDate'];
  isLoading: boolean = false;
  completedTaskView: boolean = false;

  ngOnInit() {
    this.loadTaskItems();
  };

  loadTaskItems() {
    this.isLoading = true;
    this.taskService.getAll().subscribe({
      next: (data) => {
        this.tasks = data;
        this.filterCompletedTasks();
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading tasks:', error);
        this.isLoading = false;
      }
    });
  };

  deleteTask(taskId:number){
    this.isLoading = true;
    this.taskService.remove(taskId).subscribe({
      next: () => {
        console.log('Task deleted:', taskId);
        this.loadTaskItems();
        this.isLoading = false;
        },
      error: (error) => {
        console.error('Error deleting task:', error);
        this.isLoading = false;
      }
    });
  };

  navigateToAddTask() {
    window.location.href = '/tasks/add';
  };

  naviageteToEditTask(taskId: number) {
    window.location.href = `/tasks/edit/${taskId}`;
  };

  filterCompletedTasks() {
    this.completedTasks = this.tasks.filter(task => task.status === 2);
  }

  toggleCompletedTaskView() {
    this.completedTaskView = !this.completedTaskView; // Toggle the value
    // Perform any other actions needed
  }
  
  getStatusText(status: number): string {
    switch(status) {
    case 0: return 'Not Started';
    case 1: return 'In Progress';
    case 2: return 'Completed';
    case 3: return 'On Hold';
    case 4: return 'Cancelled';
    default: return 'Unknown';
    };
  };
  
  getStatusClass(status: number): string {
  switch(status) {
    case 0: return 'bg-secondary'; // Not Started
    case 1: return 'bg-primary';   // In Progress
    case 2: return 'bg-success';   // Completed
    case 3: return 'bg-warning text-dark'; // On Hold
    case 4: return 'bg-danger';    // Cancelled
    default: return 'bg-light text-dark';
    };
  };
}
