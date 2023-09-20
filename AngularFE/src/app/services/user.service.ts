// src/app/services/user.service.ts

import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { User } from '../models/user.model';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  private apiUrl = 'https://localhost:7003/api/v1/users';

  constructor(private http: HttpClient) {}

  getUsers(ascending:boolean = true, lastName:string = ''): Observable<User[]> {
    if(lastName != '') {
      return this.http.get<User[]>(`${this.apiUrl}?ascending=${ascending}&lastName=${lastName}`);
    }
    return this.http.get<User[]>(`${this.apiUrl}?ascending=${ascending}`);
  }

  getUser(id: number): Observable<User> {
    return this.http.get<User>(`${this.apiUrl}/${id}`);
  }

  createUser(user: User): Observable<User> {
    return this.http.post<User>(this.apiUrl, user);
  }

  updateUser(userId:number, user: User): Observable<User> {
    return this.http.put<User>(`${this.apiUrl}/${userId}`, user);
  }

  deleteUser(id: number): Observable<void> {
    return this.http.delete<void>(`${this.apiUrl}/${id}`);
  }
}
