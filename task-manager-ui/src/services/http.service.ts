// http.service.ts
import { Observable } from "rxjs";
import { HttpClient } from "@angular/common/http";

export class HttpService<T> {
    
    root: string = '/api/';

    get path(): string { return this.root + this.endpoint };

    constructor(public http: HttpClient, public endpoint: string) { }

    getAll(): Observable<T[]> {
        return this.http.get<T[]>(this.path);
    }

    get(id: number): Observable<T> {
        return this.http.get<T>(`${this.path}/${id}`);
    }

    update(id: number, entity: T): Observable<T> {
        return this.http.put<T>(`${this.path}/${id}`, entity);
    }

    create(entity: T): Observable<T> {
        return this.http.post<T>(this.path, entity);
    }

    remove(id: number): Observable<any> {
        return this.http.delete<T>(`${this.path}/${id}`);
    }
}