import { Injectable } from '@angular/core';
import { Router, CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { AuthService } from '../services/auth.service';
import { Observable } from 'rxjs';
import { map } from 'rxjs/operators';

@Injectable({ providedIn: 'root' })
export class SellerGuard implements CanActivate {
    constructor(
        private router: Router,
        private authService: AuthService
    ) { }

    public canActivateChild(childRoute: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.isSellerOrAdmin();
    }

    public canActivate(route: ActivatedRouteSnapshot, state: RouterStateSnapshot): Observable<boolean> {
        return this.isSellerOrAdmin();
    }

    isSellerOrAdmin(): Observable<boolean>{
        return this.authService.getCurrentUser().pipe(
            map(user => user.role === 'Admin' || user.role === 'Seller')
        )
    }
}