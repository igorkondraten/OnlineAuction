import { Injectable } from '@angular/core';
import { Observable } from 'rxjs';
import { HttpClient } from '@angular/common/http';
import { Lot } from '../models/lot.model';
import { LotList } from '../models/lot-list.model';
import { User } from '../models/user.model';
import { UserList } from '../models/user-list.model';
import { catchError } from 'rxjs/operators';
import { ErrorHandlingService } from './error-handling.service';

@Injectable({
  providedIn: 'root'
})
export class UserService {
  USER_LIST_PATH = 'http://localhost:54741/api/users';

  constructor(private http: HttpClient, private errorHandlingService: ErrorHandlingService) { }

  public getAll(limit: number, offset: number): Observable<UserList> {
    let pages_path = `/?limit=${limit}&offset=${offset}`;
    return this.http.get<UserList>(this.USER_LIST_PATH + pages_path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public get(userId: number): Observable<User> {
    const path = `/${userId}`;
    return this.http.get<User>(this.USER_LIST_PATH + path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public delete(userId: number): Observable<{}> {
    const path = `/${userId}`;
    return this.http.delete<Lot>(this.USER_LIST_PATH + path).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public update(userId: number, user: User): Observable<{}> {
    return this.http.put(this.USER_LIST_PATH + `/${userId}`, user).pipe(
      catchError(this.errorHandlingService.handleError));
  }
}