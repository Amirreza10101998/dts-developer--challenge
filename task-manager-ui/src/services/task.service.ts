import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { TaskItem } from '../models/taskItem';
import { HttpService } from './http.service';

@Injectable({
  providedIn: 'root'
})

export class TaskService extends HttpService<TaskItem> {
  constructor(http: HttpClient) {
    super(http, 'v1/task');
  }
}