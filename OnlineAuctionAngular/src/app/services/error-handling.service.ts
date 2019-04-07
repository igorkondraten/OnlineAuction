import { Injectable } from '@angular/core';
import { Observable, throwError } from 'rxjs';
import { Router } from '@angular/router';

@Injectable({
  providedIn: 'root'
})
export class ErrorHandlingService {

  constructor(private router: Router) { 
  }

  // pay attention! this is an arrow function
  // uncomment console.logs, what is 'this' (current scope / context)?
  // uncomment handleError() below
  // look at console logs
  // this is undefine now. 
  // it's because context was lost due to call like 'catchError(this.errorHandlingSerivce.handleError)'

  public handleError = (error: any): Observable<any> => {
    console.error(error);

    // console.log(this);
    
    if (error.status === 401) {
      // console.log(this.router);
      this.router.navigate(['sign-in']);
    }
   
    return throwError(error);
  }

  // public handleError(error: any): Observable<any> {
  //   // here might be sending logs to back-end
  //   console.error(error);

  //   console.log(this);
    
  //   if (error.status === 401) {
  //     console.log(this.router);
  //     this.router.navigate(['sign-in']);
  //   }
   
  //   return throwError(error);
  // }
}
