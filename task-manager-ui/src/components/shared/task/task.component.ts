import { CommonModule } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { FormControl, FormGroup, Validators, ReactiveFormsModule } from '@angular/forms';
import { TaskService } from '../../../services/task.service';
import { TaskItem } from '../../../models/taskItem';
import { ActivatedRoute, Router } from '@angular/router';
import { TaskItemStatus } from '../../../models/taskItemStatus';

@Component({
  selector: 'app-task',
  standalone: true,
  imports: [CommonModule, ReactiveFormsModule],
  templateUrl: './task.component.html',
  styleUrls: ['./task.component.css']
})
export class TaskComponent implements OnInit {
  constructor(
    private taskService: TaskService,
    private route: ActivatedRoute,
    private router: Router
  ) { }

  isEdit: boolean = false;
  isLoading: boolean = false;
  taskId?: number;
  currentTask?: TaskItem;

  statusOptions = [
    { value: TaskItemStatus.NotStarted, label: 'Not Started' },
    { value: TaskItemStatus.InProgress, label: 'In Progress' },
    { value: TaskItemStatus.Completed, label: 'Completed' },
    { value: TaskItemStatus.OnHold, label: 'On Hold' },
    { value: TaskItemStatus.Cancelled, label: 'Cancelled' }
  ];

  taskForm = new FormGroup({
    title: new FormControl('', [Validators.required, Validators.minLength(3)]),
    description: new FormControl(''),
    dueDate: new FormControl<Date | null>(null),
    status: new FormControl<TaskItemStatus>(TaskItemStatus.NotStarted)
  });

  ngOnInit(): void {
    this.route.params.subscribe(params => {
      if (params['id']) {
        this.taskId = +params['id'];
        this.isEdit = true;
        this.loadTaskForEdit();
      }
    });
  }

  loadTaskForEdit(): void {
    if (!this.taskId) return;

    this.isLoading = true;
    this.taskService.get(this.taskId).subscribe({
      next: (task) => {
        this.currentTask = task;
        this.taskForm.patchValue({
          title: task.title,
          description: task.description || '',
          status: task.status,
          dueDate: task.dueDate ? new Date(task.dueDate) : null
        });
        this.isLoading = false;
      },
      error: (error) => {
        console.error('Error loading task:', error);
        this.isLoading = false;
        // Optionally navigate back or show error message
      }
    });
  }

  onSubmit(): void {
    if (this.taskForm.invalid) {
      this.taskForm.markAllAsTouched();
      return;
    }

    this.isEdit ? this.updateTask() : this.createTask();
  }

  createTask(): void {
    this.isLoading = true;
    const formValue = this.taskForm.value;
    
    const task: TaskItem = {
      title: formValue.title ?? '',
      description: formValue.description || undefined,
      status: Number(formValue.status) || TaskItemStatus.NotStarted,
      dueDate: formValue.dueDate ? new Date(formValue.dueDate) : undefined
    };

    this.taskService.create(task).subscribe({
      next: (data) => {
        this.isLoading = false;
        this.navigateToTaskList();
      },
      error: (error) => {
        console.error('Error creating task:', error);
        this.isLoading = false;
      }
    });
  }

  updateTask(): void {
    if (!this.taskId || !this.currentTask) return;

    this.isLoading = true;
    const formValue = this.taskForm.value;
    const updatedTask: TaskItem = {
      ...this.currentTask,
      title: formValue.title ?? this.currentTask.title,
      description: formValue.description || undefined,
      status: Number(formValue.status),
      dueDate: formValue.dueDate ? new Date(formValue.dueDate) : undefined,
      id: this.taskId
    };

    this.taskService.update(this.taskId, updatedTask).subscribe({
      next: (data) => {
        this.isLoading = false;
        this.navigateToTaskList();
      },
      error: (error) => {
        console.error('Error updating task:', error);
        this.isLoading = false;
      }
    });
  }

  navigateToTaskList(): void {
    this.router.navigate(['/tasks']);
  }

  getStatusText(status: TaskItemStatus): string {
    const option = this.statusOptions.find(opt => opt.value === status);
    return option ? option.label : 'Unknown';
  }

  getStatusClass(status: TaskItemStatus): string {
    switch(status) {
      case TaskItemStatus.NotStarted: return 'bg-secondary';
      case TaskItemStatus.InProgress: return 'bg-primary';
      case TaskItemStatus.Completed: return 'bg-success';
      case TaskItemStatus.OnHold: return 'bg-warning text-dark';
      case TaskItemStatus.Cancelled: return 'bg-danger';
      default: return 'bg-light text-dark';
    }
  }
}