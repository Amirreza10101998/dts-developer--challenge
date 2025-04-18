import { TaskItemStatus } from "./taskItemStatus";

export interface TaskItem {
    id: number;
    title: string;
    description?: string;
    status: TaskItemStatus;
    createdAt: Date;
    dueDate?: Date;
}