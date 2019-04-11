import { Injectable } from '@angular/core';
import {HttpClient, HttpHeaders } from '@angular/common/http'
import { Observable, BehaviorSubject, of } from 'rxjs';
import { catchError, map, tap, delay } from 'rxjs/operators';
import { User } from '../models/user';
import { UserRegisterModel } from '../models/user-register-model';
import { UserLoginModel } from '../models/user-login-model';
import { Token } from '../models/token';
import { CurrentUserService } from './current-user.service';
import { ErrorHandlingService } from './error-handling.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  private currentUser$: BehaviorSubject<User>;
  private token: Token;

  constructor(currentUserService: CurrentUserService,
    private httpClient: HttpClient,
    private window: Window,
    private errorHandlingService: ErrorHandlingService) { 
    this.currentUser$ = new BehaviorSubject(currentUserService.currentUser);
  }

  public signUp(registerModel: UserRegisterModel): Observable<User> {
    const PATH = 'http://localhost:54741/api/users/';

    return this.httpClient.post<User>(PATH, registerModel).pipe(
      catchError(this.errorHandlingService.handleError));
  }

  public signIn(loginModel: UserLoginModel): Observable<Token> {
    const PATH = 'http://localhost:54741/Token';
    let body: URLSearchParams = new URLSearchParams(); 
    body.set('grant_type', "password"); 
    body.set('userName', loginModel.userName); 
    body.set('password', loginModel.password); 
    let options = {
      headers: new HttpHeaders().set('Content-Type', 'application/x-www-form-urlencoded')
    };
    return this.httpClient.post<Token>(PATH, body.toString(), options).pipe(
      tap(token => this.handleToken(token)),
      catchError(this.errorHandlingService.handleError)
    );
  }

  public signOut(): Observable<void> {
    return of(null).pipe(
      delay(1500),
      tap(() => {
        this.currentUser$.next(null);
        this.clearToken();
      })
    )
  }

  public isSignedIn(): Observable<boolean> {    
    return this.currentUser$.pipe(
      map(currentUser => !!currentUser)
    );
  }

  public isAdmin(): Observable<boolean>{
    return this.currentUser$.pipe(
      map(currentUser => !!currentUser && currentUser.role == "Admin")
    );
  }

  public isSeller(): Observable<boolean>{
    return this.currentUser$.pipe(
      map(currentUser => !!currentUser && currentUser.role == "Seller")
    );
  }

  public getCurrentUser(): Observable<User> {
    return this.currentUser$.asObservable();
  }

  private clearToken(): void {
    this.token = null;
    this.window.localStorage.removeItem('Token');
  }

  private handleToken(token: Token): void {
    this.window.localStorage.setItem('Token', token["access_token"]);
    this.token = token;
    let user = <User> {name: token.userName, role: token.role, email: token.email, userProfileId: token.id};
    this.currentUser$.next(user);
  }
}
