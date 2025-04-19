// http.service.ts
import { Observable } from "rxjs";
import { HttpClient, HttpHeaders } from "@angular/common/http";

export class HttpService<T> {
    
    root: string = 'http://localhost:5193/api/';

    get path(): string { return this.root + this.endpoint };

    constructor(public http: HttpClient, public endpoint: string) { }

    getAll(): Observable<T[]> {
        return this.http.get<T[]>(this.path);
    }

    get(id: number): Observable<T> {
        return this.http.get<T>(`${this.path}/${id}`);
    }

    update(id: number, entity: T): Observable<T> {
        return this.http.patch<T>(
            `${this.path}/${id}`, 
            this.preparePayload(entity),
            {
                headers: new HttpHeaders({
                    'Content-Type': 'application/json',
                })
          }
        );
    }

    create(entity: T): Observable<T> {
        return this.http.post<T>(this.path, entity);
    }

    remove(id: number): Observable<any> {
        return this.http.delete<T>(`${this.path}/${id}`);
    }

    private preparePayload(entity: any): any {
        if (entity.dueDate instanceof Date) {
            return {
                ...entity,
                dueDate: entity.dueDate.toISOString()
            };
        }
        return entity;
    }
}