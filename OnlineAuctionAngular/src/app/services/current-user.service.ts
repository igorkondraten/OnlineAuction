import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { User } from '../models/user';

@Injectable({
  providedIn: 'root'
})
export class CurrentUserService {

  public currentUser: User;

  constructor(private httpClient: HttpClient) {  }

  public loadCurrentUser(): Promise<boolean> {
    return new Promise((resolve, reject) => {
        return this.httpClient
        .get<User>('http://localhost:54741/api/users/current')
        .subscribe(currentUser => {
            this.currentUser = currentUser;
            console.log(this.currentUser);
            resolve(true);
        }, error => {
            if (error.status !== 401) {
                throw new Error(error);
            }
            resolve(true)
        });
    });
}
}