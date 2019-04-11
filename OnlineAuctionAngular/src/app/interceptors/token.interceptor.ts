import { HttpInterceptor, HttpRequest, HttpHandler, HttpEvent } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Injectable } from '@angular/core';
import { AuthService } from '../services/auth.service';

@Injectable()
export class TokenInterceptor implements HttpInterceptor {

    constructor(private window: Window) {
    }

    intercept(req: HttpRequest<any>, next: HttpHandler): Observable<HttpEvent<any>> {      
      const token = this.window.localStorage.getItem('Token');
        req = req.clone({
            setHeaders: {
                Authorization: `Bearer ${token}`
            }
        });

        console.log(req);

        return next.handle(req);
    }
}
