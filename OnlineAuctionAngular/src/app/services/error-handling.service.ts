import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingService {

  constructor(private router: Router) { 
  }

  public handleError = (error: any): Observable<any> => {
    console.error(error);

    if (error.status === 401) {
      this.router.navigate(['sign-in']);
    }
   
    return throwError(error);
  }
}
